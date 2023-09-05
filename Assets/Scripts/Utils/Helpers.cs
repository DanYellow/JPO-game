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
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(layerName), 
                layerIndex, 
                enabled
            );
        }
    }

}
