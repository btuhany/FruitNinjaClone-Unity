using System;
using System.Collections.Generic;
using System.Linq;
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
	[Serializable] struct Pool
	{
		public Queue<MonoBehaviour> PrefabQueue;  //? MonoBehaviour
		public MonoBehaviour Prefab;
		public int PoolSize;
		public PoolID PoolID; 
	}
	[SerializeField] Pool[] _objectPools;
	public static ObjectPoolManager Instance;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		InitializePools();
	}
	void InitializePools()
	{
		for (int i = 0; i < _objectPools.Length; i++)
		{			
			_objectPools[i].PrefabQueue = new Queue<MonoBehaviour>();
			for (int j = 0; j < _objectPools[i].PoolSize; j++)
			{
				MonoBehaviour newPrefab = Instantiate(_objectPools[i].Prefab, this.transform);
				newPrefab.gameObject.SetActive(false);
                _objectPools[i].PrefabQueue.Enqueue(newPrefab);
            }
		}
	}
	public MonoBehaviour GetObjFromPool(PoolID poolID)
	{
		Pool pool = FindPool(poolID);
		if(pool.PrefabQueue.Count<=0)
		{
			IncreasePoolSize(pool, _poolIncrementAtZeroCount);
		}
        MonoBehaviour poolObj =pool.PrefabQueue.Dequeue();
		poolObj.gameObject.SetActive(true);
		return poolObj;
	}
	public void SetObjToPool(MonoBehaviour poolObj, PoolID poolID)
	{
		poolObj.gameObject.SetActive(false);
		FindPool(poolID).PrefabQueue.Enqueue(poolObj);
	}
	void IncreasePoolSize(Pool pool, int increment)
	{
		for (int i = 0; i < increment; i++)
		{
            MonoBehaviour newPrefab = Instantiate(pool.Prefab, this.transform);
            newPrefab.gameObject.SetActive(false);
            pool.PrefabQueue.Enqueue(newPrefab);
        }
	}
	Pool FindPool(PoolID poolID)
	{
        foreach (Pool pool in _objectPools)
        {
            if (pool.PoolID == poolID)
                return pool;
        }
		return _objectPools[0];
    }

}
