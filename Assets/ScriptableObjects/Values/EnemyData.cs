
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "ScriptableObjects/Values/EnemyData", order = 0)]
public class EnemyData : CharacterData
{
    [Range(0, 35)]
    public float baseWalkSpeed = 3;

    [HideInInspector]
    public float walkSpeed = 3;

    [Range(0, 35)]
    public float baseRunSpeed = 3;

    [HideInInspector]
    public float runSpeed = 3;

    public float attackRate = 1;
    public int damage = 1;

    public new string name = "";

    public float distanceDetector = 0;

    public float obstacleCheckRadius = 0.25f;

    public int knockbackForce = 0;

    public GameObject dropItem;
    public GameObject blastEffect;
    [Range(0, 1)]
    public float dropProbability = 0.25f;

    [SerializeField]
    private BoolEventChannel onInteractRangeEvent;

    private void Awake()
    {
        walkSpeed = baseWalkSpeed;
        runSpeed = baseRunSpeed;
    }

    private void OnEnable()
    {
        onInteractRangeEvent.OnEventRaised += ToggleTime;
        ToggleTime(false);
    }

    void ToggleTime(bool isPaused)
    {
        runSpeed = isPaused ? 0 : baseRunSpeed;
        walkSpeed = isPaused ? 0 : baseWalkSpeed;
    }

    private void OnDisable()
    {
        onInteractRangeEvent.OnEventRaised -= ToggleTime;
    }
}
