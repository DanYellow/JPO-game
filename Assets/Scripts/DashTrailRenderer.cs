using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class DashTrailRenderer : MonoBehaviour
{
    [SerializeField]
    private int clonesPerSecond = 3;
    private SpriteRenderer sr;

    public Vector3 scalePerSecond = new Vector3(1f, 1f, 1f);
    public Color colorPerSecond = new Color(255, 255, 255, 1f);
    [HideInInspector]
    public bool emit = false;

    [SerializeField]
    private bool useScale = false;
    [SerializeField]
    private bool useColor = false;

    [SerializeField]
    private Material material;

    private IObjectPool<GameObject> pool;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        pool = new ObjectPool<GameObject>(
                () => CreateFunc(),
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                true
            );

        StartCoroutine(GenerateClones());
    }

    GameObject CreateFunc()
    {
        GameObject clone = new GameObject();
        clone.transform.position = transform.position;
        clone.transform.right = transform.right.normalized;

        SpriteRenderer srClone = clone.AddComponent<SpriteRenderer>();
        srClone.sprite = sr.sprite;
        srClone.material = material;
        // srClone.color = colorPerSecond;
        srClone.sortingOrder = sr.sortingOrder - 1;

        return clone;
    }

    IEnumerator DisableClone(GameObject go)
    {
        SpriteRenderer srClone = go.GetComponent<SpriteRenderer>();
        float startTime = Time.time;

        while ((Time.time - startTime) < 0.25f)
        {
            // srClone.color -= colorPerSecond * Time.deltaTime;
            // .transform.localScale -= scalePerSecond * Time.deltaTime;
            yield return null;
        }

        pool.Release(go);
    }

    void ActionOnGet(GameObject item)
    {
        item.transform.position = transform.position;
        item.transform.right = transform.right.normalized;
        SpriteRenderer srClone = item.GetComponent<SpriteRenderer>();
        // Color color = srClone.color;
        // color.a = 1;
        // srClone.color = color;

        item.SetActive(true);
        StartCoroutine(DisableClone(item));
    }

    void ActionOnRelease(GameObject item)
    {
        item.SetActive(false);
    }

    void ActionOnDestroy(GameObject item)
    {
        Destroy(item);
    }

    IEnumerator GenerateClones()
    {
        int i = 0;

        while (true)
        {
            if (emit)
            {
                GameObject clone = pool.Get();
                clone.name = $"TrailClone_{i}";

                i++;
            }

            yield return new WaitForSecondsRealtime(1f / clonesPerSecond);
        }
    }
}
