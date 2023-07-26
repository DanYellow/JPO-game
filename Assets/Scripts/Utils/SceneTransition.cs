using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField]
    private UnityEvent onShowStart;

    [SerializeField]
    private UnityEvent onShowEnd;

    [SerializeField]
    private UnityEvent onHideStart;

    [SerializeField]
    private UnityEvent onHideEnd;

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Hide());
        }
        #endif
    }

    public IEnumerator Show()
    {
        onShowStart.Invoke();
        sceneTransitionMaterial.SetFloat(propertyName, minValue);

        yield return new WaitForSeconds(0.15f);
        float currentTime = 0f;
        while (sceneTransitionMaterial.GetFloat(propertyName) < maxValue)
        {
            currentTime += Time.deltaTime;
            sceneTransitionMaterial.SetFloat(propertyName, Mathf.Clamp(currentTime / transitionTime, minValue, maxValue));
            yield return null;
        }
        onShowEnd.Invoke();
    }

    public IEnumerator Hide()
    {
        onHideStart.Invoke();
        sceneTransitionMaterial.SetFloat(propertyName, maxValue);
        yield return new WaitForSeconds(0.15f);

        float currentTime = transitionTime;
        while (sceneTransitionMaterial.GetFloat(propertyName) > minValue)
        {
            currentTime -= Time.deltaTime;
            sceneTransitionMaterial.SetFloat(propertyName, Mathf.Clamp(currentTime / transitionTime, minValue, maxValue));
            yield return null;
        }

        onHideEnd.Invoke();
        // callback();
    }
}
