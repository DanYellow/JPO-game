using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    private Vector3 moveInput;
    
    public void OnShoot(InputAction.CallbackContext ctx) {
        if(ctx.phase == InputActionPhase.Performed) {
            Debug.Log("OnAttack2 ");
        }
    }
}
