using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField]
    float duration = 1f;

    [SerializeField]
    AnimationCurve curve;

    public void TriggerShake() => StartCoroutine(Shaking());

    IEnumerator Shaking()
    {
        var rotationZ = transform.rotation.z;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Debug.Log("Shaking");
            elapsedTime += Time.deltaTime;
            // var strength = curve.Evaluate(elapsedTime / duration);
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + Random.Range(-1f, 1f));
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
