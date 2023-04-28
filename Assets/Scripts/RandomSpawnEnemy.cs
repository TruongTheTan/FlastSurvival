using System.Collections;
using Assets.Scripts.DesignPatterns.FactoryMethod;
using UnityEngine;

public class RandomSpawnEnemy : MonoBehaviour
{
	#region Properties

	private int _gameRound;
	private int _spawnLimit;
	private float _cameraWidth;
	private float _cameraHeight;
	private int _gameRoundSeconds;
	private const float _enemySpawnOffset = 1f;

	#endregion



	private Camera _mainCamera;
	private CloseCombatEnemyFactory _closeCombatEnemyFactory;
	private RangedCombatEnemyFactory _rangedCombatEnemyFactory;

	public int SpawnLimit { get => _spawnLimit; set => _spawnLimit = value; }




	private void Awake()
	{
		_gameRound = 0;
		_spawnLimit = 60;
		_gameRoundSeconds = 60;

		_mainCamera = Camera.main;
		_cameraHeight = 2f * _mainCamera.orthographicSize;
		_cameraWidth = _cameraHeight * _mainCamera.aspect;

		/* Singleton */
		_closeCombatEnemyFactory = CloseCombatEnemyFactory.GetInstance();
		_rangedCombatEnemyFactory = RangedCombatEnemyFactory.GetInstance();
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
		Vector3 spawnPosition = GenerateRandomPosition();
		spawnPosition += DataPreserve.player.transform.position;


		int randomSpawnNumber = Random.Range(1, 4);
		switch (randomSpawnNumber)
		{
			case (int)EnemyEnum.FodderJoe:
			case (int)EnemyEnum.BigDaddy:
			case (int)EnemyEnum.ExplosiveDave:
				_closeCombatEnemyFactory.CreateEnemy(randomSpawnNumber, spawnPosition);
				break;


			case (int)EnemyEnum.BlitzJok:
				_rangedCombatEnemyFactory.CreateEnemy(randomSpawnNumber, spawnPosition);
				break;
		}
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
		float randomDirection = Random.Range(0f, 1f);

		// Spawn phía trên camera
		if (randomDirection < 0.25f)
			positon = new Vector3(Random.Range(leftBound, rightBound), topBound + _enemySpawnOffset, 0f);

		// Spawn phía bên phải camera
		else if (randomDirection < 0.5f)
			positon = new Vector3(rightBound + _enemySpawnOffset, Random.Range(bottomBound, topBound), 0f);

		// Spawn phía dưới camera
		else if (randomDirection < 0.75f)
			positon = new Vector3(Random.Range(leftBound, rightBound), bottomBound - _enemySpawnOffset, 0f);

		// Spawn phía bên trái camera
		else
			positon = new Vector3(leftBound - _enemySpawnOffset, Random.Range(bottomBound, topBound), 0f);

		return positon;
	}
}