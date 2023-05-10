using UnityEngine;

public class WholeFruitController : MonoBehaviour
{
    FruitController _fruitController;
    private void Awake()
    {
        _fruitController = GetComponentInParent<FruitController>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("!!");
            _fruitController.GetSliced();

        }

    }
}
