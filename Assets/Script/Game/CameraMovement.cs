using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPlaying)
            transform.Translate(Vector3.forward * (GameManager.Instance.GameSpeed) * Time.deltaTime);
    }
}
