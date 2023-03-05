using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneHandler : MonoBehaviour
{
    private TextMeshProUGUI _totalScoreText;
    private TextMeshProUGUI _survivedTimeText;

    private string totalScorePrefix = "Total Score: ";
    private string survivedTimePrefix = "Survived Time: ";

    private void Awake()
    {
        _totalScoreText = GameObject.Find("TotalScoreText").GetComponent<TextMeshProUGUI>();
        _survivedTimeText = GameObject.Find("SurvivedTimeText").GetComponent<TextMeshProUGUI>();

        SetTestData();

        string timed = GetSurvivedTime();

        _totalScoreText.text = totalScorePrefix + DataPreserve.totalScore.ToString();
        _survivedTimeText.text = survivedTimePrefix + timed;
    }

    private string GetSurvivedTime()
    {
        int minutes = Mathf.FloorToInt(DataPreserve.survivedTime / 60f);
        int seconds = Mathf.FloorToInt(DataPreserve.survivedTime % 60f);
        return minutes.ToString() + ":" + seconds.ToString();
    }

    public void ExitButtonClick()
    {
        //Reset saved score and time
        DataPreserve.totalScore = 0;
        DataPreserve.survivedTime = 0;

        //Load main menu scene
        SceneManager.LoadScene("SceneMainMenu");
    }

    private static void SetTestData()
    {
        if (DataPreserve.survivedTime == 0 && DataPreserve.totalScore == 0)
        {
            DataPreserve.survivedTime = 500;
            DataPreserve.totalScore = 2500;
        }
    }
}