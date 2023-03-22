using UnityEngine;

public class ScenePlayGameController : MonoBehaviour
{
    [SerializeField]
    private Sprite JohnSprite;

    [SerializeField]
    private Sprite AriahSprite;

    [SerializeField]
    private Sprite SteveSprite;

    private GameObject _playableCharacter;

    private void Awake()
    {
        if (DataPreserve.isNewGame)
        {
            SpawnCharacterBySelectionNumber();
        }
        else
        {
            //Load all saved data
            DataManager dataManager = new DataManager();
            SaveData saveData = dataManager.ReadData();
            DataPreserve.characterSelectedNumber = saveData.CharacterNumber;
            DataPreserve.survivedTime = saveData.SurvivedTime;
            DataPreserve.totalScore = saveData.Score;
            SpawnCharacterBySelectionNumber();

            PlayableCharacterController controller = _playableCharacter.GetComponent<PlayableCharacterController>();
            RandomSpawnEnermy spawnController = GameObject.Find("SpawnEnemy").GetComponent<RandomSpawnEnermy>();
            controller.LoadSaveData(saveData);
            DataPreserve.numberOfUpgrades = saveData.NumberOfUpgrades;
            spawnController.SetSpawnLimit(saveData.SpawnLimit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DataPreserve.survivedTime += Time.deltaTime;
    }

    private void SpawnCharacterBySelectionNumber()
    {
        GameObject playablePrefab = Resources.Load<GameObject>("Prefabs/PlayableCharacter/Player");
        _playableCharacter = Instantiate(playablePrefab, new Vector3(0, 0, 1), Quaternion.identity);

        SpriteRenderer playerSpriteRenderer = _playableCharacter.GetComponent<SpriteRenderer>();

        switch (DataPreserve.characterSelectedNumber)
        {
            case 1: playerSpriteRenderer.sprite = JohnSprite; break;
            case 2: playerSpriteRenderer.sprite = AriahSprite; break;
            case 3: playerSpriteRenderer.sprite = SteveSprite; break;
        }
    }
}