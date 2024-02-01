using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeIntensity = 0.1f;

    public float shakeDuration = 0.5f;

    private void OnEnable()
    {
        this.Register(EventID.CameraShake, StartShake);
    }

    private void OnDisable()
    {
        this.Unregister(EventID.CameraShake, StartShake);
    }

    private IEnumerator Shake()
    {
        Vector3 originalPosition = transform.position;

        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;

            transform.position = originalPosition + shakeOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }

    public void StartShake(object data = null)
    {
        StartCoroutine(Shake());
    }
}