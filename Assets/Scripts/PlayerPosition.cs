using UnityEngine;

public class Playerposition : MonoBehaviour
{
    private float heightSprite;

    private void Awake() {
        heightSprite = GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
    }

    public void SetPosition(Transform transform) {
        GetComponent<Rigidbody>().position = new Vector3(
            transform.position.x,
            transform.position.y + heightSprite,
            transform.position.z
        );
    }
}
