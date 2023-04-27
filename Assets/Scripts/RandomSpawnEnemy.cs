using System.Collections;
using Assets.Scripts.DesignPatterns.StrategyPattern;
using Assets.Scripts.DesignPatterns.StrategyPattern.Concreates;
using Assets.Scripts.PrefabControllers.EnemyControllers;
using UnityEngine;

public class RandomSpawnEnemy : MonoBehaviour
{

	#region Enemies prefab
	[SerializeField]
	private GameObject _fodderJoePrefab;

	[SerializeField]
	private GameObject _bigDaddyPrefab;

	[SerializeField]
	private GameObject _blitzJokPrefab;

	[SerializeField]
	private GameObject _explosiveDavePrefab;

	[SerializeField]
	private GameObject _enemyBulletPrefab;
	#endregion


	#region Properties

	private float _cameraHeight;
	private float _cameraWidth;
	private float _enermySpawnOffset = 1f;
	private int _spawnLimit;
	private int _gameRoundSeconds;
	private int _gameRound;

	#endregion

	private Camera _mainCamera;
	public int SpawnLimit { get => _spawnLimit; set => _spawnLimit = value; }




	private void Awake()
	{
		_gameRound = 0;
		_gameRoundSeconds = 60;
		_spawnLimit = 60;

		_mainCamera = Camera.main;
		_cameraHeight = 2f * _mainCamera.orthographicSize;
		_cameraWidth = _cameraHeight * _mainCamera.aspect;

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
		Vector3 spawnPostion = GenerateRandomPosition();
		spawnPostion += DataPreserve.player.transform.position;

		//Random.Range(1, 4)
		GameObject enemySpawned;



		switch (Random.Range(1, 4))
		{
			case (int)EnemyEnum.FodderJoe:
				enemySpawned = Instantiate(_fodderJoePrefab, spawnPostion, Quaternion.identity);

				FodderJoeController fodderJoeController = enemySpawned.GetComponent<FodderJoeController>();
				fodderJoeController.CloseCombatBehavior = new MeleeCombat(fodderJoeController.EnemyDamage);
				break;



			case (int)EnemyEnum.BigDaddy:
				enemySpawned = Instantiate(_bigDaddyPrefab, spawnPostion, Quaternion.identity);

				BigDaddyController bigDaddyJoeController = enemySpawned.GetComponent<BigDaddyController>();
				bigDaddyJoeController.CloseCombatBehavior = new MeleeCombat(bigDaddyJoeController.EnemyDamage);
				break;



			case (int)EnemyEnum.BlitzJok:
				enemySpawned = Instantiate(_blitzJokPrefab, spawnPostion, Quaternion.identity);

				BlitzJokController blitzJokController = enemySpawned.GetComponent<BlitzJokController>();
				blitzJokController.RangedCombatBehavior = new ShootBulletCombat(_enemyBulletPrefab, enemySpawned);
				break;



			case (int)EnemyEnum.ExplosiveDave:
				enemySpawned = Instantiate(_explosiveDavePrefab, spawnPostion, Quaternion.identity);

				ExplosiveDaveController explosiveDaveController = enemySpawned.GetComponent<ExplosiveDaveController>();
				explosiveDaveController.CloseCombatBehavior = new SuicideCombat(explosiveDaveController.EnemyDamage, enemySpawned, explosiveDaveController.EnemyHealthBar);
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