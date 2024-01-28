using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFollowCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position*Vector2.one;
    }
}
