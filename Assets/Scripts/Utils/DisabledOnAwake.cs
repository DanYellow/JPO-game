using UnityEngine;

public class DisabledOnAwake : MonoBehaviour
{
    public bool isActive = false;
    private void Awake() {
        // gameObject.SetActive(false);
    }
}
