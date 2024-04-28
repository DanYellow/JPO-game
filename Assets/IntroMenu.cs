using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject gameTitle;
    private List<GameObject> listEchoTitles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        // gameTitle.transform.localScale *= 3;
        StartCoroutine(DisplayTitle());
        // StartCoroutine(HideEchoTitles());
    }

    IEnumerator DisplayTitle()
    {
        int count = 0;
        float step = 0.1f;
        float yStep = 5f;
        int maxItems = 10;

        

        while (listEchoTitles.Count < maxItems)
        {
            count++;

            GameObject go = Instantiate(gameTitle);
            go.transform.position = gameTitle.transform.position;
            go.transform.position -= new Vector3(0, yStep * count, 0);
            go.GetComponentInChildren<TextMeshProUGUI>().faceColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            go.transform.parent = gameTitle.transform.parent;
            go.transform.localScale = Vector3.one;
            go.transform.SetSiblingIndex(maxItems - count);
            listEchoTitles.Add(go);

            yield return null;
        }
        gameTitle.transform.SetSiblingIndex(maxItems + 1);
        // while (gameTitle.transform.localScale.x >= 1)
        // {
        //     gameTitle.transform.localScale -= new Vector3(step, step, step);
        //     if (count % 5 == 0)
        //     {
        //         GameObject go = Instantiate(gameTitle);
        //         go.transform.position = gameTitle.transform.position;
        //         go.transform.parent = gameTitle.transform.parent;
        //         listEchoTitles.Add(go);
        //     }
        //     count++;
        //     yield return null;
        // }
        gameTitle.transform.localScale = Vector3.one;
    }

    IEnumerator HideEchoTitles()
    {
        yield return Helpers.GetWait(1f);


        foreach (GameObject child in listEchoTitles)
        {
            Destroy(child);
            yield return Helpers.GetWait(0.25f);
            //child is your child transform
        }

        listEchoTitles.Clear();

        yield return null;

        // for (var i = 0; i <= gameTitle.transform.parent.childCount; i++)
        // {
        //     if (transform.GetChild(i).name == $"{gameTitle.name}(Clone)")
        //     {
        //         listEchoTitles.Add(transform.GetChild(i).gameObject);
        //     }
        // }

        // while (listEchoTitles.Count > 0)
        // {
        //     Destroy(listEchoTitles[0]);
        //     listEchoTitles.RemoveAt(0);

        //     yield return Helpers.GetWait(0.005f);
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
