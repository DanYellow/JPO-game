using TMPro;
using UnityEngine;

public class DisplayBuildVersion : MonoBehaviour
{
    private void Awake() {
        Debug.Log("Application.version" + Application.version);
        if(TryGetComponent(out TextMeshProUGUI text)) {
        Debug.Log("Application.version" + Application.version);
            text.SetText(Application.version);
        }
    }
}
