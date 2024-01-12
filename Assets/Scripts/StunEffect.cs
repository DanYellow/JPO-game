using UnityEngine;

public class StunEffect : MonoBehaviour
{
    private Fade fade;

    private void Awake()
    {
        fade = GetComponent<Fade>();
    }

    private void OnEnable()
    {
        StartCoroutine(fade.Show());
    }
}