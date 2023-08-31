using UnityEngine;

public class Followed : MonoBehaviour
{
    [SerializeField] Transform follower;
    private BoxCollider2D bc;
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        follower.position = new Vector3(
            bc.bounds.center.x,
            follower.position.y,
            follower.position.z
        );
    }
}
