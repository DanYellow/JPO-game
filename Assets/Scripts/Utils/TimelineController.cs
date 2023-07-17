using UnityEngine;
using UnityEngine.Playables;

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

        onStart.OnEventRaised += StartTimeline;
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
        onStart.OnEventRaised -= StartTimeline;
    }
}