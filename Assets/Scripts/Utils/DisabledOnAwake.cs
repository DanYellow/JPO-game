using UnityEngine;

public class ActiveOnStart : MonoBehaviour
{
    public bool isActive = false;
    private void Awake() {
        gameObject.SetActive(false);
    }
}
