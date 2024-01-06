using System.Collections;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{
    private IEnumerator checkShieldGenerationCo;

    public bool needsToActivateShield = false;
    // Start is called before the first frame update
    void Start()
    {
        checkShieldGenerationCo = CheckShieldGeneration();
    }

    private IEnumerator CheckShieldGeneration()
    {
        // yield return Helpers.GetWait(1.75f);
        while (true)
        {
            yield return Helpers.GetWait(2.75f);

            needsToActivateShield = Random.value < 0.35f;
        }
    }

    public void StartShieldGenerationChecking() {
        StartCoroutine(checkShieldGenerationCo);
    } 

    public void StopShieldGenerationChecking() {
        StopCoroutine(checkShieldGenerationCo);
    } 
}
