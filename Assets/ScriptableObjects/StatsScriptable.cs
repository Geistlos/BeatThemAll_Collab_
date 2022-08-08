using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntitieStats", order = 1)]
public class StatsScriptable : ScriptableObject
{
    public int life;
    public float speed;
    public float runningSpeed;
    public int dmg;
    public float attackSpeed;
    public RuntimeAnimatorController animator;

    //Players'only stats
    public float airTime;
    public float jumpHeight;
    public float jumpingSpeed;
}