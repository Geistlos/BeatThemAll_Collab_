using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    float sec;
    int min;
    bool uI;

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

        if (Input.anyKey && uI)
        {
            Debug.Log("Menu");
            SceneManager.LoadScene("Menu");
        }
    }

    public string GetTime()
    {
        player1.GetComponent<PlayerBehavior>().enabled = false;
        Debug.Log(""+ player1.GetComponent<PlayerBehavior>().currentState.ToString());
        StartCoroutine(WaitForUi());
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

    IEnumerator WaitForUi()
    {
        yield return new WaitForSeconds(2);
        uI = true;
    }
}
