using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Tooltip("Define where the player will spawn if there is an issue"), ReadOnlyInspector]
    public Vector3 currentSpawnPosition;

    [Tooltip("Define where the player started the level"), ReadOnlyInspector]
    public Vector3 initialSpawnPosition;

    [SerializeField]
    private VoidEventChannel resetLastCheckPoint;

    private void OnEnable() {
        resetLastCheckPoint.OnEventRaised += RestartLastCheckpoint;
    }

    private void Awake()
    {
        currentSpawnPosition = gameObject.transform.position;
        initialSpawnPosition = gameObject.transform.position;
    }

    void RestartLastCheckpoint()
    {
        transform.position = currentSpawnPosition;
    }

    private void OnDisable() {
        resetLastCheckPoint.OnEventRaised -= RestartLastCheckpoint;
    }
}
