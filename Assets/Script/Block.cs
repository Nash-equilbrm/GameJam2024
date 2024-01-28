using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum DifficultyLevel
{
    Easy,
    Normal,
    Hard,
}
public class Block : MonoBehaviour
{
    [SerializeField] private int iFloor;
    [SerializeField] private DifficultyLevel difficultyLevel;
    [SerializeField] private Tilemap wall;
    [SerializeField] private Tilemap thorn;
    [SerializeField] private Tilemap obstacle;
    [SerializeField] private Tilemap finish;
    [SerializeField] Tile tile;

    public Tilemap Wall { get => wall; set => wall = value; }
    public Tilemap Thorn { get => thorn; set => thorn = value; }
    public Tilemap Obstacle { get => obstacle; set => obstacle = value; }
    public Tilemap Finish { get => finish; set => finish = value; }
    public DifficultyLevel DifficultyLevel { get => difficultyLevel; set => difficultyLevel = value; }
    public int IFloor { get => iFloor; set => iFloor = value; }

    public Vector3 GetBlockMapSize()
    {
        Vector3Int size = wall.size;
        Vector3 cellSize = Vector3.one * tile.sprite.bounds.size.x; //1.28f
        Vector3 worldSize = new Vector3(size.x * cellSize.x, (size.y * cellSize.y) - -(tile.sprite.bounds.size.y * 4), size.z * cellSize.z);
        return worldSize;
    }

    public void SpawnThorn(GameObject prefab)
    {
        if (thorn == null) return;
        BoundsInt bounds = thorn.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tb = thorn.GetTile(cellPosition);
                if (tb != null)
                {
                    Vector3 tileWorldPos = this.thorn.CellToWorld(cellPosition);
                    Thorn thorn = PhotonNetwork.Instantiate(prefab.name, tileWorldPos, Quaternion.identity).GetComponent<Thorn>();
                }
            }
        }
    }

    //public void SpawnThorn(GameObject prefab, int numberSpawm = 0)
    //{
    //    List<Vector3> tileWorldPos = new();
    //    int count = 0;
    //    BoundsInt bounds = wall.cellBounds;
    //    for (int x = bounds.xMin; x < bounds.xMax; x++)
    //    {
    //        for (int y = bounds.yMin; y < bounds.yMax; y++)
    //        {
    //            Vector3Int cellPosition = new Vector3Int(x, y, 0);
    //            //TileBase tb = wall.GetTile(cellPosition);
    //            bool isHasTileFinish = false;
    //            if (finish != null)
    //                isHasTileFinish = finish.HasTile(cellPosition);
    //            if (!wall.HasTile(cellPosition) && !obstacle.HasTile(cellPosition) && !isHasTileFinish)
    //            {
    //                tileWorldPos.Add(wall.CellToWorld(cellPosition));
    //            }
    //        }
    //    }

    //    while (count++ < numberSpawm)
    //    {
    //        Thorn thorn = SimplePool.Spawn(prefab, tileWorldPos[UnityEngine.Random.Range(0, tileWorldPos.Count)], Quaternion.identity).GetComponent<Thorn>();
    //    }
    //}
}
