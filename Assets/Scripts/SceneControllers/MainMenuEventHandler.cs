using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEventHandler : MonoBehaviour
{
    private GameObject _mainMenuPanel;
    private GameObject _confirmationPanel;

    private void Awake()
    {
        _mainMenuPanel = GameObject.Find("MainMenuPanel");
        _confirmationPanel = GameObject.Find("ConfirmationPanel");
        _confirmationPanel.SetActive(false);
    }

    public void NewGameButtonClick()
    {
        //Load choose character scene here
        SceneManager.LoadScene("SceneChooseCharacter");
    }

    public void QuitButtonClick()
    {
        if (_mainMenuPanel != null && _confirmationPanel != null)
        {
            _mainMenuPanel.SetActive(false);
            _confirmationPanel.SetActive(true);
        }
    }

    public void QuitConfirmButtonClick()
    {
        Application.Quit();
    }

    public void QuitDeclineButtonClick()
    {
        if (_mainMenuPanel != null && _confirmationPanel != null)
        {
            _mainMenuPanel.SetActive(true);
            _confirmationPanel.SetActive(false);
        }
    }
}