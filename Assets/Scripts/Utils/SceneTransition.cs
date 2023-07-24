using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// https://www.youtube.com/watch?v=WvvvzupH18s
// https://www.youtube.com/watch?v=WoNjob5E7Vw

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private Material sceneTransitionMaterial;
    [SerializeField]
    private float transitionTime = 1f;
    [SerializeField]
    private string propertyName = "_Cutoff";

    [SerializeField]
    private float minValue = -0.5f;
    [SerializeField]
    private float maxValue = 1.1f;

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Hide(() => {}));
        }
        #endif
    }

    public IEnumerator Show(System.Action callback)
    {
        sceneTransitionMaterial.SetFloat(propertyName, minValue);

        yield return new WaitForSeconds(0.5f);
        float currentTime = 0f;
        while (sceneTransitionMaterial.GetFloat(propertyName) <= maxValue)
        {
            sceneTransitionMaterial.SetFloat(
                propertyName,
                Mathf.MoveTowards(sceneTransitionMaterial.GetFloat(propertyName), maxValue, transitionTime * Time.deltaTime)
            );
            // currentTime += Time.deltaTime;
            // sceneTransitionMaterial.SetFloat(propertyName, Mathf.Clamp(currentTime / transitionTime, minValue, maxValue));
            yield return null;
        }

        callback();
    }

    public IEnumerator Hide(System.Action callback)
    {
        sceneTransitionMaterial.SetFloat(propertyName, maxValue);
        yield return new WaitForSeconds(0.5f);

        float currentTime = 1f;
        while (currentTime >= -0.75f)
        {
            currentTime -= Time.deltaTime;
            sceneTransitionMaterial.SetFloat(propertyName, Mathf.Clamp(currentTime / transitionTime, minValue, maxValue));
            yield return null;
        }

        callback();
    }
}
