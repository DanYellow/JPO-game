using System.Collections;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{

    public bool needsToActivateShield = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        // yield return Helpers.GetWait(1.75f);
        while (true)
        {
            yield return Helpers.GetWait(2.75f);

            needsToActivateShield = Random.value < 0.35f;
        }
    }
}
