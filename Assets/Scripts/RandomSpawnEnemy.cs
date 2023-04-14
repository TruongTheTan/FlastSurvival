using System.Collections;
using UnityEngine;

public class RandomSpawnEnemy : MonoBehaviour
{
	[SerializeField]
	private Sprite[] _enemy;

	private float _cameraHeight;
	private float _cameraWidth;
	private float _enermySpawnOffset = 1f;
	private Camera _mainCamera;
	private GameObject _newEnermy;
	private int _spawnLimit;
	private int _gameRoundSeconds;
	private int _gameRound;

	public int SpawnLimit { get => _spawnLimit; set => _spawnLimit = value; }


	private void Awake()
	{
		_gameRound = 0;
		_gameRoundSeconds = 60;
		_spawnLimit = 60;
	}

	// Start is called before the first frame update
	void Start()
	{
		_mainCamera = Camera.main;
		_cameraHeight = 2f * _mainCamera.orthographicSize;
		_cameraWidth = _cameraHeight * _mainCamera.aspect;

		StartCoroutine(nameof(SpawnEnemyByRound));
		StartCoroutine(nameof(IncreaseEnemySpawnLimit));
		StartCoroutine(nameof(EnemyRushEvent));
	}

	private void SpawnEnemy()
	{
		Vector3 postion = GenerateRandomPosition();
		postion += GameObject.FindGameObjectWithTag("Player").transform.position;
		int randomSprite = Random.Range(0, _enemy.Length);

		GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/Enemy");
		SpriteRenderer playerSpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
		playerSpriteRenderer.sprite = _enemy[randomSprite];

		_newEnermy = Instantiate(enemyPrefab, postion, Quaternion.identity);
		_newEnermy.transform.parent = transform;
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