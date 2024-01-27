using UnityEngine;
using System.Collections.Generic;

public enum Corner
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

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

    public static Vector3 GetBoxCollider2DCorners(GameObject go, Corner corner)
    {
        BoxCollider2D collider = go.GetComponent<BoxCollider2D>();

        Vector2 size = collider.size;

        Vector3 worldPos = go.transform.TransformPoint(collider.offset);

        float top = worldPos.y + (size.y / 2f);
        float btm = worldPos.y - (size.y / 2f);
        float left = worldPos.x - (size.x / 2f);
        float right = worldPos.x + (size.x / 2f);

        if(corner == Corner.TopLeft) {
            return new Vector3(left, top, worldPos.z);
        }

        if(corner == Corner.TopRight) {
            return new Vector3(left, top, worldPos.z);
        }

        if(corner == Corner.BottomLeft) {
            return new Vector3( left, btm, worldPos.z);
        }

        if(corner == Corner.BottomLeft) {
            return new Vector3(right, btm, worldPos.z);
        }

        return new Vector3(left, top, worldPos.z);
    }
}
