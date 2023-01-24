using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    private MenuButton[] listMenuButton;
    private void Awake()
    {
        listMenuButton = GameObject.FindObjectsOfType<MenuButton>();
        Debug.Log("listMenuButton" + listMenuButton.Length);
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
        // animator.SetBool("IsSelected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // animator.SetBool("IsSelected", false);
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

    public void OnDeselect(BaseEventData eventData)
    {
        // animator.SetBool("IsSelected", false);
    }
}
