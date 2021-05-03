using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camShake : MonoBehaviour
{
    public Transform camTransform;
    // How long the object should shake for.
    public float shakeDuration = 0f;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    Vector3 originalPos;
    public bool boss;
    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();
        if(boss)
            originalPos = camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boss)
            originalPos = camTransform.localPosition;
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}
