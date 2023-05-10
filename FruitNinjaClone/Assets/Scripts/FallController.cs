using UnityEngine;

public class FallController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WholeFruit"))
            GameManager.Instance.DecreaseLive();
            
    }
}
