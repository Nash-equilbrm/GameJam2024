using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBG : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 camLastPosition;
    [SerializeField]
    private float positionMultiply = 0.2f;

    private float height;
    private float startPos;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        camLastPosition = cameraTransform.position;
        startPos = transform.position.y;
        height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
    }
    private void Update()
    {
        //Parallax 
        
        Vector3 deltaMovement = cameraTransform.position - camLastPosition;
        transform.position += deltaMovement * positionMultiply;
        camLastPosition = cameraTransform.position;


        //Infinite
        float tempPos = cameraTransform.position.y * (1 - positionMultiply);

        if (tempPos > startPos + height)
        {
            startPos += height;
            transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        }
        else if (tempPos < startPos - height)
        {
            startPos -= height;
            transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        }
    }

}
