using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 camLastPosition;
    [SerializeField]
    private float positionMultiply = 0.5f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        camLastPosition = cameraTransform.position;
    }
    private void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - camLastPosition;
        transform.position += deltaMovement * positionMultiply;
        camLastPosition = cameraTransform.position;
    }
}
