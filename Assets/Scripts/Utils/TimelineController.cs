using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;


public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director;

    [SerializeField]
    private VoidEventChannel onStart;

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

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            var timelineAsset = director.playableAsset as TimelineAsset;
            var markers = timelineAsset.markerTrack.GetMarkers().ToArray();

            director.time = markers.First().time;
        }
        #endif
    }


    private void Director_Stopped(PlayableDirector obj)
    {
    }

    private void Director_Played(PlayableDirector obj)
    {
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