//using NaughtyAttributes;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class MapSettings : MonoBehaviour
//{
//    [Header("DO NOT TOUCH")]
//    [SerializeField] private Transform map;
//    [Header("MAP INFOMATION")]
//    [SerializeField] private GameObject mapBegin;
//    [SerializeField] private GameObject mapFinish;
//    [Space(2.0f)]
//    [SerializeField] private GameObject[] mapBegins;
//    [SerializeField] private GameObject[] mapFinishs;
//    [SerializeField] private List<GameObject> bodyMap;
//    [Header("EDIT MAP")]
//    [SerializeField] private Map mapEdit;




//    public void SaveMap(string name)
//    {
//        if (name == string.Empty) name = "Map" + Random.Range(0, 1000);
//        Map map = ScriptableObject.CreateInstance<Map>();
//        map.BlockMaps = new();
//        map.BeginBlock = mapBegin;
//        map.FinishBlock = mapFinish;
//        map.BlockMaps.AddRange(bodyMap);
//#if UNITY_EDITOR
//        UnityEditor.AssetDatabase.CreateAsset(map, "Assets/Resources/Map/" + name + ".asset");
//#endif
//    }
//    [Button("Load Map")]
//    public void LoadMap()
//    {
//        ResetMap();
//        ClearPeference();
//        mapBegin = mapEdit.BeginBlock;
//        mapFinish = mapEdit.FinishBlock;
//        bodyMap = mapEdit.BlockMaps;
//        InitBlock(mapEdit.BeginBlock, mapEdit.FinishBlock, mapEdit.BlockMaps);
//    }
//    [Button("Reset Map")]
//    public void ResetMap()
//    {
//        for (int i = map.childCount - 1; i >= 0; i--)
//        {
//            DestroyImmediate(map.GetChild(i).gameObject);
//        }
//    }
//    [Button("Clear Map")]
//    public void ClearPeference()
//    {
//        bodyMap.Clear();
//    }
//    [Button("Generate Map")]
//    public void GenerateMap()
//    {
//        ResetMap();
//        mapEdit.BeginBlock = mapBegin;
//        mapEdit.FinishBlock = mapFinish;
//        InitBlock(mapBegin, mapFinish, bodyMap);
//        EditorUtility.SetDirty(mapEdit);
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }

//    private void InitBlock(GameObject beginBlock, GameObject finishBlock, List<GameObject> blocks)
//    {
//        Vector3 pos = Vector3.zero;
//        //begin
//        GameObject begin = Instantiate(beginBlock, pos, Quaternion.identity);
//        begin.transform.SetParent(map);

//        //body
//        pos.y += begin.GetComponent<Block>().GetBlockMapSize().y / 2f;
//        for (int i = 0; i < blocks.Count; i++)
//        {
//            pos.y += blocks[i].GetComponent<Block>().GetBlockMapSize().y / 2f;
//            GameObject block = Instantiate(blocks[i], pos, Quaternion.identity);
//            block.transform.SetParent(map);
//            pos.y += blocks[i].GetComponent<Block>().GetBlockMapSize().y / 2f;
//        }

//        //end
//        pos.y += finishBlock.GetComponent<Block>().GetBlockMapSize().y / 2f;
//        GameObject finish = Instantiate(finishBlock, pos, Quaternion.identity);
//        finish.transform.SetParent(map);
//    }
//}
