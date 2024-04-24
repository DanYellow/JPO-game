using UnityEngine;

public class Meteor : MonoBehaviour
{
    private ParticleSystem ps;

    [SerializeField]
    private GameObject effect;

    private void Awake() {
        ps = GetComponentInChildren<ParticleSystem>();
    }
    private void OnCollisionEnter(Collision other) {
        effect.SetActive(true);
        // ps.Play();
    }
}
