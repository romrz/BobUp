using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    public float shake_decay = 0.002f;
    public float shake_intensity = 0.3f;

    private float _shake_intensity = 0f;
    private float _shake_decay = 0f;


    private Vector3 originPosition;
    private Quaternion originRotation;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
        }
    }

    public void Shake()
    {
        Shake(shake_intensity, shake_decay);
    }
    public void Shake(float intensity, float decay)
    {
        _shake_intensity = intensity;
        originPosition = transform.position;
        originRotation = transform.rotation;
        _shake_decay = decay;

        InvokeRepeating("ShakeCamera", 0, .01f);
        Invoke("StopShaking", 1f);       
    }

    void ShakeCamera()
    {
        if (_shake_intensity > 0)
        {
            transform.position = originPosition + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0) * _shake_intensity;
            _shake_intensity -= _shake_decay;
        }
    }

    void StopShaking()
    {
        CancelInvoke("ShakeCamera");
        transform.position = originPosition;
    }
}
