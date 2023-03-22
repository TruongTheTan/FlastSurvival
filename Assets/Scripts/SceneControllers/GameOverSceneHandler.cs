using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneHandler : MonoBehaviour
{
    private TextMeshProUGUI _totalScoreText;
    private TextMeshProUGUI _survivedTimeText;

    private readonly string _totalScorePrefix = "Total Score: ";
    private readonly string _survivedTimePrefix = "Survived Time: ";

    private void Awake()
    {
        _totalScoreText = GameObject.Find("TotalScoreText").GetComponent<TextMeshProUGUI>();
        _survivedTimeText = GameObject.Find("SurvivedTimeText").GetComponent<TextMeshProUGUI>();

        string timed = GetSurvivedTime();

        _totalScoreText.text = _totalScorePrefix + DataPreserve.totalScore.ToString();
        _survivedTimeText.text = _survivedTimePrefix + timed;
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
        string saveFile = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(saveFile))
        {
            File.Delete(saveFile);
        }
        //Load main menu scene
        SceneManager.LoadScene("SceneMainMenu");
    }
}