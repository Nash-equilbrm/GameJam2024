using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePatrol : MonoBehaviour
{
    private Vector3 center;

    [SerializeField]
    private bool clockWise;
    public float radius = 5f;
    public float speed = 2f;

    [SerializeField, Range(0f, 3f)]
    private float startAngle;
    private float curentAngle = 0f;

    private float originalScaleX;
    private void Start()
    {
        float f = (float)Math.PI / 2f;
        curentAngle = startAngle * f;
        center = transform.position;
        originalScaleX = transform.localScale.x;
    }
    void Update()
    {
        float x = 0;
        float y = 0;
        curentAngle += speed * Time.deltaTime;

        if (!clockWise)
        {
            x = center.x + Mathf.Cos(curentAngle) * radius;
        }
        else
        {
            x = center.x - Mathf.Cos(curentAngle) * radius;
        }

        y = center.y + Mathf.Sin(curentAngle) * radius;
        transform.position = new Vector3(x, y, 0);

        if (transform.position.y >= center.y)
        {
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, transform.localScale.z);
        }

    }
}
