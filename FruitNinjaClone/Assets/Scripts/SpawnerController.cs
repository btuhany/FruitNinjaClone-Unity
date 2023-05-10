using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] float _minThrowForce;
    [SerializeField] float _maxThrowForce;
    [SerializeField] [Range(0,1)] float _bombProbability = 0.2f;
    [SerializeField][Range(-80, 0)] float _minAngle;
    [SerializeField][Range(0, 80)] float _maxAngle;
    [SerializeField] Transform _minSpawnPointTransform;
    [SerializeField] Transform _maxSpawnPointTransform;

    Transform _transform;
    private void Awake()
    {
        _transform = this.transform;
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            SpawnThrowRandomFruitPosAngle();
        }
        else if(Input.GetKeyDown(KeyCode.F))
        {
            SpawnThrowRandomFruitPosAngle();
        }

    }
    void SpawnThrowRandomFruitPosAngle()
    {
        //Order is important
        SetRandomPos();
        SetRandomAngle();
        SpawnAndThrowRandomFruitOrBomb();
    }
    
    void SpawnAndThrowRandomFruitOrBomb()
    {
        if(Random.value<_bombProbability)
        {
            BombController newBomb = ObjectPoolManager.Instance.GetBombPrefab();
            newBomb.transform.position = this.transform.position;
           // BombController newBomb = Instantiate(_bombPrefab, _transform.position, _transform.rotation);
            newBomb.Rb.AddForce(_transform.up * Random.Range(_minThrowForce, _maxThrowForce));
            newBomb.Rb.AddTorque(_transform.forward * Random.Range(-300f,300f));
        }
        else
        {
            // FruitController newFruitRb = Instantiate(_fruitPrefabs[Random.Range(0, _fruitPrefabs.Length)], _transform.position, _transform.rotation);
            PoolID randomFruitPoolId;
            switch (Random.Range(0,5))
            {
                case 1:
                    randomFruitPoolId = PoolID.Apple;
                    break;
                case 2:
                    randomFruitPoolId = PoolID.Watermelon;
                    break;
                case 3:
                    randomFruitPoolId = PoolID.Kiwi;
                    break;
                case 4:
                    randomFruitPoolId = PoolID.Lemon;
                    break;
                case 0:
                    randomFruitPoolId = PoolID.Orange;
                    break;
                default:
                    randomFruitPoolId = PoolID.Apple;
                    break;
            }
            FruitController newFruit = ObjectPoolManager.Instance.GetFruitPrefab(randomFruitPoolId);
            newFruit.transform.position = this.transform.position;
            newFruit.transform.rotation = this.transform.rotation;
            newFruit.WholeFruitRb.AddForce(_transform.up * Random.Range(_minThrowForce,_maxThrowForce));
            //Can be added AddTorque
        }
    }
    void SetRandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(_minSpawnPointTransform.position.x, _maxSpawnPointTransform.position.x), _transform.position.y, _transform.position.z);
        _transform.position = randomPos;
    }
    void SetRandomAngle()
    {
        Quaternion randomRotation;
        if (_transform.position.x>(_minSpawnPointTransform.position.x + _maxSpawnPointTransform.position.x)/2)
        {
            randomRotation = Quaternion.Euler(0, 0, Random.Range(0, _maxAngle));
        }
        else
        {
            randomRotation = Quaternion.Euler(0, 0, Random.Range(_minAngle, 0));  
        }
        _transform.rotation = randomRotation;
    }
}
