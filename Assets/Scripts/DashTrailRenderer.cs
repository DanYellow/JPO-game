using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrailRenderer : MonoBehaviour
{
    [SerializeField]
    private int clonesPerSecond = 3;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Transform tf;
    private List<SpriteRenderer> clones;
    public Vector3 scalePerSecond = new Vector3(1f, 1f, 1f);
    public Color colorPerSecond = new Color(255, 255, 255, 1f);
    [HideInInspector]
    public bool emit = false;

    [SerializeField]
    private bool useScale = false;
    [SerializeField]
    private bool useColor = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        
        clones = new List<SpriteRenderer>();
        StartCoroutine(Trail());
    }

    void Update()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            clones[i].sortingOrder = 9;
            if (useColor)
            {
                clones[i].color -= colorPerSecond * Time.deltaTime;
            }

            if (useScale)
            {
                clones[i].transform.localScale -= scalePerSecond * Time.deltaTime;
            }

            if (clones[i].color.a <= 0f || clones[i].transform.localScale == Vector3.zero)
            {
                Destroy(clones[i].gameObject);
                clones.RemoveAt(i);
                i--;
            }
        }
    }

    public IEnumerator Clear()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            Destroy(clones[i].gameObject);
            clones.RemoveAt(i);
        }

        yield return new WaitUntil(() => clones.Count == 0);
    }

    IEnumerator Trail()
    {
        int i = 0;
        while (true)
        {
            if (emit)
            {
                GameObject clone = new GameObject($"trailClone ({i})");
                clone.transform.position = tf.position;
                clone.transform.right = tf.right.normalized;

                SpriteRenderer cloneRend = clone.AddComponent<SpriteRenderer>();
                cloneRend.sprite = sr.sprite;
                cloneRend.sortingOrder = sr.sortingOrder - 1;
                clones.Add(cloneRend);
                i++;
            }

            yield return new WaitForSecondsRealtime(1f / clonesPerSecond);
        }
    }
}
