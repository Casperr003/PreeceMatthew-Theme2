using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour
{
    // GameUI
    public Button[] buttons;
    private int selectedIndex = 0;
    private InputSystem_Actions inputActions;
    // MainMenuUI
    public Text keyCountText;
    private int totalKeys = 0;
    private const int maxKeys = 7;

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.UI.Navigate.performed += ctx => OnNavigate(ctx.ReadValue<Vector2>());
        inputActions.UI.Submit.performed += ctx => OnSubmit();

        inputActions.Enable();
    }

    void Start()
    {
        UpdateUIForCurrentScene();

        foreach (Button button in buttons)
        {
            int index = System.Array.IndexOf(buttons, button);
            button.onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void UpdateUIForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Title")
        {
            EnableButtons(true);
            UpdateButtonSelection();
            keyCountText.gameObject.SetActive(false);
        }
        else if (currentSceneName == "GameLevel")
        {
            EnableButtons(false);
            keyCountText.gameObject.SetActive(true);
            UpdateKeyCount(totalKeys);
        }
    }

    void OnNavigate(Vector2 direction)
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            if (direction.y > 0)
            {
                selectedIndex--;
            }
            else if (direction.y < 0)
            {
                selectedIndex++;
            }

            if (selectedIndex < 0) selectedIndex = buttons.Length - 1;
            if (selectedIndex >= buttons.Length) selectedIndex = 0;

            UpdateButtonSelection();
        }
    }

    void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Graphic>().color = (i == selectedIndex) ? Color.yellow : Color.white;
        }

        buttons[selectedIndex].Select();
    }

    void OnSubmit()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            if (selectedIndex == 0)
            {
                Debug.Log("Starting the game!");
                SceneManager.LoadScene("GameLevel");
            }
            else if (selectedIndex == 1)
            {
                Debug.Log("Quitting the game!");
                QuitGame();
            }
        }
    }

    public void OnButtonClick(int buttonIndex)
    {
        if (buttonIndex == 0)
        {
            SceneManager.LoadScene("GameLevel");
        }
        else if (buttonIndex == 1)
        {
            QuitGame();
        }
    }

    public void UpdateKeyCount(int keys)
    {
        totalKeys += keys;
        totalKeys = Mathf.Clamp(totalKeys, 0, maxKeys);
        keyCountText.text = "Keys: " + totalKeys + "/" + maxKeys;
    }

    // New method to get the total number of keys collected
    public int GetTotalKeys()
    {
        return totalKeys; // Return the total keys collected
    }

    void EnableButtons(bool enable)
    {
        foreach (Button button in buttons)
        {
            button.interactable = enable;
        }
    }

    void QuitGame()
    {
        Debug.Log("Quitting the game...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
#else
        Application.Quit();
#endif
    }
}
