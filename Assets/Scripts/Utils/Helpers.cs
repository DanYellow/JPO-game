using UnityEngine;
using System.Collections.Generic;

public static class Helpers
{
   private static readonly Dictionary<float, WaitForSeconds> waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();

   public static WaitForSeconds GetWait(float time) {
    if (waitForSecondsDictionary.TryGetValue(time, out var wait)) {
        return wait;
    }

    waitForSecondsDictionary[time] = new WaitForSeconds(time);

    return waitForSecondsDictionary[time];
   }

}
