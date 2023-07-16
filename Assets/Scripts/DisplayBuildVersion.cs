using TMPro;
using UnityEngine;

public class DisplayBuildVersion : MonoBehaviour
{
    private void Awake() {
        if(TryGetComponent(out TextMeshProUGUI text)) {
            text.SetText($"v{Application.version}");
        }
    }
}
