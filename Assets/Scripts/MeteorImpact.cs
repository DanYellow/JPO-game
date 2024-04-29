using UnityEngine;

public class MeteorImpact : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            onCarSlowdown.Raise();
        }
    }
}
