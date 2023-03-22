using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEventHandler : MonoBehaviour
{
    private bool _isLoadable = false;
    private GameObject _mainMenuPanel;
    private GameObject _confirmationPanel;
    private GameObject _loadButton;
    private string _saveFile;

    private void Awake()
    {
        _saveFile = Application.persistentDataPath + "/savedata.json";

        _isLoadable = File.Exists(_saveFile) && !string.IsNullOrWhiteSpace(
            File.ReadAllText(_saveFile).Replace("{", string.Empty).Replace("}", string.Empty)
            );

        _mainMenuPanel = GameObject.Find("MainMenuPanel");
        _confirmationPanel = GameObject.Find("ConfirmationPanel");
        _loadButton = GameObject.Find("LoadGameButton");
        _confirmationPanel.SetActive(false);
        _loadButton.SetActive(_isLoadable);
    }

    public void NewGameButtonClick()
    {
        DataPreserve.isNewGame = true;
        File.Delete(_saveFile);
        SceneManager.LoadScene("SceneChooseCharacter");
    }

    public void LoadButtonClick()
    {
        DataPreserve.isNewGame = false;
        SceneManager.LoadScene(2);
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