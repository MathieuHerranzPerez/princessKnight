using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    // ---- INTERN ----
    private Vector3 initialPos;
    private Quaternion initialRot;

    void Start()
    {
        Instance = this;
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

    public void Shake(float duration, float translationMagnitude, float rotationMagnitude)
    {
        StartCoroutine(ShakeCam(duration, translationMagnitude, rotationMagnitude));
    }

    private IEnumerator ShakeCam(float duration, float translationMagnitude, float rotationMagnitude)
    {
        float time = 0f;
        while (time < duration)
        {
            float x = Random.Range(-1f, 1f) * translationMagnitude;
            float y = Random.Range(-1f, 1f) * translationMagnitude;

            float rotY = Random.Range(-1f, 1f) * rotationMagnitude;
            float rotZ = Random.Range(-1f, 1f) * rotationMagnitude;

            transform.localPosition = new Vector3(x, y, initialPos.z);
            transform.Rotate(new Vector3(0f, rotY, rotZ));

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPos;
        transform.localRotation = initialRot;
    }
}
