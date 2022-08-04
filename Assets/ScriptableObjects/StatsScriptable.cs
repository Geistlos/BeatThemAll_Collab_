using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntitieStats", order = 1)]
public class StatsScriptable : ScriptableObject
{
    public float life;
    public float speed;
    public float runningSpeed;
    public float dmg;
    public float attackSpeed;
    public Animator animator;

    //Players'only stats
    public float airTime;
    public float jumpHeight;
    public float jumpingSpeed;
}