using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour,IPoolable
{
    [SerializeField] GameObject _wholeFruit;
    [SerializeField] GameObject _slicedFruit;
    [SerializeField] LayerMask _backgroundLayer;
    [SerializeField] SplashController _splashPrefab;
    [SerializeField] float _splashFadeTime=1f;
    [SerializeField] PoolID _poolID;
    Collider[] _colliders;
    Rigidbody[] _rigidbodies;
    ParticleSystem _particle;
    Color _splashColor;
    Transform _transform;
    public Rigidbody WholeFruitRb => _rigidbodies[0];

    public PoolID PoolID => _poolID;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _particle = GetComponentInChildren<ParticleSystem>();
        _splashColor = GetComponentInChildren<MeshRenderer>().material.color;
        _transform = transform;
    }
    private void OnEnable()
    {
        GetUnsliced();
    }
    void GetUnsliced()
    {
        _wholeFruit.transform.localPosition = Vector3.zero;
        _rigidbodies[1].MovePosition(Vector3.zero);
        _rigidbodies[2].MovePosition(Vector3.zero);
        _slicedFruit.SetActive(false);
        _wholeFruit.SetActive(true);
    }
    public void GetSliced()
    {
        _transform.position = _wholeFruit.transform.position;
        _colliders[0].enabled = false;
        _slicedFruit.SetActive(true);
        for (int i = 1; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].velocity = _rigidbodies[0].velocity;
            _colliders[i].enabled = true;
        }
            
        _wholeFruit.SetActive(false);
        _particle.Play();
        HandleSplash();
        HandleSlicedFruitsMovement(BladeController.Instance.Direction, BladeController.Instance.Velocity, BladeController.Instance.TransformPos);
        GameManager.Instance.IncreaseScore();

    }

    void HandleSlicedFruitsMovement(Vector3 bladeDirection, float bladeForce, Vector3 bladePos)
    {
        float additionalForce = Mathf.Clamp(bladeForce, 0.5f, 3f);
        if (bladeDirection.magnitude < 0.2f)
        {

            bladeDirection = Vector3.down / 2;   //parametric?
        }
        float angle = Mathf.Atan2(bladeDirection.y, bladeDirection.x) * Mathf.Rad2Deg;
        _slicedFruit.transform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log(additionalForce);
        for (int i = 1; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].velocity = _rigidbodies[i].velocity * 0.3f;
            _rigidbodies[i].AddForceAtPosition(bladeDirection.normalized * additionalForce , bladePos, ForceMode.Impulse);
            _rigidbodies[i].velocity = new Vector3(_rigidbodies[i].velocity.x, _rigidbodies[i].velocity.y+2f, _rigidbodies[i].velocity.z);
        }

    }
    public void HandleSplash()
    {
        RaycastHit hit;
        if(Physics.Raycast(_transform.position, _transform.position-Camera.main.transform.position, out hit, 500f , _backgroundLayer))
        {
           SplashController newSplash = Instantiate(_splashPrefab, hit.point, hit.transform.rotation);
           newSplash.DoSplash(_splashColor, _splashFadeTime);
        }
          //  Debug.DrawRay(transform.position, (transform.position - Camera.main.transform.position)  * 500f, Color.yellow);
    }
    
}
