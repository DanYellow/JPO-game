using UnityEngine.UI;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField]
    Sprite activeSprite;

    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void Activate() {
        image.sprite = activeSprite;
    }
}
