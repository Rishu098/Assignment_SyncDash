using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [SerializeField]
    private CameraShake CameraShake;

    private Rigidbody characterController;

    // Swipe detection variables
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float minSwipeDistance = 50f; // Minimum distance for a swipe to be detected

    // Event for player actions (e.g., jump, position update)
    public event Action<Vector3> OnPlayerPositionUpdate;
    public event Action OnPlayerJump;
    public event Action<int> PlayerHorizontalMovement; // -1 for Left and 1 for right

    private float initialDistance = 0;
    private float distanceRemainigToIncreaseSpeed = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (initialDistance < GameManager.Instance.TotalDistanceToCover)
        {
            GameManager.Instance.isPlaying = true;
            // Automatically move forward
            characterController.transform.Translate(Vector3.forward * GameManager.Instance.GameSpeed * Time.deltaTime);
            initialDistance += .1f;
            distanceRemainigToIncreaseSpeed += 0.1f;
            // Handle swipe input for mobile
            HandleSwipeInput();

            // Trigger position update event
            Invoke("SendPlayerPosition", 0);

            // Jump on tap
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }

            if (distanceRemainigToIncreaseSpeed >= GameManager.Instance.IncreaseSpeedDistance)
            {
                GameManager.Instance.GameSpeed += 2f;
                distanceRemainigToIncreaseSpeed = 0;
            }
        }
        else
        {
            if (GameManager.Instance.isPlaying)
            {
                GameManager.Instance.isPlaying = false;
                StartCoroutine(Delay());
            }

        }
    }

    void HandleSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    touchEndPos = touch.position;
                    DetectSwipe();
                    break;
            }
        }
    }

    void DetectSwipe()
    {
        float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);

        if (swipeDistance > minSwipeDistance)
        {
            Vector2 swipeDirection = touchEndPos - touchStartPos;

            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                // Horizontal swipe
                if (swipeDirection.x > 0)
                {
                    // Swipe right
                    MoveRight();
                }
                else
                {
                    // Swipe left
                    MoveLeft();
                }
            }
        }
    }

    void MoveRight()
    {
        if (transform.position.x + 1.32f < 4)
        {
            characterController.transform.position = new Vector3(transform.position.x + 1.32f, transform.position.y, transform.position.z);
            Invoke("TriggerRightMove", GameManager.Instance.Delay); // Trigger Right event
        }
    }

    void MoveLeft()
    {
        if (transform.position.x - 1.32f > 1.1)
        {
            characterController.transform.position = new Vector3(transform.position.x - 1.32f, transform.position.y, transform.position.z);
            Invoke("TriggerLeftMove", GameManager.Instance.Delay); // Trigger Left event
        }
    }

    void Jump()
    {
        characterController.AddForce(new Vector3(0, jumpForce), ForceMode.Impulse);
        Invoke("TriggerJumpEvent", GameManager.Instance.Delay); // Trigger jump event
    }

    void TriggerJumpEvent()
    {
        OnPlayerJump?.Invoke();
    }

    void TriggerLeftMove()
    {
        PlayerHorizontalMovement?.Invoke(-1);
    }

    void TriggerRightMove()
    {
        PlayerHorizontalMovement?.Invoke(1);
    }

    void SendPlayerPosition()
    {
        OnPlayerPositionUpdate?.Invoke(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            UIManager.Instance.UpdateHealth();
            CameraShake.TriggerShake(1, .1f);
            VibratePhone();
        }
        if (other.transform.tag == "Orb")
        {
            UIManager.Instance.UpdateCoinCount();
            var enable = other.GetComponentInChildren<ParticleSystem>().emission;
            enable.enabled = true;
            other.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    private IEnumerator Delay()
    {

        yield return new WaitForSeconds(3f);
        UIManager.Instance.GameoverPrefab.SetActive(true);

    }

    void VibratePhone()
    {
        // Check if the platform supports vibration
        if (SystemInfo.supportsVibration)
        {
            // Vibrate for 500 milliseconds (0.5 seconds)
            Handheld.Vibrate();
            Debug.Log("Vibration triggered!");
        }
        else
        {
            Debug.LogWarning("Vibration is not supported on this device.");
        }
    }
}