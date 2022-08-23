using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        highestScore = PlayerPrefs.GetInt("SavedScore");
    }
    private void Start()
    {
        
    }

    public GameObject player1;
    public GameObject player2;

    int score;
    public int highestScore;
    public void PlayerDied(int playerNumber)
    {

    }

    public void SetScore(int playerScore)
    {
        score = playerScore;
        if(score > highestScore)
        {
            PlayerPrefs.SetInt("SavedScore", score);
        }
    }
}
