using Photon.Pun;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;


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
    public  GameObject[] blocksBody;
    public GameObject[] blocksEnd;
    public GameObject[] blocksBegin;


    private bool _mapGenerated = false;

    private void Awake()
    {
        blocks = new();

    }

    private void Update()
    {
        //Debug.Log("Update: _mapGenerated = " + _mapGenerated);
        if (!_mapGenerated)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                string mapGenerateCode = GenerateMapCode(maxFloor);
                GenerateMap(mapGenerateCode);
                //Hashtable prop = new Hashtable() { { "mapGenerateCode", mapGenerateCode } };
                //PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
                //Debug.Log("mapGenerateCode: " + mapGenerateCode);
                _mapGenerated = true;
                //Debug.Log("Update: _mapGenerated = " + _mapGenerated);
            }

            //else
            //{
            //    string mapCode = "";
            //    if (PhotonNetwork.MasterClient.CustomProperties.TryGetValue("mapGenerateCode", out object data))
            //    {
            //        mapCode = (string)data;
            //        GenerateMap(mapCode);

            //        _mapGenerated = true;
            //        Debug.Log("Update: _mapGenerated = " + _mapGenerated);

            //    }
            //}
        }

    }




    private void GenerateBeginBlock(int index)
    {
        endPosY = 0;
        //GameObject blockBegin = blocksBegin[Random.Range(0, blocksBegin.Length)];
        GameObject blockBegin = blocksBegin[index];
        //Block begin = SimplePool.Spawn(blockBegin, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        Block begin = PhotonNetwork.Instantiate(blockBegin.name, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
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
        //Block block = SimplePool.Spawn(blockTemp.gameObject, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        Block block = PhotonNetwork.Instantiate(blockTemp.gameObject.name, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
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
        Block finish = PhotonNetwork.Instantiate(blockEnd.name, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        finish.transform.SetParent(mapPosition);
        finish.IFloor = maxFloor;
        finish.SpawnThorn(thornPref);
        ChangeTile(finish, maxFloor);
    }

    private void ChangeTile(Block block, int floor)
    {
        if (floor < 2)
        {
            for (int i = 0; i < tilesetNatural.Count; i++)
            {
                TileMapHelper.ChangeTile(block.Obstacle, tilesetNatural[i], tilesetWells[i]);
                TileMapHelper.ChangeTile(block.Wall, tilesetNatural[i], tilesetWells[i]);
            }
        }
    }


    public string GenerateMapCode(int size)
    {
        string res = "";
        // begin block
        res += (UnityEngine.Random.Range(0, blocksBegin.Length)).ToString() + "-";

        for (int i = 1; i < maxFloor - 1; i++)
        {
            if (i < 2)
            {
                res += GenerateBodyBlockID(DifficultyLevel.Easy, i).ToString() + "-";

            }

            else if (i < maxFloor - 1)
            {
                res += GenerateBodyBlockID(DifficultyLevel.Normal, i).ToString() + "-";

            }
            else
            {
                res += GenerateBodyBlockID(DifficultyLevel.Hard, i).ToString() + "-";

            }
        }


        // end block
        res += UnityEngine.Random.Range(0, blocksEnd.Length).ToString();
        return res;
    }



    public void GenerateMap(string mapCode)
    {
        List<string> blockIdStr = mapCode.Split('-').ToList<string>();

        List<int> blockId = blockIdStr.Select(x => Int32.Parse(x)).ToList();


        GenerateBeginBlock(blockId[0]);

        for (int i = 1; i < maxFloor; i++)
        {
            GenerateBodyBlock(blockId[i], i);
        }

        GenerateEndBlock(blockId[blockId.Count - 1]);

    }
}
