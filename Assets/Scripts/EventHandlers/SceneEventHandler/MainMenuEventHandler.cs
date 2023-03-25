using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEventHandler : MonoBehaviour
{
    private bool _isLoadable = false;
    private GameObject _mainMenuPanel;
    private GameObject _confirmationPanel;
    private GameObject _loadGameButton;
    private string _saveFile;



    private void Awake()
    {
        InstaniateData();
    }



    private void InstaniateData()
    {
        _saveFile = DataPreserve.saveFilePath;


        _isLoadable = File.Exists(_saveFile) && !string.IsNullOrWhiteSpace(
            File.ReadAllText(_saveFile).Replace("{", string.Empty).Replace("}", string.Empty));

        _mainMenuPanel = GameObject.Find("MainMenuPanel");
        _loadGameButton = GameObject.Find("LoadGameButton");
        _confirmationPanel = GameObject.Find("ConfirmationPanel");

        _confirmationPanel.SetActive(false);
        _loadGameButton.SetActive(_isLoadable);
    }


    public void NewGameButtonClick()
    {
        DataPreserve.isNewGame = true;
        DataPreserve.characterSelectedNumber = 1;
        SceneManager.LoadScene("SceneChooseCharacter");
    }


    public void LoadGameButtonClick()
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


    /// <summary>
    /// Quit game (app)
    /// </summary>
    public void QuitConfirmButtonClick()
    {
        Application.Quit();
    }


    /// <summary>
    /// Continue staying in main menu
    /// </summary>
    public void QuitDeclineButtonClick()
    {
        if (_mainMenuPanel != null && _confirmationPanel != null)
        {
            _mainMenuPanel.SetActive(true);
            _confirmationPanel.SetActive(false);
        }
    }
}