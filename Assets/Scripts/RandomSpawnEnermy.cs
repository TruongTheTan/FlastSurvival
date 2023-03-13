using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomSpawnEnermy : MonoBehaviour
{
    [SerializeField] Sprite[] enermy;
    [SerializeField] float spawnTimer;
    float _cameraHeight;
    float _cameraWidth;
    float _enermySpawnOffset = 1f;
    Camera mainCamera;
    GameObject _newEnermy;

    float _timer;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        _cameraHeight = 2f * mainCamera.orthographicSize;
        _cameraWidth = _cameraHeight * mainCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer < 0f)
        {
            SpawnEnemy();
            _timer = spawnTimer;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 postion = GenerateRandomPosition();
        postion += GameObject.FindGameObjectWithTag("Player").transform.position;
        int randomSprite = UnityEngine.Random.Range(0, enermy.Length);

        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/Enemy");
        SpriteRenderer playerSpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.sprite = enermy[randomSprite];

        _newEnermy = Instantiate(enemyPrefab, postion, Quaternion.identity) as GameObject;
        _newEnermy.transform.parent = transform;

    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        float leftBound = cameraPosition.x - _cameraWidth / 2f;
        float rightBound = cameraPosition.x + _cameraWidth / 2f;
        float bottomBound = cameraPosition.y - mainCamera.orthographicSize;
        float topBound = cameraPosition.y + mainCamera.orthographicSize;

        Vector3 positon = new Vector3();
        float randomDirection = UnityEngine.Random.Range(0f, 1f);
        if (randomDirection < 0.25f) // Spawn phía trên camera
        {
            positon = new Vector3(UnityEngine.Random.Range(leftBound, rightBound), topBound + _enermySpawnOffset, 0);
        }
        else if (randomDirection < 0.5f) // Spawn phía bên phải camera
        {
            positon = new Vector3(rightBound + _enermySpawnOffset, UnityEngine.Random.Range(bottomBound, topBound), 0);
        }
        else if (randomDirection < 0.75f) // Spawn phía dưới camera
        {
            positon = new Vector3(UnityEngine.Random.Range(leftBound, rightBound), bottomBound - _enermySpawnOffset, 0);
        }
        else // Spawn phía bên trái camera
        {
            positon = new Vector3(leftBound - _enermySpawnOffset, UnityEngine.Random.Range(bottomBound, topBound), 0);
        }
        return positon;
    }
}
