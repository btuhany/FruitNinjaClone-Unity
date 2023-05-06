using TMPro;
using UnityEditor.Performance.ProfileAnalyzer;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    private bool _isActive;
    SphereCollider _collider;
    Camera _mainCam;
    Transform _transform;
    TrailRenderer _trail;
    float _initialMainCamZpos;
    float _initialBladeZpos;
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _mainCam = Camera.main;
        _transform = transform;
        _initialMainCamZpos = _mainCam.transform.position.z;
        _trail = GetComponentInChildren<TrailRenderer>();
        _initialBladeZpos = _transform.position.z;
    }
    private void OnEnable()
    {
        DeactivateBlade();
    }
    private void OnDisable()
    {
        DeactivateBlade();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ActivateBlade();
            
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            DeactivateBlade();
        }
        else if(_isActive)
        {
            HandleBladeSlicingActions();
        }

    }
    void ActivateBlade()
    {
        _isActive= true;
        _collider.enabled=true;
        _trail.Clear();
        _trail.enabled=true;
        HandleBladeSlicingActions();
    }
    void DeactivateBlade()
    {
        _isActive = false;
        _trail.enabled=false;
        _trail.Clear();
        _collider.enabled=false;

    }
    void HandleBladeSlicingActions()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -_initialMainCamZpos + _initialBladeZpos;
        Vector3 newPos = _mainCam.ScreenToWorldPoint(mousePos);
        //Debug.Log(newPos);
        newPos.z = _initialBladeZpos;
        _transform.position = newPos;
      
        
    }


}
