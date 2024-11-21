using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI textContainer;

    [SerializeField]
    private Color hoverColor = Color.red;

    [SerializeField]
    private Color pressedColor = Color.red;
    [SerializeField]
    private Color disabledColor = Color.gray;

    private Color normalColor;

    private string originalText;

    private Color originalColor;

    [SerializeField]
    private bool keepImageButton = false;

    private Button button;

    private void Awake()
    {
        textContainer = GetComponentInChildren<TextMeshProUGUI>();
        normalColor = textContainer.color;
        button = GetComponent<Button>();
        button.GetComponent<Image>().enabled = keepImageButton;

        originalColor = textContainer.color;
        originalText = textContainer.text;

        if (button.interactable == false)
        {
            textContainer.color = disabledColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        textContainer.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        textContainer.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        textContainer.color = pressedColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!button.interactable) return;
        if (eventData.selectedObject == gameObject)
        {
            textContainer.color = hoverColor;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!button.interactable) return;
        if (eventData.selectedObject == gameObject)
        {
            textContainer.color = originalColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Enable()
    {
        textContainer.color = normalColor;
        button.interactable = true;
    }

    public void Disable()
    {
        textContainer.color = disabledColor;
        button.interactable = false;
    }
}