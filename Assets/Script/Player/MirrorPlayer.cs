using System;
using System.Collections;
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
        var pos = positionQueue.Dequeue();
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, pos.z), 1);
        UIManager.Instance.UpdateDistance(.1f);
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
            gameObject.GetComponent<Animator>().Play("DissolveEffect");
        }
        if (other.transform.tag == "Orb")
        {
            //Destroy(other.gameObject);
            var go = other.gameObject.transform.GetChild(0).gameObject;
            go.SetActive(true);
            var enable = go.GetComponent<ParticleSystem>().emission;
            enable.enabled = true;
            other.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(HideParticleSystem(go,other.gameObject));
        }
    }
    private IEnumerator HideParticleSystem(GameObject _go,GameObject parent)
    {
        yield return new WaitForSeconds(.1f);
        var enable = _go.GetComponent<ParticleSystem>().emission;
        enable.enabled = false;
        _go.SetActive(false);
        parent.SetActive(false);
    }
}
