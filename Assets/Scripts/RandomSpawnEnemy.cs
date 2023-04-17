using System.Collections;
using Assets.Scripts.FactoryMethod;
using UnityEngine;

public class RandomSpawnEnemy : MonoBehaviour
{
	[SerializeField]
	private Sprite[] _enemy;

	GameObject _rangedEnemyPrefab;
	GameObject _closeCombatEnemyPrefab;

	private float _cameraHeight;
	private float _cameraWidth;
	private float _enermySpawnOffset = 1f;
	private Camera _mainCamera;
	private int _spawnLimit;
	private int _gameRoundSeconds;
	private int _gameRound;

	private AbstractEnemyFactory _enemyFactory;

	public int SpawnLimit { get => _spawnLimit; set => _spawnLimit = value; }




	private void Awake()
	{
		_gameRound = 0;
		_gameRoundSeconds = 60;
		_spawnLimit = 60;

		_mainCamera = Camera.main;
		_cameraHeight = 2f * _mainCamera.orthographicSize;
		_cameraWidth = _cameraHeight * _mainCamera.aspect;

		_rangedEnemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/RangedEnemy");
		_closeCombatEnemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/CloseCombatEnemy");

	}



	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(SpawnEnemyByRound());
		StartCoroutine(IncreaseEnemySpawnLimit());
		StartCoroutine(EnemyRushEvent());
	}




	private void SpawnEnemy()
	{
		Vector3 postion = GenerateRandomPosition();
		postion += GameObject.FindGameObjectWithTag("Player").transform.position;
		int randomSprite = Random.Range(0, _enemy.Length);



		SpriteRenderer playerSpriteRenderer = _closeCombatEnemyPrefab.GetComponent<SpriteRenderer>();
		playerSpriteRenderer.sprite = _enemy[randomSprite];


		GameObject enemyPrefab;
		switch (playerSpriteRenderer.sprite.name)
		{
			case DataPreserve.FODDER_JOE_SPRITE_NAME:
			case DataPreserve.BIG_DADDY_SPRITE_NAME:
			case DataPreserve.EXPLOSIVE_DAVE_SPRITE_NAME:

				enemyPrefab = _closeCombatEnemyPrefab;
				_enemyFactory = GetComponent<CloseCombatEnemyFactory>();
				break;


			default:
				enemyPrefab = _rangedEnemyPrefab;
				_enemyFactory = GetComponent<RangedCombatEnemyFactory>();
				break;

		}
		_enemyFactory.CreateEnemy(enemyPrefab, postion).transform.parent = transform;
	}



	IEnumerator SpawnEnemyByRound()
	{
		while (DataPreserve.totalEnemiesOnMap < 150)
		{
			float secondsBetweenSpawn = (float)_gameRoundSeconds / _spawnLimit;
			SpawnEnemy();
			yield return new WaitForSeconds(secondsBetweenSpawn);
		}
	}



	IEnumerator IncreaseEnemySpawnLimit()
	{
		while (_spawnLimit < 150)
		{
			yield return new WaitForSeconds(_gameRoundSeconds);
			_gameRound++;
			_spawnLimit += 10;
		}
	}



	IEnumerator EnemyRushEvent()
	{
		while (true)
		{
			yield return new WaitForSeconds(_gameRoundSeconds * 5);
			for (int i = 0; i < (_spawnLimit * 2); i++)
			{
				SpawnEnemy();
			}
		}
	}




	private Vector3 GenerateRandomPosition()
	{
		Vector3 cameraPosition = _mainCamera.transform.position;
		float leftBound = cameraPosition.x - _cameraWidth / 2f;
		float rightBound = cameraPosition.x + _cameraWidth / 2f;
		float bottomBound = cameraPosition.y - _mainCamera.orthographicSize;
		float topBound = cameraPosition.y + _mainCamera.orthographicSize;

		Vector3 positon;
		float randomDirection = UnityEngine.Random.Range(0f, 1f);
		if (randomDirection < 0.25f) // Spawn phía trên camera
		{
			positon = new Vector3(UnityEngine.Random.Range(leftBound, rightBound), topBound + _enermySpawnOffset, 0f);
		}
		else if (randomDirection < 0.5f) // Spawn phía bên phải camera
		{
			positon = new Vector3(rightBound + _enermySpawnOffset, UnityEngine.Random.Range(bottomBound, topBound), 0f);
		}
		else if (randomDirection < 0.75f) // Spawn phía dưới camera
		{
			positon = new Vector3(UnityEngine.Random.Range(leftBound, rightBound), bottomBound - _enermySpawnOffset, 0f);
		}
		else // Spawn phía bên trái camera
		{
			positon = new Vector3(leftBound - _enermySpawnOffset, UnityEngine.Random.Range(bottomBound, topBound), 0f);
		}
		return positon;
	}

}