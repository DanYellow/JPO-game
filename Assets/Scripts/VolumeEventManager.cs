using System.Collections;

using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class VolumeEventManager : MonoBehaviour
{

    private Volume volume;
    [SerializeField]
    private VoidEventChannel onPlayerHurtEvent;
    private UnityAction onPlayerHurt;

    private void Awake() {
        volume = GetComponent<Volume>();
        volume.enabled = false;

        onPlayerHurt = () => {
            StopAllCoroutines();
            StartCoroutine(Hurt());
        };
    }


    private void OnEnable() {
        

        onPlayerHurtEvent.OnEventRaised += onPlayerHurt;
    }

    IEnumerator Hurt() {
        volume.enabled = true;
        yield return new WaitForSeconds(0.25f);
        volume.enabled = false;
    }

    private void OnDisable() {
        onPlayerHurtEvent.OnEventRaised -= onPlayerHurt;
    }
}
