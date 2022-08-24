using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    float sec;
    int min;

    private void Awake()
    {
        Instance = this;
        highestScore = PlayerPrefs.GetInt("SavedScore");
    }
    private void Start()
    {

    }

    private void Update()
    {
        sec += Time.deltaTime;
        if (sec >= 60)
        {
            min++;
            sec = 0;
        }
    }

    public string GetTime()
    {
        return "Time : " + min.ToString("00") + ":" + Mathf.Floor(sec).ToString("00");
    }

    public GameObject player1;
    public bool isPlayer1Dead;
    public GameObject player2;
    public bool isPlayer2Dead;

    public int score;
    public int highestScore;
    public void PlayerDied(int playerNumber)
    {
        if (playerNumber == 1) isPlayer1Dead = true;
        if (playerNumber == 2) isPlayer2Dead = true;
    }

    public void SetScore(int playerScore)
    {
        score = playerScore;
        if (score > highestScore)
        {
            PlayerPrefs.SetInt("SavedScore", score);
        }
    }
}
