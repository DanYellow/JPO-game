using UnityEngine;  
using TMPro;
using UnityEngine.EventSystems;  

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private TextMeshProUGUI text;

    [SerializeField]
    private Color hoverColor = Color.red;

    [SerializeField]
    private Color pressedColor = Color.red;

    private Color originalColor;

    private void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        // Button button = GetComponent<Button>();
        originalColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.color = pressedColor;
    }
}
