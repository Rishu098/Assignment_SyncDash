using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Duration of the shake effect
    public float shakeDuration = 0.5f;

    // Magnitude of the shake
    public float shakeMagnitude = 0.1f;

    // How quickly the shaking settles
    public float dampingSpeed = 1.0f;

    // The initial position of the camera
    private Vector3 initialPosition;

    void OnEnable()
    {
        // Store the initial position of the camera
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if(GameManager.Instance.isPlaying)
            transform.Translate(Vector3.forward * (GameManager.Instance.GameSpeed)* Time.deltaTime);
        //if (shakeDuration > 0)
        //{
        //    // Randomize the camera's position within a sphere
        //    transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

        //    // Reduce the shake duration over time
        //    shakeDuration -= Time.deltaTime * dampingSpeed;
        //}
        //else
        //{
        //    // Reset the camera position when the shake is over
        //    shakeDuration = 0f;
        //    transform.localPosition = initialPosition;
        //}
    }

    // Call this method to trigger the camera shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}