using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructable : MonoBehaviour, IDamageable
{
    private Animator animator;
    private List<GameObject> children = new List<GameObject>();

    private int currentLifePoints = 1;

    [SerializeField]
    private MaterialChangeValue materialChange;

    private MaterialManager materialManager;

    [SerializeField]
    private GameObject treasure; 

    [SerializeField]
    public UnityEvent onDone;

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

    public int GetHealth() {
        return currentLifePoints;
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

        if(treasure != null) {
            Instantiate(treasure, transform.position, Quaternion.identity);
        }

        onDone?.Invoke();
        Destroy(gameObject);
    }

    void OnGUI()
    {
        //Output the first Animation speed to the screen
        // GUI.Label(new Rect(25, 25, 200, 20),  "fullPathHash of State : " + animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
    }
}
