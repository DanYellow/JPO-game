using UnityEngine;
using UnityEngine.Events;

public class PlayerPosition : MonoBehaviour
{
    private float heightSprite;

    [SerializeField]
    private UnityEvent onPositionSet;

    private void Awake()
    {
        heightSprite = GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
    }

    public void SetPosition(Transform transform)
    {
        GetComponent<Rigidbody>().position = new Vector3(
            transform.position.x,
            transform.position.y + heightSprite,
            transform.position.z
        );
        onPositionSet.Invoke();
    }
}
