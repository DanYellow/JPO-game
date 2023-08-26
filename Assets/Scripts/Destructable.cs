using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamageable
{
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger(AnimationStrings.isHit);
    }
}
