using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    public int nbDuplicates = 0;
    public int xSpacing;
    public int ySpacing;

    public GameObject objectDuplicated; 

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = objectDuplicated.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        float startPosX = transform.position.x;
        float startPosY = transform.position.y;

        for (int i = 0; i < nbDuplicates; i++)
        {
            Vector2 nextPos = new Vector2(
                xSpacing == 0 ? gameObject.transform.position.x : (sr.bounds.size.x * i) + (xSpacing * i) + startPosX,
                ySpacing == 0 ? gameObject.transform.position.y : (sr.bounds.size.y * i) + (ySpacing * i) + startPosY
                // gameObject.transform.position.y
            );
            GameObject duplicate = Instantiate(objectDuplicated, nextPos, Quaternion.identity);
            duplicate.SetActive(true);
        }
    }
}