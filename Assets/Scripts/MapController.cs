using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private List<MapBlock> _blocks;
    private GameObject[] _mapPrefabs;
    private GameObject _playerReference;

    //Half the width and height of the map block
    private float _blockHalfWidth = 60;

    private float _blockHalfHeight = 60;

    private int _playerCurrentBlock;

    public int PlayerCurrentBlock
    {
        get { return _playerCurrentBlock; }
        set
        {
            if (_playerCurrentBlock != value)
            {
                _playerCurrentBlock = value;
            }
        }
    }

    private void Awake()
    {
        _mapPrefabs = Resources.LoadAll<GameObject>("Prefabs/Map");
        _blocks = new List<MapBlock>();

        InitializeMap();

        _playerReference = GameObject.FindGameObjectWithTag("Player");

        PlayerCurrentBlock = _blocks[4].GridPosition;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #region MapBlock object

    [Serializable]
    public class MapBlock
    {
        public GameObject MapPrefab;
        public int GridPosition;
    }

    #endregion MapBlock object

    #region Methods

    /// <summary>
    /// Return a normalized vector based on given grid position
    /// </summary>
    /// <param name="gridPosition">Position on a 3x3 grid</param>
    /// <returns></returns>
    private Vector3 GetNormalizedVector(int gridPosition)
    {
        Vector3 normalized = new Vector3(0, 0, 0);
        switch (gridPosition)
        {
            case 0:
                normalized = new Vector3(-1, 1, 0);
                break;

            case 1:
                normalized = new Vector3(0, 1, 0);
                break;

            case 2:
                normalized = new Vector3(1, 1, 0);
                break;

            case 3:
                normalized = new Vector3(-1, 0, 0);
                break;

            case 4:
                normalized = new Vector3(0, 0, 0);
                break;

            case 5:
                normalized = new Vector3(1, 0, 0);
                break;

            case 6:
                normalized = new Vector3(-1, -1, 0);
                break;

            case 7:
                normalized = new Vector3(0, -1, 0);
                break;

            case 8:
                normalized = new Vector3(1, -1, 0);
                break;
        }
        return normalized;
    }

    /// <summary>
    /// Place a map block with the given grid position
    /// </summary>
    /// <param name="block">Map block to place</param>
    /// <param name="gridPosition">Position to place based on a 3x3 grid</param>
    private void PlaceBlock(MapBlock block, int gridPosition, int steps = 1)
    {
        Vector3 normalized = GetNormalizedVector(gridPosition) * steps;

        Vector3 currentPosition = block.MapPrefab.transform.position;

        Vector3 newPosition = currentPosition + new Vector3(
            normalized.x * _blockHalfWidth,
            normalized.y * _blockHalfHeight,
            0
            );

        block.MapPrefab.transform.position = newPosition;
    }

    private void InitializeMap()
    {
        if (_mapPrefabs.Length > 0 && _blocks != null)
        {
            for (int i = 0; i < _mapPrefabs.Length; i++)
            {
                GameObject mapPrefab = Instantiate(_mapPrefabs[i]);
                MapBlock block = new MapBlock()
                {
                    MapPrefab = mapPrefab,
                    GridPosition = i
                };
                _blocks.Add(block);
                PlaceBlock(block, i);
            }
        }
    }

    #endregion Methods
}