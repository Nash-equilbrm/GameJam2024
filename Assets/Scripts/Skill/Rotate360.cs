using UnityEngine;

public class Rotate360 : MonoBehaviour
{
    public float rotationSpeed = 5f;

    void Update()
    {
        RotateAroundZ();
    }

    void RotateAroundZ()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
