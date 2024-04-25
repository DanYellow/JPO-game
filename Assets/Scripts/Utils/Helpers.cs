using UnityEngine;
using System.Collections.Generic;

public static class Helpers
{
    private static readonly Dictionary<float, WaitForSeconds> waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (waitForSecondsDictionary.TryGetValue(time, out var wait))
        {
            return wait;
        }

        waitForSecondsDictionary[time] = new WaitForSeconds(time);

        return waitForSecondsDictionary[time];
    }

    public static IEnumerable<float> LinearDistribution(float min, float max, int count)
    {
        if (count <= 1)
        {
            yield return Mathf.Lerp(min, max, 0.5f);
            yield break;
        }

        float delta = 1f / (count - 1);
        for (int i = 0; i < count; i++)
        {
            yield return Mathf.Lerp(min, max, delta * (count - 1));
        }
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}