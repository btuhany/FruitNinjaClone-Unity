using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [SerializeField] GameObject _wholeFruit;
    [SerializeField] GameObject _slicedFruit;

    
    Collider[] _colliders;
    Rigidbody[] _rigidbodies;

    public Rigidbody WholeFruitRb => _rigidbodies[0];
    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
    }
    private void OnEnable()
    {
        _slicedFruit.SetActive(false);
        _wholeFruit.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetSliced();
            HandleSlicedFruitsMovement(BladeController.Instance.Direction, BladeController.Instance.Velocity, BladeController.Instance.TransformPos);
        }
       
    }
    public void GetSliced()
    {
        _colliders[0].enabled = false;
        _slicedFruit.SetActive(true);
        for (int i = 1; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].velocity = _rigidbodies[0].velocity;
            _colliders[i].enabled = true;
        }
        _wholeFruit.SetActive(false);


    }
    public void HandleSlicedFruitsMovement(Vector3 bladeDirection, float bladeForce, Vector3 bladePos)
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
    
}
