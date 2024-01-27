//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AutoCreateMap : MonoBehaviour
//{
//    [Header("MAP INFOMATION")]
//    [SerializeField] private GameObject[] mapBegin;
//    [SerializeField] private GameObject[] mapFinish;
//    private int perE = 60;
//    private int perN = 30;

//    private void Awake()
//    {
//        GenerateMapAuto();
//    }

//    private GameObject RandomMapBegin()
//    {
//        return mapBegin[Random.Range(0, mapBegin.Length)];
//    }

//    private GameObject RandomMapEnd()
//    {
//        return mapFinish[Random.Range(0, mapBegin.Length)];
//    }

//    private void GenerateMapAuto()
//    {
//        int index = 0;
//        int limitLevel = 20;
//        int nextLimit = 5;
//        int numberBlock = 5;
//        Block[] objects = Resources.LoadAll<Block>("Block");
//        //TypeLevel(objects);

//        while (index < limitLevel)
//        {
//            while (index++ < nextLimit)
//            {
//                List<GameObject> list = NumberBlocks(objects, numberBlock, index, nextLimit);
//                Map map = ScriptableObject.CreateInstance<Map>();
//                map.ID = index;
//                map.BlockMaps = new();
//                map.BeginBlock = RandomMapBegin();
//                map.FinishBlock = RandomMapEnd();
//                map.BlockMaps.AddRange(list);
//                UnityEditor.AssetDatabase.CreateAsset(map, "Assets/Resources/Map/Level_" + index + ".asset");
//            }
//            nextLimit += 5;
//        }
//    }

//    private List<GameObject> NumberBlocks(Block[] objects, int numberBlock, int level, int nextLimit)
//    {
//        List<GameObject> list = new();
//        int[] blocks = new int[3];
//        if (level < nextLimit)
//        {
//            blocks[0] = numberBlock * perE / 100;
//            blocks[1] = numberBlock * perN / 100;
//            blocks[2] = numberBlock - (blocks[0] + blocks[1]);
//            perE -= 15;
//            perE += 10;
//            perE += 5;
//        }


//        for (int i = 0; i < blocks[0]; i++)
//        {
//            list.Add(RandomBlock(objects, DifficultyLevel.Easy));
//        }
//        for (int i = 0; i < blocks[1]; i++)
//        {
//            list.Add(RandomBlock(objects, DifficultyLevel.Normal));
//        }
//        for (int i = 0; i < blocks[2]; i++)
//        {
//            list.Add(RandomBlock(objects, DifficultyLevel.Hard));
//        }
//        return list;
//    }

//    private GameObject RandomBlock(Block[] objects, DifficultyLevel difficulty)
//    {
//        Block block;
//        do
//        {
//            block = objects[Random.Range(0, objects.Length - 1)];
//        } while (block.DifficultyLevel != difficulty);

//        return block.gameObject;
//    }

//    private void TypeLevel(Block[] objects)
//    {
//        for (int i = 0; i < objects.Length; i++)
//        {
//            if (i < 5)
//                objects[i].DifficultyLevel = DifficultyLevel.Easy;
//            else if (i >= 5 && i < 10)
//                objects[i].DifficultyLevel = DifficultyLevel.Normal;
//            else
//                objects[i].DifficultyLevel = DifficultyLevel.Hard;
//        }
//    }
//}
