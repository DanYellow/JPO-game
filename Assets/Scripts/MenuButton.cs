using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    private TextMeshProUGUI text;
    private Color origColor;

    [SerializeField]
    private Material hoverMaterial;
    // private Color hoverColor = new Vector4(1.988f, 0.438f, 0.438f, 1.0f);

    [ColorUsageAttribute(true, true), SerializeField]
    public Color hoverColor;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        origColor = text.faceColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.faceColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.faceColor = origColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        text.faceColor = hoverColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.faceColor = origColor;
    }
}