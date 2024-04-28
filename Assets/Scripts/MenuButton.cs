using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    private Color origColor;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        origColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = new Color(0, 245, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = origColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        text.color = new Color(0, 245, 255);
    }
}