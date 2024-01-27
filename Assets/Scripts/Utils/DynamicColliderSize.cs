using UnityEngine;

public class DynamicColliderSize : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D bc2d;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 spriteSize = sr.sprite.bounds.size;
        bc2d.size = new Vector2(bc2d.size.x, spriteSize.y);
    }
}
