using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomSpawnEnermy : MonoBehaviour
{
    [SerializeField] Sprite[] enermy;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    GameObject _newEnermy;

    float _timer;
    // Start is called before the first frame update
    void Start()
    {
        
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
        
        Vector3 positon = new Vector3();
        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if(UnityEngine.Random.value > 0.5f)
        {
            positon.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.y);
            positon.y = spawnArea.y * f;
        }
        else
        {
            positon.y = UnityEngine.Random.Range(-spawnArea.y,spawnArea.x);
            positon.x = spawnArea.x * f;
        }
        positon.z = 0;
        return positon;
    }
}
