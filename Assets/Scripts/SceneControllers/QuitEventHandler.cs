using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _pauseButton;

    private GameObject _pausePanel;

    private GameObject _confirmationPanel;

    private void Awake()
    {
        _pauseButton = GameObject.Find("PauseButton");
        _pausePanel = GameObject.Find("PausePanel");
        _confirmationPanel = GameObject.Find("ConfirmationPanel");
        _pausePanel.SetActive(false);
        _confirmationPanel.SetActive(false);
    }

    public void PauseButtonClick()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void ContinueButtonClick()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void QuitButtonClick()
    {
        _pausePanel.SetActive(false);
        _confirmationPanel.SetActive(true);
    }

    public void ConfirmButtonClick()
    {
        //Save game here
        GameObject spawner = GameObject.Find("SpawnEnemy");
        PlayableCharacterController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayableCharacterController>();

        int currentSpawnLimit = spawner.GetComponent<RandomSpawnEnermy>().SpawnLimit;
        int weaponType = 0;
        switch (playerController.GunSprite.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "Gun_3":
                weaponType = 0;
                break;

            case "Gun_10":
                weaponType = 1;
                break;

            case "Gun_11":
                weaponType = 2;
                break;

            case "Gun_5":
                weaponType = 3;
                break;
        }
        GameObject expBarReference = GameObject.Find("ExpBar");
        float currentExp = expBarReference.GetComponent<ExpBarController>().GetCurrentExp();
        DataManager dataManager = new DataManager();
        SaveData saveData = new SaveData()
        {
            CharacterNumber = DataPreserve.characterSelectedNumber,
            SurvivedTime = DataPreserve.survivedTime,
            Score = DataPreserve.totalScore,
            MaxHealth = playerController.MaxHealthPoint,
            CurrentHealth = playerController.CurrentHealthPoint,
            CurrentSpeed = playerController.DefaultSpeed,
            RequiredExp = playerController.MaxExp,
            CurrentExp = currentExp,
            Level = playerController.Level,
            WeaponType = weaponType,
            WeaponLevel = DataPreserve.gunLevel,
            SpawnLimit = currentSpawnLimit,
            NumberOfUpgrades = DataPreserve.numberOfUpgrades
        };
        dataManager.SaveData(saveData);
        Time.timeScale = 1;
        DataPreserve.ResetFields();
        SceneManager.LoadScene("SceneMainMenu");
    }

    public void DeclineButtonClick()
    {
        _pausePanel.SetActive(true);
        _confirmationPanel.SetActive(false);
    }
}