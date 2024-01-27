using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillationBehaviour : StateMachineBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float floatStrength = 0.2f;
    [SerializeField]
    private float speed = 5;

    private Vector2 startingPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float newY = (Mathf.Sin(Time.time * speed) * floatStrength) + startingPosition.y;
        Vector2 position = new Vector2(rb.position.x, newY);
        rb.MovePosition(position);
    }
}
