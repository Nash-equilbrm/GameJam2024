using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

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


    private void Awake()
    {
        LoadDataBlocks();
        blocks = new();
        GenerateMap();
    }

    private void LoadDataBlocks()
    {
        blocksBody  = Resources.LoadAll<GameObject>("Block");
        blocksEnd   = Resources.LoadAll<GameObject>("BlockEnd");
        blocksBegin = Resources.LoadAll<GameObject>("BlockBegin");

    }     

    private void GenerateBeginBlock()
    {
        endPosY = 0;
        GameObject blockBegin = blocksBegin[Random.Range(0, blocksBegin.Length)];
        Block begin = SimplePool.Spawn(blockBegin, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        begin.transform.SetParent(mapPosition);
        endPosY += begin.GetBlockMapSize().y / 2f;
        begin.IFloor = 0;
        this.blocks.Add(begin);
        ChangeTile(begin, begin.IFloor);
    }

    private void GenerateBodyBlock(DifficultyLevel difficultyLevel, int floor)
    {
        Block blockTemp;
        do
        {
            int rd = Random.Range(0, blocksBody.Length);
            blockTemp = blocksBody[rd].GetComponent<Block>();
        } while (blockTemp.DifficultyLevel != difficultyLevel);
        
        endPosY += blockTemp.GetBlockMapSize().y / 2f;
        Block block = SimplePool.Spawn(blockTemp.gameObject, new Vector2(0, endPosY), Quaternion.identity).GetComponent<Block>();
        block.transform.SetParent(mapPosition);
        block.IFloor = floor;
        endPosY += blockTemp.GetBlockMapSize().y / 2f;

        block.SpawnThorn(thornPref);

        this.blocks.Add(block);
        ChangeTile(block, block.IFloor);
    }

    private void GenerateEndBlock()
    {
        GameObject blockEnd = blocksEnd[Random.Range(0, blocksEnd.Length)];
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

    public void GenerateMap()
    {
        GenerateBeginBlock();

        for(int i = 1; i < maxFloor; i++)
        {
            if(i < 2)
                GenerateBodyBlock(DifficultyLevel.Easy, i);
            else if (i < maxFloor - 1)
                GenerateBodyBlock(DifficultyLevel.Normal, i);
            else
                GenerateBodyBlock(DifficultyLevel.Hard, i);
        }

        GenerateEndBlock();
    }
}
