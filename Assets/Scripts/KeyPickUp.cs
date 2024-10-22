using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>().isPlayerOne == false)
        {
            other.GetComponent<PlayerController>().CollectKey();

            Destroy(gameObject);
        }
    }
}
