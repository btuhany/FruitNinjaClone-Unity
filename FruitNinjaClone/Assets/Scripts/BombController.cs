using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] PoolID _poolID;
    public PoolID PoolID => _poolID;
    public Rigidbody Rb;


    private void Awake()
    {
        Rb= GetComponent<Rigidbody>();
    }
    private void OnDisable()
    {
        Rb.velocity= Vector3.zero;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
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
