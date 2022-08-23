using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public GameObject player1;
    public bool isPlayer1Dead;
    public GameObject player2;
    public bool isPlayer2Dead;

    public void PlayerDied(int playerNumber)
    {
        if(playerNumber == 1) isPlayer1Dead = true;
        if(playerNumber == 2) isPlayer2Dead = true; 
    }
}
