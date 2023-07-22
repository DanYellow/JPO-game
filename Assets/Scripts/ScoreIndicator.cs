using UnityEngine.UI;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField]
    Sprite activeSprite;

    Image image;

    [SerializeField]
    CameraShakeTypeValue shakeAppear;

    [SerializeField]
    CinemachineShakeEventChannel onAppearence;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void Activate() {
        image.sprite = activeSprite;
        onAppearence.Raise(shakeAppear);
    }
}
