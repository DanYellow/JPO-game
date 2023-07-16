using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private VoidEventChannel OnFirstLevelStart;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Open();
            yield return null;
            yield return new WaitForSeconds(animator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            
            Animator animatorPlayer = other.GetComponent<Animator>();
            Rigidbody2D rbPlayer = other.GetComponent<Rigidbody2D>();
            other.GetComponent<PlayerMovements>().enabled = false;
            rbPlayer.velocity = new Vector2(0.5f, 0); 
            // animatorPlayer.SetTrigger("EntersDoor");
            // yield return null;
            // yield return new WaitForSeconds(animatorPlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            
            // Close();
            // yield return null;
            // yield return new WaitForSeconds(animator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * 1.5f);
            // OnFirstLevelStart.Raise();
        }
        yield return null;
    }

    public void Open()
    {
        animator.SetBool("IsOpening", true);
    }

    public void Close()
    {
        animator.SetBool("IsOpening", false);
    }
}
