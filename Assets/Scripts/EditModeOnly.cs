using UnityEngine;
// https://www.youtube.com/watch?v=GPxDIUcNX-o
public class EditModeOnly : MonoBehaviour
{
    public bool isActive = false;
    private void Awake()
    {
#if UNITY_EDITOR
#else
    isActive = false;
#endif
        gameObject.SetActive(isActive);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
