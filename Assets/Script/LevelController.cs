using Photon.Pun;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;
using ExitGames.Client.Photon;
using Photon.Realtime;


public enum TypeTerrain
{
    well,
    natural,
}
public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform mapPosition;
    [SerializeField] private int level;
    [SerializeField] private TypeTerrain terrain;
    [SerializeField] private int maxFloor = 5;

    [Space(2.0f)]
    [Header("TRANSFORM")]
    [SerializeField] private Transform player;
    
    [Space(2.0f)]
    [Header("PREFAB")]
    [SerializeField] private GameObject thornPref;

    [SerializeField] List<Tile> tilesetWells = new();
    [SerializeField] List<Tile> tilesetNatural = new();

    private List<Block> blocks;
    private float endPosY;
    private GameObject[] blocksBody;
    private GameObject[] blocksEnd;
    private GameObject[] blocksBegin;

    private bool _mapGenerated = false;

    private void Awake()
    {
        LoadDataBlocks();
        blocks = new();

    }

    private void Update()
    {
        if (!_mapGenerated)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("RaiseEvent");
                int mapGenerateCode = GenerateMapCode(maxFloor);
                Hashtable prop = new Hashtable() { { "mapGenerateCode", mapGenerateCode } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(prop);

                GenerateMap(mapGenerateCode);

                _mapGenerated = true;
            }

            else
            {
                int mapCode = 0;
                if (PhotonNetwork.MasterClient.CustomProperties.TryGetValue("mapGenerateCode", out object data))
                {
                    mapCode = (int)data;
                    GenerateMap(mapCode);
                    _mapGenerated = true;

                }
            }
        }
        
    }

    private void LoadDataBlocks()
    {
        blocksBody  = Resources.LoadAll<GameObject>("Block");
        blocksEnd   = Resources.LoadAll<GameObject>("BlockEnd");
        blocksBegin = Resources.LoadAll<GameObject>("BlockBegin");

    }     



    private void GenerateBeginBlock(int index)
    {
        endPosY = 0;
        //GameObject blockBegin = blocksBegin[Random.Range(0, blocksBegin.Length)];
        GameObject blockBegin = blocksBegin[index];
        Block begin = SimplePool.Spawn(blockBegin, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        begin.transform.SetParent(mapPosition);
        endPosY += begin.GetBlockMapSize().y / 2f;
        begin.IFloor = 0;
        this.blocks.Add(begin);
        ChangeTile(begin, begin.IFloor);
    }

    private void GenerateBodyBlock(int index, int floor)
    {
        Block blockTemp;
        //do
        //{
        //    int rd = Random.Range(0, blocksBody.Length);
        //    blockTemp = blocksBody[rd].GetComponent<Block>();
        //} while (blockTemp.DifficultyLevel != difficultyLevel);
        blockTemp = blocksBody[index].GetComponent<Block>();

        endPosY += blockTemp.GetBlockMapSize().y / 2f;
        Block block = SimplePool.Spawn(blockTemp.gameObject, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        block.transform.SetParent(mapPosition);
        block.IFloor = floor;
        endPosY += blockTemp.GetBlockMapSize().y / 2f;

        block.SpawnThorn(thornPref);

        this.blocks.Add(block);
        ChangeTile(block, block.IFloor);
    }


    private int GenerateBodyBlockID(DifficultyLevel difficultyLevel, int floor)
    {
        int res = -1;
        do
        {
            res = UnityEngine.Random.Range(0, blocksBody.Length);
        } while (blocksBody[res].GetComponent<Block>().DifficultyLevel != difficultyLevel);
        return res;
    }

    private void GenerateEndBlock(int index)
    {
        //GameObject blockEnd = blocksEnd[Random.Range(0, blocksEnd.Length)];
        GameObject blockEnd = blocksEnd[index];
        endPosY += blockEnd.GetComponent<Block>().GetBlockMapSize().y / 2f;
        Block finish = Instantiate(blockEnd, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        finish.transform.SetParent(mapPosition);
        finish.IFloor = maxFloor;
        finish.SpawnThorn(thornPref);
        ChangeTile(finish, maxFloor);
    }

    private void ChangeTile(Block block, int floor)
    {
        if(floor < 2)
        {
            for (int i = 0; i < tilesetNatural.Count; i++)
            {
                TileMapHelper.ChangeTile(block.Obstacle, tilesetNatural[i], tilesetWells[i]);
                TileMapHelper.ChangeTile(block.Wall, tilesetNatural[i], tilesetWells[i]);
            }
        }
    }


    public int GenerateMapCode(int size)
    {
        int res = 0;
        // begin block
        res += UnityEngine.Random.Range(0, blocksBegin.Length);
        res *= 10;

        for (int i = 1; i < maxFloor - 1; i++)
        {
            if (i < 2)
                res += GenerateBodyBlockID(DifficultyLevel.Easy, i);
            else if (i < maxFloor - 1)
                res += GenerateBodyBlockID(DifficultyLevel.Normal, i);
            else
                res += GenerateBodyBlockID(DifficultyLevel.Hard, i);
            res *= 10;

        }


        // end block
        res += UnityEngine.Random.Range(0, blocksEnd.Length);
        return res;
    }



    public void GenerateMap(int mapCode)
    {
        Debug.Log("GenerateMap");
        GenerateBeginBlock(mapCode % 10);
        mapCode /= 10;

        for(int i = 1; i < maxFloor; i++)
        {
            GenerateBodyBlock(mapCode % 10, i);
            mapCode /= 10;
        }

        GenerateEndBlock(mapCode);

    }
}
