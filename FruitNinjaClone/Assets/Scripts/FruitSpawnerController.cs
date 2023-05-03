using UnityEngine;

public class FruitSpawnerController : MonoBehaviour
{
    [SerializeField] float _minThrowForce;
    [SerializeField] float _maxThrowForce;
    [SerializeField] Rigidbody[] _fruitPrefabs;
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
    }
    void SpawnThrowRandomFruitPosAngle()
    {
        SetRandomPos();
        SetRandomAngle();
        SpawnAndThrowRandomFruit();
    }
    void SpawnAndThrowRandomFruit()
    {
        Rigidbody newFruitRb = Instantiate(_fruitPrefabs[Random.Range(0, _fruitPrefabs.Length)], this.transform.position, this.transform.rotation);
        newFruitRb.AddForce(_transform.up * Random.Range(_minThrowForce,_maxThrowForce));
    }
    void SetRandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(_minSpawnPointTransform.position.x, _maxSpawnPointTransform.position.x), _transform.position.y, _transform.position.z);
        _transform.position = randomPos;
    }
    void SetRandomAngle()
    {
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(_minAngle, _maxAngle));
        _transform.rotation = randomRotation;
    }
}
