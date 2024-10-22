using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WingKey : MonoBehaviour
{
    public Text messageText; // Assign this in the Inspector to show the message
    private bool hasPlayerEntered = false; // To ensure the message is only shown once

    private void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false); // Make the text inactive at the start
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is a player
        if (collision.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true; // Prevent multiple triggers
            ShowEscapeMessage();
        }
    }

    private void ShowEscapeMessage()
    {
        // Show the message
        if (messageText != null)
        {
            messageText.text = "You Escaped!";
            messageText.gameObject.SetActive(true); // Ensure the message is visible
        }

        // Start the coroutine to wait and load the title scene
        StartCoroutine(LoadTitleSceneAfterDelay(5f));
    }

    private System.Collections.IEnumerator LoadTitleSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene("Title"); // Load the Title scene
    }
}
