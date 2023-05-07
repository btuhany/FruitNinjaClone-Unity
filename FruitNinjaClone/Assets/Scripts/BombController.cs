using UnityEngine;

public class BombController : MonoBehaviour
{
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
