using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private List<MapBlock> _blocks;
    private GameObject[] _mapPrefabs;
    private GameObject _playerReference;

    private int _playerCurrentBlock;
    //Half the width and height of the map block
    private readonly float _blockHalfWidth = 60;
    private readonly float _blockHalfHeight = 60;


    public int PlayerCurrentBlock
    {
        get { return _playerCurrentBlock; }
        set
        {
            if (_playerCurrentBlock != value)
            {
                _playerCurrentBlock = value;
                UpdateMapGrid();
            }
        }
    }

    private void Awake()
    {
        _mapPrefabs = Resources.LoadAll<GameObject>("Prefabs/Map");
        _blocks = new List<MapBlock>();

        InitializeMap();
        PlayerCurrentBlock = _blocks[4].GridPosition;
        _playerReference = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    private void Update()
    {
        if (_playerReference == null)
        {
            _playerReference = GameObject.FindGameObjectWithTag("Player");
        }
        PlayerCurrentBlock = GetTheNearestBlockPosition();
    }

    #region Custom Methods

    /// <summary>
    /// Initialize map
    /// </summary>
    private void InitializeMap()
    {
        if (_mapPrefabs.Length > 0 && _blocks != null)
        {
            //Randomize initial map prefabs array
            System.Random randomizer = new System.Random();
            for (int i = 0; i < _mapPrefabs.Length - 1; ++i)
            {
                int randomizeArrayIndex = randomizer.Next(i, _mapPrefabs.Length);
                (_mapPrefabs[randomizeArrayIndex], _mapPrefabs[i]) = (_mapPrefabs[i], _mapPrefabs[randomizeArrayIndex]);
            }

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

    /// <summary>
    /// Place a map block with the given grid position
    /// </summary>
    /// <param name="block">Map block to place</param>
    /// <param name="gridPosition">Direction to place based on a 3x3 grid</param>
    /// <param name="steps">Offset by number of blocks</param>
    private void PlaceBlock(MapBlock block, int gridPosition, int steps = 0)
    {
        Vector3 normalized = GetNormalizedVector(gridPosition) * (steps + 1);

        Vector3 currentPosition = block.MapPrefab.transform.position;

        Vector3 newPosition = currentPosition + new Vector3(
            normalized.x * _blockHalfWidth,
            normalized.y * _blockHalfHeight,
            0
            );

        block.MapPrefab.transform.position = newPosition;
    }

    private int GetTheNearestBlockPosition()
    {
        float distance = Mathf.Infinity;
        int nearestBlockPosition = -1;

        for (int i = 0; i < 9; i++)
        {
            MapBlock tempBlock = _blocks[i];

            float tempDistance = Vector3.Distance(
                tempBlock.MapPrefab.transform.position,
                _playerReference.transform.position
                );

            if (tempDistance < distance)
            {
                distance = tempDistance;
                nearestBlockPosition = tempBlock.GridPosition;
            }
        }
        return nearestBlockPosition;
    }

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
    /// Update map based on player grid block position
    /// </summary>
    private void UpdateMapGrid()
    {
        switch (PlayerCurrentBlock)
        {
            /*
             * player move from 4 to 0
             * [0][1][2] move blocks [8][5][2] update grid numbering [0][1][2]
             * [3][4][5]             [7][0][1]                       [3][4][5]
             * [6][7][8]             [6][3][4]                       [6][7][8]
             */
            case 0:
                PlaceBlock(_blocks[2], 0, 0);
                PlaceBlock(_blocks[6], 0, 0);
                PlaceBlock(_blocks[5], 0, 1);
                PlaceBlock(_blocks[7], 0, 1);
                PlaceBlock(_blocks[8], 0, 2);

                _blocks[0].GridPosition = 4;
                _blocks[1].GridPosition = 5;
                _blocks[3].GridPosition = 7;
                _blocks[4].GridPosition = 8;
                _blocks[5].GridPosition = 1;
                _blocks[8].GridPosition = 0;
                _blocks[7].GridPosition = 3;
                break;

            /*
             * player move from 4 to 1
             * [0][1][2] move blocks [6][7][8] update grid numbering [0][1][2]
             * [3][4][5]             [0][1][2]                       [3][4][5]
             * [6][7][8]             [3][4][5]                       [6][7][8]
             */
            case 1:
                PlaceBlock(_blocks[6], 1, 2);
                PlaceBlock(_blocks[7], 1, 2);
                PlaceBlock(_blocks[8], 1, 2);

                _blocks[0].GridPosition = 3;
                _blocks[1].GridPosition = 4;
                _blocks[2].GridPosition = 5;
                _blocks[3].GridPosition = 6;
                _blocks[4].GridPosition = 7;
                _blocks[5].GridPosition = 8;
                _blocks[6].GridPosition = 0;
                _blocks[7].GridPosition = 1;
                _blocks[8].GridPosition = 2;
                break;

            /*
             * player move from 4 to 2
             * [0][1][2] move blocks [0][3][6] update grid numbering [0][1][2]
             * [3][4][5]             [1][2][7]                       [3][4][5]
             * [6][7][8]             [4][5][8]                       [6][7][8]
             */
            case 2:
                PlaceBlock(_blocks[0], 2, 0);
                PlaceBlock(_blocks[8], 2, 0);
                PlaceBlock(_blocks[3], 2, 1);
                PlaceBlock(_blocks[7], 2, 1);
                PlaceBlock(_blocks[6], 2, 2);

                _blocks[1].GridPosition = 3;
                _blocks[2].GridPosition = 4;
                _blocks[3].GridPosition = 1;
                _blocks[4].GridPosition = 6;
                _blocks[5].GridPosition = 7;
                _blocks[6].GridPosition = 2;
                _blocks[7].GridPosition = 5;
                break;

            /*
             * player move from 4 to 3
             * [0][1][2] move blocks [2][0][1] update grid numbering [0][1][2]
             * [3][4][5]             [5][3][4]                       [3][4][5]
             * [6][7][8]             [8][6][7]                       [6][7][8]
             */
            case 3:
                PlaceBlock(_blocks[2], 3, 2);
                PlaceBlock(_blocks[5], 3, 2);
                PlaceBlock(_blocks[8], 3, 2);

                _blocks[0].GridPosition = 1;
                _blocks[1].GridPosition = 2;
                _blocks[2].GridPosition = 0;
                _blocks[3].GridPosition = 4;
                _blocks[4].GridPosition = 5;
                _blocks[5].GridPosition = 3;
                _blocks[6].GridPosition = 7;
                _blocks[7].GridPosition = 8;
                _blocks[8].GridPosition = 6;
                break;

            /*
             * player move from 4 to 4, no need to update map
             */
            case 4:
                break;

            /*
             * player move from 4 to 5
             * [0][1][2] move blocks [1][2][0] update grid numbering [0][1][2]
             * [3][4][5]             [4][5][3]                       [3][4][5]
             * [6][7][8]             [7][8][6]                       [6][7][8]
             */
            case 5:
                PlaceBlock(_blocks[0], 5, 2);
                PlaceBlock(_blocks[3], 5, 2);
                PlaceBlock(_blocks[6], 5, 2);

                _blocks[0].GridPosition = 2;
                _blocks[1].GridPosition = 0;
                _blocks[2].GridPosition = 1;
                _blocks[3].GridPosition = 5;
                _blocks[4].GridPosition = 3;
                _blocks[5].GridPosition = 4;
                _blocks[6].GridPosition = 8;
                _blocks[7].GridPosition = 6;
                _blocks[8].GridPosition = 6;
                break;

            /*
             * player move from 4 to 6
             * [0][1][2] move blocks [0][3][4] update grid numbering [0][1][2]
             * [3][4][5]             [1][6][7]                       [3][4][5]
             * [6][7][8]             [2][5][8]                       [6][7][8]
             */
            case 6:
                PlaceBlock(_blocks[0], 6, 0);
                PlaceBlock(_blocks[1], 6, 1);
                PlaceBlock(_blocks[2], 6, 2);
                PlaceBlock(_blocks[5], 6, 1);
                PlaceBlock(_blocks[8], 6, 0);

                _blocks[0].GridPosition = 0;
                _blocks[1].GridPosition = 3;
                _blocks[2].GridPosition = 6;
                _blocks[3].GridPosition = 1;
                _blocks[4].GridPosition = 2;
                _blocks[5].GridPosition = 7;
                _blocks[6].GridPosition = 4;
                _blocks[7].GridPosition = 5;
                break;

            /*
             * player move from 4 to 7
             * [0][1][2] move blocks [3][4][5] update grid numbering [0][1][2]
             * [3][4][5]             [6][7][8]                       [3][4][5]
             * [6][7][8]             [0][1][2]                       [6][7][8]
             */
            case 7:
                PlaceBlock(_blocks[0], 7, 2);
                PlaceBlock(_blocks[1], 7, 2);
                PlaceBlock(_blocks[2], 7, 2);

                _blocks[0].GridPosition = 6;
                _blocks[1].GridPosition = 7;
                _blocks[2].GridPosition = 8;
                _blocks[3].GridPosition = 0;
                _blocks[4].GridPosition = 1;
                _blocks[5].GridPosition = 2;
                _blocks[6].GridPosition = 3;
                _blocks[7].GridPosition = 4;
                _blocks[8].GridPosition = 5;
                break;

            /*
             * player move from 4 to 8
             * [0][1][2] move blocks [4][5][2] update grid numbering [0][1][2]
             * [3][4][5]             [7][8][1]                       [3][4][5]
             * [6][7][8]             [6][3][0]                       [6][7][8]
             */
            case 8:
                PlaceBlock(_blocks[0], 8, 2);
                PlaceBlock(_blocks[1], 8, 1);
                PlaceBlock(_blocks[2], 8, 0);
                PlaceBlock(_blocks[3], 8, 1);
                PlaceBlock(_blocks[6], 8, 0);

                _blocks[0].GridPosition = 8;
                _blocks[1].GridPosition = 5;
                _blocks[3].GridPosition = 7;
                _blocks[4].GridPosition = 0;
                _blocks[5].GridPosition = 1;
                _blocks[7].GridPosition = 3;
                _blocks[8].GridPosition = 4;
                break;

            default:
                break;
        }

        //Sort the list by new grid position after update grid numbering
        _blocks.Sort((x, y) => x.GridPosition.CompareTo(y.GridPosition));
    }

    #endregion Custom Methods

    #region MapBlock object

    [Serializable]
    public class MapBlock
    {
        public GameObject MapPrefab;
        public int GridPosition;
    }

    #endregion MapBlock object
}