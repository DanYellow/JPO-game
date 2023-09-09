using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Tooltip("Define where the player will spawn if there is an issue"), ReadOnlyInspector]
    public Vector3 currentSpawnPosition;

    [Tooltip("Define where the player started the level"), ReadOnlyInspector]
    public Vector3 initialSpawnPosition;

    [SerializeField]
    private Vector2Value lastCheckpointPosition;

    private void Awake()
    {
        if(lastCheckpointPosition.CurrentValue != null) {
            gameObject.transform.position = (Vector3) lastCheckpointPosition.CurrentValue;
            lastCheckpointPosition.CurrentValue = null;
        }
        currentSpawnPosition = gameObject.transform.position;
        initialSpawnPosition = gameObject.transform.position;
    }

    public void SetLastGroundPosition(Vector2 pos) {
        currentSpawnPosition = pos;
    }
}
