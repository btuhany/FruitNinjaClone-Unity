using TMPro;
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
    public Vector3 Direction { get; private set; }
    public Vector3 TransformPos { get => _transform.position;}
    public float Velocity { get => Direction.magnitude/Time.deltaTime; }

    public static BladeController Instance;
    public bool CanReadInput = true;
    private void Awake()
    {
        Instance = this;
        CanReadInput = true;
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
        if (!CanReadInput) return;
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
        //if(newPos != _transform.position)
            Direction = (newPos - _transform.position);
        _transform.position = newPos;

        
    }


}
