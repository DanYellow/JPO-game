using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamageable
{
    private Animator animator;
    private List<GameObject> children = new List<GameObject>();

    private int currentLifePoints = 3;

    [SerializeField]
    private MaterialChangeValue materialChange;

    private MaterialManager materialManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        materialManager = GetComponent<MaterialManager>();

        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            if(g != transform) {
                children.Add(g.gameObject);
            }
        }
        foreach (var item in children)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            child.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentLifePoints -= damage;
        materialManager.ChangeMaterialProxy(materialChange);
        // animator.SetTrigger(AnimationStrings.isHit);
        if (currentLifePoints <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        // animator.SetTrigger(AnimationStrings.die);
        foreach (var item in children)
        {
            item.SetActive(true);
            item.transform.SetParent(null);
        }

        Destroy(gameObject);
    }

    void OnGUI()
    {
        //Output the first Animation speed to the screen
        // GUI.Label(new Rect(25, 25, 200, 20),  "fullPathHash of State : " + animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
    }
}
