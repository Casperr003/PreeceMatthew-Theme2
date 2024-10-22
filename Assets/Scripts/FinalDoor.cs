using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private UIcontroller uiController;

    private void Start()
    {
        // Find the UI controller in the scene
        uiController = FindObjectOfType<UIcontroller>();
    }

    private void Update()
    {
        // Check if the player has collected all keys
        if (uiController != null && uiController.GetTotalKeys() >= 7) // Use the method to get total keys
        {
            Destroy(gameObject); // Remove the FinalDoor from the game
        }
    }
}
