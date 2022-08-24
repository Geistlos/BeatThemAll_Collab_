using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public static Level1Manager Instance;
    public enum LevelState {wave1,wave1p,wave2,wave2p,wave3,wave4,wave5,wave6,transi1,transi2,transi3,transi4,transi5}
    public LevelState currentState;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<GameObject> enemyPrefab;
    [SerializeField] List<BoxCollider2D> boundColliders;
    [SerializeField] CameraMovement camMovment;
    [SerializeField] GameObject goArrow;
    Transform playerTransform;
    float timer = 0f;
    public int killCount;
    bool finLevel1;

    void OnStateEnter()
    {
        switch (currentState)
        {
            case LevelState.wave1:
                Instantiate(enemyPrefab[1], spawnPoints[1]);
                break;
            case LevelState.wave1p:
                Instantiate(enemyPrefab[1], spawnPoints[2]);
                Instantiate(enemyPrefab[1], spawnPoints[3]);
                break;
            case LevelState.transi1:
                camMovment.follow = true;
                goArrow.SetActive(true);
                break;
            case LevelState.wave2:
                camMovment.follow = false;
                goArrow.SetActive(false);
                Instantiate(enemyPrefab[1], spawnPoints[4]);
                Instantiate(enemyPrefab[1], spawnPoints[5]);
                break;
            case LevelState.wave2p:
                Instantiate(enemyPrefab[1], spawnPoints[6]);
                Instantiate(enemyPrefab[1], spawnPoints[7]);
                Instantiate(enemyPrefab[1], spawnPoints[8]);
                Instantiate(enemyPrefab[1], spawnPoints[9]);
                break;
            case LevelState.transi2:
                camMovment.follow = true;
                goArrow.SetActive(true);
                break;
            case LevelState.wave3:
                goArrow.SetActive(false);
                camMovment.cameraBounds = boundColliders[1];
                Instantiate(enemyPrefab[1], spawnPoints[10]);
                Instantiate(enemyPrefab[1], spawnPoints[11]);
                Instantiate(enemyPrefab[2], spawnPoints[12]);
                Instantiate(enemyPrefab[2], spawnPoints[13]);
                Instantiate(enemyPrefab[3], spawnPoints[14]);
                Instantiate(enemyPrefab[3], spawnPoints[15]);
                break;
            case LevelState.transi3:
                goArrow.SetActive(true);
                camMovment.cameraBounds = boundColliders[0];
                break;
            case LevelState.wave4:
                camMovment.follow = false;
                goArrow.SetActive(false);
                Instantiate(enemyPrefab[1], spawnPoints[16]);
                Instantiate(enemyPrefab[1], spawnPoints[17]);
                Instantiate(enemyPrefab[1], spawnPoints[18]);
                Instantiate(enemyPrefab[1], spawnPoints[19]);
                Instantiate(enemyPrefab[2], spawnPoints[20]);
                Instantiate(enemyPrefab[2], spawnPoints[21]);
                Instantiate(enemyPrefab[3], spawnPoints[22]);
                Instantiate(enemyPrefab[3], spawnPoints[23]);
                break;
            case LevelState.transi4:
                camMovment.follow = true;
                goArrow.SetActive(true);
                break;
            case LevelState.wave5:
                goArrow.SetActive(false);
                camMovment.cameraBounds = boundColliders[2];
                Instantiate(enemyPrefab[1], spawnPoints[24]);
                Instantiate(enemyPrefab[1], spawnPoints[25]);
                Instantiate(enemyPrefab[2], spawnPoints[26]);
                Instantiate(enemyPrefab[2], spawnPoints[27]);
                Instantiate(enemyPrefab[2], spawnPoints[28]);
                Instantiate(enemyPrefab[2], spawnPoints[29]);
                Instantiate(enemyPrefab[3], spawnPoints[30]);
                Instantiate(enemyPrefab[3], spawnPoints[31]);
                Instantiate(enemyPrefab[3], spawnPoints[32]);
                Instantiate(enemyPrefab[3], spawnPoints[33]);
                break;
            case LevelState.transi5:
                goArrow.SetActive(true);
                camMovment.cameraBounds = boundColliders[0];
                break;
            case LevelState.wave6:
                goArrow.SetActive(false);
                camMovment.cameraBounds = boundColliders[3];
                Instantiate(enemyPrefab[1], spawnPoints[34]);
                Instantiate(enemyPrefab[1], spawnPoints[35]);
                Instantiate(enemyPrefab[2], spawnPoints[36]);
                Instantiate(enemyPrefab[2], spawnPoints[37]);
                Instantiate(enemyPrefab[2], spawnPoints[38]);
                Instantiate(enemyPrefab[2], spawnPoints[39]);
                Instantiate(enemyPrefab[2], spawnPoints[40]);
                Instantiate(enemyPrefab[3], spawnPoints[41]);
                Instantiate(enemyPrefab[3], spawnPoints[42]);
                Instantiate(enemyPrefab[3], spawnPoints[43]);
                Instantiate(enemyPrefab[3], spawnPoints[44]);
                Instantiate(enemyPrefab[3], spawnPoints[45]);
                break;
            default:
                break;
        }
    }
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case LevelState.wave1:
                if(killCount == 1)
                {
                    timer += Time.deltaTime;
                    if (timer > 3f) TransitionToState(LevelState.wave1p);
                }
                break;
            case LevelState.wave1p:
                if (killCount == 3) TransitionToState(LevelState.transi1);
                break;
            case LevelState.transi1:
                if (playerTransform.position.x >= 8.25f) TransitionToState(LevelState.wave2);
                break;
            case LevelState.wave2:
                if (killCount == 5)
                {
                    timer += Time.deltaTime;
                    if (timer > 3f) TransitionToState(LevelState.wave2p);
                }
                break;
            case LevelState.wave2p:
                if (killCount == 9) TransitionToState(LevelState.transi2);
                break;
            case LevelState.transi2:
                if (playerTransform.position.x >= 32f) TransitionToState(LevelState.wave3);
                break;
            case LevelState.wave3:
                if (killCount == 15) TransitionToState(LevelState.transi3);
                break;
            case LevelState.transi3:
                if (playerTransform.position.x >= 51f) TransitionToState(LevelState.wave4);
                break;
            case LevelState.wave4:
                if (killCount == 23)TransitionToState(LevelState.transi4);
                break;
            case LevelState.transi4:
                if (playerTransform.position.x >= 69f) TransitionToState(LevelState.wave5);
                break;
            case LevelState.wave5:
                if (killCount == 33) TransitionToState(LevelState.transi5);
                break;
            case LevelState.transi5:
                if (playerTransform.position.x >= 98f) TransitionToState(LevelState.wave6);
                break;
            case LevelState.wave6:
                if (killCount == 45 && !finLevel1) {Debug.Log("Level 1 Finished"); finLevel1 = true; }
                break;
            default:
                break;
        }
    }
    void OnStateExit()
    {
        switch (currentState)
        {
            case LevelState.wave1:
                timer = 0f;
                break;
            case LevelState.wave2:
                timer = 0f;
                break;
            case LevelState.wave3:
                timer = 0f;
                break;
            case LevelState.wave4:
                timer = 0f;
                break;
            case LevelState.wave5:
                timer = 0f;
                break;
            default:
                break;
        }
    }
    void TransitionToState(LevelState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    void Update()
    {
        OnStateUpdate();
    }

    void Start()
    {
        TransitionToState(LevelState.wave1);
        playerTransform = GameManager.Instance.player1.transform;
    }
    private void Awake()
    {
        Instance = this;
    }
}
