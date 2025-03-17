using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MirrorPlayer : MonoBehaviour
{
    private Queue<Vector3> positionQueue = new Queue<Vector3>();
    private Queue<int> horizontalMovementQueue = new Queue<int>();


    [SerializeField]
    private PlayerManager Player;

    void OnEnable()
    {
        // Subscribe to player events
        Player.OnPlayerPositionUpdate += OnPlayerPositionUpdated;
        Player.OnPlayerJump += OnPlayerJumped;
        Player.PlayerHorizontalMovement += Player_HorizontalMovement;
    }

    private void Player_HorizontalMovement(int obj)
    {
        // Add the player's position to the queue
        horizontalMovementQueue.Enqueue(obj);
        var dir = horizontalMovementQueue.Dequeue();
        if (dir == 1) { // Move Right
            transform.position = new Vector3(transform.position.x + 1.32f, transform.position.y, transform.position.z);
        }
        if (dir == -1)
        {
            transform.position = new Vector3(transform.position.x - 1.32f, transform.position.y, transform.position.z);
        }
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        Player.OnPlayerPositionUpdate -= OnPlayerPositionUpdated;
        Player.OnPlayerJump -= OnPlayerJumped;
    }
    void OnPlayerPositionUpdated(Vector3 position)
    {
        // Add the player's position to the queue
        positionQueue.Enqueue(position);

        // Apply delayed position updates
        //if (positionQueue.Count > GameManager.Instance.Delay / Time.deltaTime)
        //{
            var pos = positionQueue.Dequeue();
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, pos.z), 1);
            UIManager.Instance.UpdateDistance(.1f);
        //if (positionQueue.Count == 0)
        //{
        //}
        //}
    }

    void OnPlayerJumped()
    {
        // Mimic the player's jump
        GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Obstacle")
        {
            Destroy(other.gameObject);
        }

        if (other.transform.tag == "Orb")
        {
            Destroy(other.gameObject);
        }
    }

}
