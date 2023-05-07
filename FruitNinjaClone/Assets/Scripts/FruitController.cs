using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [SerializeField] GameObject _wholeFruit;
    [SerializeField] GameObject _slicedFruit;
    [SerializeField] LayerMask _backgroundLayer;
    [SerializeField] SplashController _splashPrefab;
    [SerializeField] float _splashFadeTime=1f;
    Collider[] _colliders;
    Rigidbody[] _rigidbodies;
    ParticleSystem _particle;
    Color _splashColor;
    Transform _transform;
    public Rigidbody WholeFruitRb => _rigidbodies[0];
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

    }
    void HandleSlicedFruitsMovement(Vector3 bladeDirection, float bladeForce, Vector3 bladePos)
    {
        float additionalForce = Mathf.Clamp(bladeForce, 4f, 9f);
        float angle = Mathf.Atan2(bladeDirection.y, bladeDirection.x) * Mathf.Rad2Deg;
        _slicedFruit.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (bladeDirection.magnitude < 1f) 
        {
            bladeDirection = Vector3.down / 3f;   //parametric?
        }
        for (int i = 1; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].AddForceAtPosition(bladeDirection * additionalForce, bladePos, ForceMode.Impulse);
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
