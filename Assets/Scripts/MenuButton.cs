using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    private MenuButton[] listMenuButton;
    private void Awake()
    {
        listMenuButton = GameObject.FindObjectsOfType<MenuButton>();
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var item in listMenuButton)
        {   
            if(gameObject == item.gameObject) {
                item.GetComponent<Animator>().SetBool("IsSelected", true);
            } else {
                item.GetComponent<Animator>().SetBool("IsSelected", false);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        foreach (var item in listMenuButton)
        {   
            item.GetComponent<Animator>().SetBool("IsSelected", false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        foreach (var item in listMenuButton)
        {   
            if(gameObject == item.gameObject) {
                item.GetComponent<Animator>().SetBool("IsSelected", true);
            } else {
                item.GetComponent<Animator>().SetBool("IsSelected", false);
            }
        }
    }

}
