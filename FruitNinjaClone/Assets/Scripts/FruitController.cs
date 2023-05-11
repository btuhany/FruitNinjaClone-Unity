using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [SerializeField] GameObject _wholeFruit;
    [SerializeField] GameObject _slicedFruit;
    [SerializeField] LayerMask _backgroundLayer;
    [SerializeField] PoolID _poolID;
    [SerializeField] float _splashFadeTime=1f;
    [SerializeField] float _timeToSetPool=6f;

    Collider[] _colliders;
    Rigidbody[] _rigidbodies;
    ParticleSystem _particle;
    Color _splashColor;
    Transform _transform;
    public Transform[] _halfFruitsTransforms;
    public Rigidbody WholeFruitRb => _rigidbodies[0];
    

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _particle = GetComponentInChildren<ParticleSystem>();
        _splashColor = GetComponentInChildren<MeshRenderer>().material.color;
        _halfFruitsTransforms = _slicedFruit.GetComponentsInChildren<Transform>();
    
        _transform = transform;
    }
    private void OnEnable()
    {
 
        StartCoroutine(SetToObjectPool());
    }
    private void OnDisable()
    {
        GetUnsliced();
    }
    void GetUnsliced()
    {
        _wholeFruit.transform.localPosition = Vector3.zero;
        _slicedFruit.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //_rigidbodies[1].MovePosition(Vector3.zero);
        //_rigidbodies[2].MovePosition(Vector3.zero);
        for (int i = 1; i < _halfFruitsTransforms.Length; i++)
        {
            _halfFruitsTransforms[i].localPosition = Vector3.zero;
            _halfFruitsTransforms[i].localRotation = Quaternion.Euler(0, 0, 0);
            _rigidbodies[i].velocity = Vector3.zero;
            _colliders[i].enabled = false;
        }
        //_halfFruitsTransforms[1].localPosition= Vector3.zero;
        //_halfFruitsTransforms[2].localPosition = Vector3.zero;
        //_halfFruitsTransforms[1].localRotation = Quaternion.Euler(0,0,0);
        //_halfFruitsTransforms[2].localRotation = Quaternion.Euler(0, 0, 0);
        _halfFruitsTransforms[2].localScale = new Vector3(1, -1, 1);
        WholeFruitRb.velocity = Vector3.zero;
        _colliders[0].enabled = true; 
        _slicedFruit.SetActive(false);
        _wholeFruit.SetActive(true);
    }
    public void GetSliced()
    {
        BladeController.Instance.IncreaseComboCounter();
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
        SoundManager.Instance.PlaySliceSound();
    }

    void HandleSlicedFruitsMovement(Vector3 bladeDirection, float bladeForce, Vector3 bladePos)
    {
        float additionalForce = Mathf.Clamp(bladeForce, 0.5f, 3f);
        if (bladeDirection.magnitude < 0.1f)
        {

            bladeDirection = Vector3.down / 2;   //parametric?
        }
        float angle = Mathf.Atan2(bladeDirection.y, bladeDirection.x) * Mathf.Rad2Deg;
        _slicedFruit.transform.rotation = Quaternion.Euler(0, 0, angle);
        //Debug.Log(additionalForce);
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
            SplashController newSplash = ObjectPoolManager.Instance.GetSplashPrefab();
            newSplash.transform.position = hit.point;
            newSplash.transform.rotation = hit.transform.rotation;
          // SplashController newSplash = Instantiate(_splashPrefab, hit.point, hit.transform.rotation);
           newSplash.DoSplash(_splashColor, _splashFadeTime);
        }
          //  Debug.DrawRay(transform.position, (transform.position - Camera.main.transform.position)  * 500f, Color.yellow);
    }
    IEnumerator SetToObjectPool()
    {
        yield return new WaitForSeconds(_timeToSetPool);
        ObjectPoolManager.Instance.SetObjToPool(this, _poolID);

      
        yield return null;

    }
}
