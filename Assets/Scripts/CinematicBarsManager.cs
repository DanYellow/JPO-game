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

        RectTransform barSize = topBar.GetComponent<RectTransform>();
        barSize.sizeDelta = new Vector2(Screen.width, 100);
        float xCenter = Screen.width / 2;

        topBar.position = new Vector3(xCenter, -barSize.sizeDelta.y, 0);
        bottomBar.position = new Vector3(xCenter, Screen.height + barSize.sizeDelta.y, 0);
        bottomBar.GetComponent<RectTransform>().sizeDelta = barSize.sizeDelta;
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

        Vector3 topBarStart = topBar.position;
        Vector3 bottomBarStart = bottomBar.position;

        RectTransform barSize = topBar.GetComponent<RectTransform>();

        Vector3 startBarEnd = topBarStart + ((show ? 1 : -1) * new Vector3(0, barSize.sizeDelta.y, 0));
        Vector3 bottomBarEnd = bottomBarStart - ((show ? 1 : -1) * new Vector3(0, barSize.sizeDelta.y, 0));

        yield return null;
        while (current <= 1)
        {
            topBar.position = Vector3.Lerp(topBarStart, startBarEnd, current);
            bottomBar.position = Vector3.Lerp(bottomBarStart, bottomBarEnd, current);

            current += Time.deltaTime / duration;

            yield return null;
        }
    }

    // Update is called once per frame
    void HideBars()
    {
        StartCoroutine(AnimateBars(false, 1.35f));
    }

    private void OnDisable()
    {
        onGameStart.OnEventRaised -= ShowBars;
        onGameCameraBlendFinished.OnEventRaised -= HideBars;
    }
}
