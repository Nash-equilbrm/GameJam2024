using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteObject : MonoBehaviour
{
    [SerializeField]
    private Transform center;
    public PlayerController playerController;
    public float radius = 5f;        // Bán kính của quỹ đạo hình tròn
    public float speed = 2f;         // Tốc độ di chuyển

    [SerializeField, Range(0f, 3f)]
    private float startAngle;
    private float curentAngle = 0f;        // Góc hiện tại

    private void OnEnable()
    {
        float f = (float)Math.PI / 2f;
        curentAngle = startAngle * f;
    }
    void Update()
    {
        if (!playerController.photonView.IsMine) { return; }
        if (playerController.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
        curentAngle += speed * Time.deltaTime;

        float x = center.position.x + Mathf.Cos(curentAngle) * radius;
        float y = center.position.y + Mathf.Sin(curentAngle) * radius;

        transform.position = new Vector3(x, y, 0);
    }
}
