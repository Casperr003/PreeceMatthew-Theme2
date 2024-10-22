using UnityEngine;
using UnityEngine.UI;
using System.Collections;  

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public Text scoreText;              
    public Text levelCompleteText;      
    private int score = 0;              
    private int totalTargets = 10;      
    public float delayBeforeQuit = 10f; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
        levelCompleteText.gameObject.SetActive(false);  
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        CheckLevelComplete();  
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score + "/" + totalTargets;
    }

    private void CheckLevelComplete()
    {
        if (score >= totalTargets)
        {
            levelCompleteText.gameObject.SetActive(true);

            StartCoroutine(QuitAfterDelay());
        }
    }

    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeQuit);

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
