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
    public GameObject player2;

    public void PlayerDied(int playerNumber)
    {

    }
}
