using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Chỉnh tag của object trong Unity
        //Darts là tag của skill 
        //Phi tiêu trong map chưa có tag -> Tạo tag mới hoặc add tag darts vào prefabs Thorn
        if (collision.CompareTag("Darts"))
        {

        }
    }
}
