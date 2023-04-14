using UnityEngine;
/// <summary>
/// <para>
/// This class use for starting new game, loading saved game
/// </para>
/// </summary>
public class ScenePlayGameController : MonoBehaviour
{
	[SerializeField]
	private Sprite JohnSprite;

	[SerializeField]
	private Sprite AriahSprite;

	[SerializeField]
	private Sprite SteveSprite;


	[SerializeField]
	private Sprite _sword;

	[SerializeField]
	private Sprite _pistol;

	[SerializeField]
	private Sprite _shotgun;

	[SerializeField]
	private Sprite _assaultRifle;


	private GameObject _playableCharacter;



	private void Awake()
	{
		DataPreserve.SWORD_SPRITE = _sword;
		DataPreserve.PISTOL_SPRITE = _pistol;
		DataPreserve.SHOTGUN_SPRITE = _shotgun;
		DataPreserve.ASSAULT_RIFLE_SPRITE = _assaultRifle;


		DataPreserve.JOHN_SPRITE = JohnSprite;
		DataPreserve.ARIAH_SPRITE = AriahSprite;
		DataPreserve.STEVE_SPRITE = SteveSprite;

		if (DataPreserve.isNewGame)
			NewGame();
		else
			LoadGame();
	}



	// Update is called once per frame
	void Update()
	{
		DataPreserve.survivedTime += Time.deltaTime; // Calculate survival time
	}




	private void NewGame()
	{
		SpawnCharacterBySelectionNumber();
	}


	private void LoadGame()
	{
		SaveData saveData = new DataManager().ReadDataFromFile();

		DataPreserve.totalScore = saveData.Score;
		DataPreserve.survivedTime = saveData.SurvivedTime;
		DataPreserve.numberOfUpgrades = saveData.NumberOfUpgrades;
		DataPreserve.characterSelectedNumber = saveData.CharacterNumber;

		SpawnCharacterBySelectionNumber();

		PlayableCharacterController playerController = _playableCharacter.GetComponent<PlayableCharacterController>();
		RandomSpawnEnemy enemySpawnController = GameObject.Find("SpawnEnemy").GetComponent<RandomSpawnEnemy>();


		playerController.LoadSaveData(saveData);
		enemySpawnController.SpawnLimit = saveData.SpawnLimit;
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