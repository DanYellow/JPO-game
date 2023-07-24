using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using UnityEngine.InputSystem;

public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director;

    [SerializeField]
    private VoidEventChannel onStart;

    [SerializeField]
    private VoidEventChannel onStartPlay;


    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;

        if (onStart != null)
        {
            onStart.OnEventRaised += StartTimeline;
        }
    }

    public void OnSkip(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            Debug.Log("ffffe");
            Skip();
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