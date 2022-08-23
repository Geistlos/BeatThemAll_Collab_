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
    Transform playerTransform;
    float timer = 0f;
    public int killCount;
    int spawnCount;

    void OnStateEnter()
    {
        switch (currentState)
        {
            case LevelState.wave1:
                Instantiate(enemyPrefab[1], spawnPoints[1]);
                spawnCount++;
                break;
            case LevelState.wave1p:
                Instantiate(enemyPrefab[1], spawnPoints[2]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[3]);
                spawnCount++;
                break;
            case LevelState.transi1:
                camMovment.follow = true;
                break;
            case LevelState.wave2:
                camMovment.follow = false;
                Instantiate(enemyPrefab[1], spawnPoints[4]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[5]);
                spawnCount++;
                break;
            case LevelState.wave2p:
                Instantiate(enemyPrefab[1], spawnPoints[6]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[7]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[8]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[9]);
                spawnCount++;
                break;
            case LevelState.transi2:
                camMovment.follow = true;
                break;
            case LevelState.wave3:
                camMovment.cameraBounds = boundColliders[1];
                Instantiate(enemyPrefab[1], spawnPoints[10]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[11]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[12]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[13]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[14]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[15]);
                spawnCount++;
                break;
            case LevelState.transi3:
                camMovment.cameraBounds = boundColliders[0];
                break;
            case LevelState.wave4:
                camMovment.follow = false;
                Instantiate(enemyPrefab[1], spawnPoints[16]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[17]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[18]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[19]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[20]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[21]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[22]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[23]);
                spawnCount++;
                break;
            case LevelState.transi4:
                camMovment.follow = true;
                break;
            case LevelState.wave5:
                camMovment.cameraBounds = boundColliders[2];
                Instantiate(enemyPrefab[1], spawnPoints[24]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[25]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[26]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[27]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[28]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[29]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[30]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[31]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[32]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[33]);
                spawnCount++;
                break;
            case LevelState.transi5:
                camMovment.cameraBounds = boundColliders[0];
                break;
            case LevelState.wave6:
                camMovment.cameraBounds = boundColliders[3];
                Instantiate(enemyPrefab[1], spawnPoints[34]);
                spawnCount++;
                Instantiate(enemyPrefab[1], spawnPoints[35]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[36]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[37]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[38]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[39]);
                spawnCount++;
                Instantiate(enemyPrefab[2], spawnPoints[40]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[41]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[42]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[43]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[44]);
                spawnCount++;
                Instantiate(enemyPrefab[3], spawnPoints[45]);
                spawnCount++;
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
                if (killCount == 45) Debug.Log("Level 1 Finished");
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
