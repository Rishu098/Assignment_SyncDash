using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingletonGeneric<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private TextMeshProUGUI coinCount;

    [SerializeField]
    private TextMeshProUGUI distance;

    private int currentHealthCount = 3;
    private int cointCount = 0;

    public GameObject GameoverPrefab;

    [HideInInspector]
    public float initialDistance = 0;

    private float increaseSpeedDistance = 100f;
    public void UpdateHealth()
    {
        currentHealthCount--;
        healthText.text = "X " + currentHealthCount.ToString(); 
    }

    public void UpdateCoinCount() 
    {
        cointCount++;
        coinCount.text = "X " + cointCount.ToString();
    }

    public void UpdateDistance(float x) 
    {
        initialDistance += x;
        distance.text = "Distance x " + Mathf.Ceil(initialDistance);
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void MainMenuBtn() { 
        SceneManager.LoadScene(0);
    }
}
