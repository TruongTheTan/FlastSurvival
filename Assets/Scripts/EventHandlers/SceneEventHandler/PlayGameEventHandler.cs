using UnityEngine;

public class PlayGameEventHandler : MonoBehaviour
{


    private GameObject _pauseButton;
    private GameObject _pausePanel;
    private GameObject _confirmationPanel;



    private void Awake()
    {
        _pauseButton = GameObject.Find("PauseGameButton");
        _pausePanel = GameObject.Find("PausePanel");
        _confirmationPanel = GameObject.Find("ConfirmationPanel");
        _pausePanel.SetActive(false);
        _confirmationPanel.SetActive(false);
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }






    public void PauseGameButtonClick()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
    }


    public void ContinueGameButtonClick()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
    }


    class QuitEventHandler { }
}
