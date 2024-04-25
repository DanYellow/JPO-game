using UnityEngine;

public class MeteorImpact : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    [SerializeField]
    private Transform world;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        transform.LookAt(world);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            onCarSlowdown.Raise();
        }
    }
}
