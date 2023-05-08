using UnityEngine;

public class BombController : MonoBehaviour, IPoolable
{
    [SerializeField] PoolID _poolID;
    public PoolID PoolID => _poolID;
    public Rigidbody Rb;


    private void Awake()
    {
        Rb= GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HandleOnBladeHit();
         

        }
    }
    void HandleOnBladeHit()
    {
        GameManager.Instance.GameOver();
    }
}
