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

    public static List<int> GetLayersIndexFromLayerMask(LayerMask layerMask)
    {
        List<int> listLayers = new List<int>();

        for (int i = 0; i < 32; i++)
        {
            if (layerMask == (layerMask | (1 << i)))
            {
                listLayers.Add(i);
            }
        }

        return listLayers;
    }

    public static void DisableCollisions(string layerName, List<int> layersToIgnore, bool enabled)
    {
        foreach (var layerIndex in layersToIgnore)
        {
            Physics.IgnoreLayerCollision(
                LayerMask.NameToLayer(layerName),
                layerIndex,
                enabled
            );
        }
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

    public static int GetLayerIndex(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }

    public static string GetLayerName(int layerNumber)
    {
        return LayerMask.LayerToName(GetLayerIndex(layerNumber));
    }
}