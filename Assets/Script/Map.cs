using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Map/New Map")]

public class Map : ScriptableObject
{
    public int ID;
    public GameObject BeginBlock;
    public GameObject FinishBlock;
    public List<GameObject> BlockMaps;
}
