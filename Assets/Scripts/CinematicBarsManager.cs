using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicBarsManager : MonoBehaviour
{
    [SerializeField]
    private Transform topBar;
    [SerializeField]
    private Transform bottomBar;


    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameStart;
    [SerializeField]
    private VoidEventChannel onGameCameraBlendFinished;

    private void Start()
    {
        bottomBar.parent.gameObject.SetActive(true);
        float xCenter = Screen.width / 2;

        RectTransform topBarSize = topBar.GetComponent<RectTransform>();
        topBarSize.sizeDelta = new Vector2(Screen.width, 100);
        topBarSize.anchoredPosition = new Vector2(xCenter, Screen.height + (topBarSize.sizeDelta.y / 2));

        RectTransform bottomBarSize = bottomBar.GetComponent<RectTransform>();
        bottomBarSize.sizeDelta = new Vector2(Screen.width, 100);
        bottomBarSize.anchoredPosition = new Vector2(xCenter, 0 +- (bottomBarSize.sizeDelta.y / 2));
        // topBar.position = new Vector3(xCenter, -barSize.sizeDelta.y, 0);
        // print(topBar.position);
        // bottomBar.position = new Vector3(xCenter, Screen.height + barSize.sizeDelta.y, 0);
        // bottomBar.GetComponent<RectTransform>().sizeDelta = barSize.sizeDelta;
    }

    private void OnEnable()
    {
        onGameStart.OnEventRaised += ShowBars;
        onGameCameraBlendFinished.OnEventRaised += HideBars;
    }

    void ShowBars()
    {
        StartCoroutine(AnimateBars(true, 0.05f));
    }

    private IEnumerator AnimateBars(bool show, float duration)
    {
        float current = 0;
        
        RectTransform topBarSize = topBar.GetComponent<RectTransform>();
        // topBarSize.anchoredPosition = new Vector2(xCenter, Screen.height + (topBarSize.sizeDelta.y / 2));

        Vector3 topBarStart = topBarSize.anchoredPosition;
        Vector3 bottomBarStart = bottomBar.position;


        RectTransform barSize = topBar.GetComponent<RectTransform>();

        Vector3 startBarEnd = topBarStart + ((show ? 1 : -1) * new Vector3(0, barSize.sizeDelta.y, 0));
        // Vector3 bottomBarEnd = bottomBarStart - ((show ? 1 : -1) * new Vector3(0, barSize.sizeDelta.y, 0));

        yield return null;
        while (current <= 1)
        {
            topBarSize.anchoredPosition = Vector3.Lerp(topBarStart, startBarEnd, current);
            // bottomBar.position = Vector3.Lerp(bottomBarStart, bottomBarEnd, current);

            current += Time.deltaTime / duration;

            yield return null;
        }
    }

    // Update is called once per frame
    void HideBars()
    {
        // StartCoroutine(AnimateBars(false, 1.35f));
    }

    private void OnDisable()
    {
        onGameStart.OnEventRaised -= ShowBars;
        onGameCameraBlendFinished.OnEventRaised -= HideBars;
    }
}
