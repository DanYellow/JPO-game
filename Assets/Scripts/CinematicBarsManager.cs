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
        Vector3 topBarStart = topBar.position;
        Vector3 bottomBarStart = bottomBar.position;

        Vector3 startBarEnd = topBarStart + new Vector3(0, 40, 0);
        Vector3 bottomBarEnd = bottomBarStart - new Vector3(0, 40, 0);

        topBar.position = startBarEnd;
        bottomBar.position = bottomBarEnd;
    }

    private void OnEnable()
    {
        onGameStart.OnEventRaised += ShowBars;
        onGameCameraBlendFinished.OnEventRaised += HideBars;
    }
    // Start is called before the first frame update
    void ShowBars()
    {
        StartCoroutine(AnimateBars(true, 0.25f));
    }

    private IEnumerator AnimateBars(bool show, float duration)
    {
        float current = 0;

        Vector3 topBarStart = topBar.position;
        Vector3 bottomBarStart = bottomBar.position;

        Vector3 startBarEnd = topBarStart + ((show ? -1 : 1) * new Vector3(0, 40, 0));
        Vector3 bottomBarEnd = bottomBarStart - ((show ? -1 : 1) * new Vector3(0, 40, 0));

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
