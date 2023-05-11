using System.Collections;
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
    bool _canReadInput;
    bool _canMakeSound;
    float _initialTrailTime;
    public float ComboCounter;
    public event System.Action OnComboEvent;

    private void Awake()
    {
        Instance = this;
        _collider = GetComponent<SphereCollider>();
        _mainCam = Camera.main;
        _transform = transform;
        _initialMainCamZpos = _mainCam.transform.position.z;
        _trail = GetComponentInChildren<TrailRenderer>();
        _initialTrailTime = _trail.time;
        _initialBladeZpos = _transform.position.z;
        _canMakeSound = true;
    }
    private void OnEnable()
    {
        DeactivateBlade();
        GameManager.Instance.OnGameOver += HandleOnGameOver;
        GameManager.Instance.OnGameStart += HandleOnGameStart;
    }
    void HandleOnGameOver()
    {
        _canReadInput = false;
        StartCoroutine(TrailEffect());
    }
    void HandleOnGameStart()
    {
        _canReadInput = true;
        //_trail.time = _initialTrailTime;
        DeactivateBlade();
    }
    private void OnDisable()
    {
        DeactivateBlade();
    }
    
    private void Update()
    {
        if (!_canReadInput) return;
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
        //Debug.Log(newPos.magnitude - _transform.position.magnitude);
        if (Mathf.Abs(newPos.magnitude - _transform.position.magnitude) > 0.06f && _canMakeSound)
        {
            SoundManager.Instance.PlaySoundRandomPitch(1,0.85f,1.1f);
            StartCoroutine(SoundDelay());
        }
        if(newPos != _transform.position)
            Direction = (newPos - _transform.position);
        _transform.position = newPos;

        
    }
    WaitForSeconds soundDelay = new WaitForSeconds(0.2f);
    WaitForSeconds trailEffect = new WaitForSeconds(0.7f);
    IEnumerator SoundDelay()
    {
        _canMakeSound= false;
        yield return soundDelay;
        _canMakeSound = true;
        yield return null;

    }
    IEnumerator TrailEffect()
    {
        _trail.time = 10f;
        yield return trailEffect;
        _trail.time = _initialTrailTime;
        yield return null;
    }
    IEnumerator comboCountProcess;
    public void IncreaseComboCounter()
    { 
        if(comboCountProcess != null)
            StopCoroutine(comboCountProcess);
        comboCountProcess = ComboCountProcess();
        StartCoroutine(comboCountProcess);
        
    }
    WaitForSeconds _comboCooldown = new WaitForSeconds(0.3f);
    IEnumerator ComboCountProcess()
    {
        ComboCounter++;
        yield return _comboCooldown;
        if (ComboCounter > 2)
        {
            SoundManager.Instance.PlaySoundRandomPitch(9,1f,1.35f);
            OnComboEvent?.Invoke();
        }
        ComboCounter = 0;
        yield return null;
    }

}
