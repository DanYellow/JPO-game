using System.Collections;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    public void Face(Transform target)
    {
        if (target.position.x > transform.position.x && !isFacingRight || target.position.x < transform.position.x && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public IEnumerator FaceWithDelay(Transform target, float delay = 2) {
        yield return Helpers.GetWait(delay);
        Face(target);
    } 
}
