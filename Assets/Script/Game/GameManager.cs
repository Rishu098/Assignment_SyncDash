using UnityEngine;

public class GameManager : MonoSingletonGeneric<GameManager>
{
    public float Delay = 0.1f;

    public float GameSpeed = 5;

    public float IncreaseSpeedDistance = 100f;
    public float TotalDistanceToCover = 300f;

    public bool isPlaying = true;
    public bool isGameOver = false;

    public AudioSource CoinCollectSound;
}
