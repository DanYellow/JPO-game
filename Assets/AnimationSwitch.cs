using System.Collections.Generic;
using UnityEngine;

public class AnimationSwitch : MonoBehaviour
{
    private Animator animator;
    private List<string> listNameClip = new List<string>();

    public string animationName = "PlayerRun";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        animator.Play(animationName);
    }
}
