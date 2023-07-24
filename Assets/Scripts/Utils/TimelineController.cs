using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director;

    [SerializeField]
    private VoidEventChannel onStart;

    [SerializeField]
    private VoidEventChannel onStartPlay;

    [SerializeField]
    private Image skipProgress;

    private float holdSkipBtnDuration = 0.7f;


    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;

        skipProgress.fillAmount = 0;

        if (onStart != null)
        {
            onStart.OnEventRaised += StartTimeline;
        }
    }

    public void OnSkip(InputAction.CallbackContext ctx)
    {
        StopAllCoroutines();
        if (ctx.phase == InputActionPhase.Performed)
        {
            StartCoroutine(FillProgressBar());
        } else if (ctx.phase == InputActionPhase.Canceled) {
            StartCoroutine(ClearProgressBar());
        }
    }

    IEnumerator FillProgressBar()
    {
        float currentTime = 0;
        while (skipProgress.fillAmount < 1)
        {
            currentTime += Time.deltaTime;
            skipProgress.fillAmount = currentTime / holdSkipBtnDuration;

            yield return null;
        }

        Skip();
    }

    IEnumerator ClearProgressBar() {
        float currentTime = skipProgress.fillAmount / 1;
        while (skipProgress.fillAmount > 0)
        {
            currentTime -= Time.deltaTime;
            skipProgress.fillAmount = currentTime / holdSkipBtnDuration;

            yield return null;
        }
    }

    private void Skip()
    {
        var timelineAsset = director.playableAsset as TimelineAsset;
        var markers = timelineAsset.markerTrack.GetMarkers().ToArray();

        director.time = markers.First().time;
    }

    private void Director_Stopped(PlayableDirector obj)
    {
    }

    private void Director_Played(PlayableDirector obj)
    {
        if (onStartPlay != null)
        {
            onStartPlay.Raise();
        }
    }

    public void StartTimeline()
    {
        director.Play();
    }

    private void OnDisable()
    {
        if (onStart != null)
        {
            onStart.OnEventRaised -= StartTimeline;
        }
    }
}