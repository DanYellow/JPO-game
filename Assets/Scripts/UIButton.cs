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

    private string originalText;

    private Color originalColor;

    [SerializeField]
    private bool keepImageButton = false;

    private Button button;

    private void Awake()
    {
        textContainer = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.GetComponent<Image>().enabled = keepImageButton;

        originalColor = textContainer.color;
        originalText = textContainer.text;
    }

    private void Update()
    {
        if (button.interactable == false)
        {
            textContainer.color = disabledColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textContainer.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textContainer.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        textContainer.color = pressedColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == gameObject)
        {
            textContainer.color = hoverColor;
            // textContainer.SetText($"►{originalText}◄");
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (eventData.selectedObject == gameObject)
        {
            textContainer.color = originalColor;
            // textContainer.SetText(originalText);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}