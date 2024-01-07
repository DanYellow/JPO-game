using System.Collections;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{
    private IEnumerator checkShieldGenerationCo;
    private bool isCheckingShieldGeneration = false;

    public bool needsToActivateShield = false;
    // Start is called before the first frame update
    void Start()
    {
        checkShieldGenerationCo = CheckShieldGeneration();
    }

    private IEnumerator CheckShieldGeneration()
    {
        yield return Helpers.GetWait(1.75f);
        while (true)
        {
            // yield return Helpers.GetWait(1f); // For tests
            yield return Helpers.GetWait(4.15f);
            bool randVal = Random.value < 0.35f;
            needsToActivateShield = randVal;
        }
    }

    public void StartShieldGenerationChecking() {
        if(isCheckingShieldGeneration) return;
        StartCoroutine(checkShieldGenerationCo);
        isCheckingShieldGeneration = true;
    } 

    public void StopShieldGenerationChecking() {
        StopCoroutine(checkShieldGenerationCo);
        isCheckingShieldGeneration = false;
        needsToActivateShield = false;
    } 
}
