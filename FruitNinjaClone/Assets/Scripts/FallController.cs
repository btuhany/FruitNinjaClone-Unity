using UnityEngine;

public class FallController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WholeFruit"))
        {
            if (!GameManager.Instance.IsGameOn) return;
            GameManager.Instance.DecreaseLive();
            SoundManager.Instance.PlaySound(8);
        }


    }
}
