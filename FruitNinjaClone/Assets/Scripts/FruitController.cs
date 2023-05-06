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
        }
        Debug.Log("yess");
    }
    public void GetSliced()
    {
        _colliders[0].enabled = false;
        _slicedFruit.SetActive(true);
        _rigidbodies[1].velocity = _rigidbodies[0].velocity;
        _rigidbodies[2].velocity = _rigidbodies[0].velocity;
        _colliders[1].enabled = true;
        _colliders[2].enabled = true;
        _wholeFruit.SetActive(false);

       
    }
}
