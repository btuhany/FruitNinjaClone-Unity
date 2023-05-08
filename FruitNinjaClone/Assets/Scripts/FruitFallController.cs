using UnityEngine;

public class FruitFallController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WholeFruit"))
            GameManager.Instance.DecreaseLive();
  
            
    }
}
