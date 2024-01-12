using UnityEngine;

public class StunEffect : MonoBehaviour
{
    private Fade fade;

    private void OnEnable()
    {
        StartCoroutine(fade.Show());
    }
}