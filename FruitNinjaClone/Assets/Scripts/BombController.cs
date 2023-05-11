using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] PoolID _poolID;
    public PoolID PoolID => _poolID;
    public Rigidbody Rb;
    bool _isExploded;

    private void Awake()
    {
        Rb= GetComponent<Rigidbody>();
    }
    private void OnDisable()
    {
        Rb.velocity= Vector3.zero;
        
    }
    private void Update()
    {
        if(Rb.velocity.y<-0.5f)
            Rb.velocity += Vector3.up * Physics2D.gravity.y * 3f * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isExploded || !GameManager.Instance.IsGameOn) return;
        if(other.CompareTag("Player"))
        {
            _isExploded = true;
            SoundManager.Instance.PlayBombSliceSound();
            HandleOnBladeHit();
        }
        else if(other.CompareTag("Fall"))
        {
            ObjectPoolManager.Instance.SetObjToPool(this, PoolID.Bomb);
        }
    }
    void HandleOnBladeHit()
    {
        GameManager.Instance.GameOver();
    }
}
