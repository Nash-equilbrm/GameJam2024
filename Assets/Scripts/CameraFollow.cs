using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float speed;
    [SerializeField]
    private bool started;


    private void Start()
    {
        this.Register(EventID.BackToMenu, ResetCamera);
    }

    private void OnDestroy()
    {
        this.Unregister(EventID.BackToMenu, ResetCamera);
    }

    public void SetupCamera(Transform player)
    {
        playerTransform = player;
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) { return; }
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, playerTransform.position) < 0.01f)
        {
            transform.position = playerTransform.position + offset;
        }
    }

    private void ResetCamera(object obj)
    {
        playerTransform = null;
        started = false ;
    }
}
