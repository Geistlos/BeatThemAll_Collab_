using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntitieStats", order = 1)]
public class StatsScriptable : ScriptableObject
{
    //Shares stats
    public int life;
    public float speed;
    public float runningSpeed;
    public int dmg;
    public float attackSpeed;
    public RuntimeAnimatorController animator;
    public float radiusHitbox;
    
    //Players stats
    public float airTime;
    public float jumpHeight;
    public float jumpingSpeed;
    public float invulnerabilityDuration;
    public int numberOfBlinks;
    public float delayBetweenBlinks;
    public int healthPerCan;
    public int energyPerCan;
    public float energyPerAtk;
    public float energy;

    //Enemy stats
    public int collectiblesMin;
    public int collectiblesMax;
}