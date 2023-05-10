using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolID
{
	Apple,
	Watermelon,
	Lemon,
	Kiwi,
	Orange,
	Bomb,
	Splash
}
public class ObjectPoolManager : MonoBehaviour
{
	[SerializeField] int _poolIncrementAtZeroCount = 3;
	[Serializable] public class Pool<T>
	{
        public Queue<T> PrefabQueue;  //? MonoBehaviour
		public T Prefab;
		public int PoolSize;
		public PoolID PoolID; 
	}
	
	[SerializeField] Pool<FruitController>[] _fruitObjectPools;
    [SerializeField] Pool<BombController> _bombObjectPool;
    [SerializeField] Pool<SplashController> _splashObjectPool;

    public static ObjectPoolManager Instance;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
        InitializePools(_fruitObjectPools);
        InitializePool(_bombObjectPool);
        InitializePool(_splashObjectPool);
    }
    void InitializePool<T>(Pool<T> pool) where T : MonoBehaviour
    {
		pool.PrefabQueue = new Queue<T>();

        for (int j = 0; j < pool.PoolSize; j++)
        {
            T newPrefab = Instantiate(pool.Prefab, this.transform);
            newPrefab.gameObject.SetActive(false);
            pool.PrefabQueue.Enqueue(newPrefab);
        }
	}
    void InitializePools<T>(Pool<T>[] pools) where T : MonoBehaviour
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].PrefabQueue = new Queue<T>();
            for (int j = 0; j < pools[i].PoolSize; j++)
            {
                T newPrefab = Instantiate(pools[i].Prefab, this.transform);
                newPrefab.gameObject.SetActive(false);
                pools[i].PrefabQueue.Enqueue(newPrefab);
            }
        }
    }

    public FruitController GetFruitPrefab(PoolID poolID)
    {
        for (int i = 0; i < _fruitObjectPools.Length; i++)
        {
            if (_fruitObjectPools[i].PoolID == poolID)
            {
                if (_fruitObjectPools[i].PrefabQueue.Count <= 0)
                    IncreasePoolSize(_fruitObjectPools[i]);
                FruitController fruit = _fruitObjectPools[i].PrefabQueue.Dequeue();
                fruit.gameObject.SetActive(true);
                return fruit;
            }
        }
        return null;
    }
    public BombController GetBombPrefab()
    {
        if (_bombObjectPool.PrefabQueue.Count <= 0)
            IncreasePoolSize(_bombObjectPool);
        BombController bomb = _bombObjectPool.PrefabQueue.Dequeue();
        bomb.gameObject.SetActive(true);
        return bomb;
    }
    public SplashController GetSplashPrefab()
    {
        if (_splashObjectPool.PrefabQueue.Count <= 0)
            IncreasePoolSize(_splashObjectPool);
        SplashController splash = _splashObjectPool.PrefabQueue.Dequeue();
        splash.gameObject.SetActive(true);
        return splash;
    }
    void IncreasePoolSize<T>(Pool<T> pool) where T: MonoBehaviour
    {
        for (int i = 0; i < _poolIncrementAtZeroCount; i++)
        {
            T newPrefab = Instantiate(pool.Prefab, this.transform);
            newPrefab.gameObject.SetActive(false);
            pool.PrefabQueue.Enqueue(newPrefab);
        }
    }
    public void SetObjToPool<T>(T poolObj, PoolID poolID) where T : MonoBehaviour
    {
        poolObj.gameObject.SetActive(false);
        if (poolObj is BombController bomb)
        {
            _bombObjectPool.PrefabQueue.Enqueue(bomb);
        }
        else if (poolObj is SplashController splash)
        {
            _splashObjectPool.PrefabQueue.Enqueue(splash);
        }
        else if (poolObj is FruitController fruit)
        {
            for (int i = 0; i < _fruitObjectPools.Length; i++)
            {
                if (_fruitObjectPools[i].PoolID == poolID)
                {
                    _fruitObjectPools[i].PrefabQueue.Enqueue(fruit);
                }
            }
        }
    }
    //public void SetObjToPool(MonoBehaviour poolObj, PoolID poolID)
    //{
    //	poolObj.gameObject.SetActive(false);
    //	FindPool(poolID).PrefabQueue.Enqueue(poolObj);
    //}
    //void IncreasePoolSize(Pool pool, int increment)
    //{
    //	for (int i = 0; i < increment; i++)
    //	{
    //           MonoBehaviour newPrefab = Instantiate(pool.Prefab, this.transform);
    //           newPrefab.gameObject.SetActive(false);
    //           pool.PrefabQueue.Enqueue(newPrefab);
    //       }
    //}
    //Pool<T> FindPool<T>(PoolID poolID) where T : MonoBehaviour
    //{
    //    if(poolID == PoolID.Bomb && _bombObjectPool is Pool<T> bombPool)
    //    {
    //        return bombPool;
    //    }
    //    else if(poolID == PoolID.Splash && _splashObjectPool is Pool<T> splashPool)
    //    {
    //        return splashPool;
    //    }
    //    else
    //    {
    //        foreach (Pool<FruitController> pool in _fruitObjectPools)
    //        {
    //            if (pool.PoolID == poolID && pool is Pool<T> fruitPool)
    //                return fruitPool as Pool<T>;
    //        }

    //    }

    //        return fruitPool;

    //}
}
