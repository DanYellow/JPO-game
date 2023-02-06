using System.Collections;
using UnityEngine;

public class OscillationMovement : MonoBehaviour
{
    public IEnumerator Move(float height = 0.03f, float speed = 1.75f)
    {
        while (gameObject != null)
        {
            Vector3 pos = transform.localPosition;
            float newY = Mathf.Sin(Time.time * speed) + pos.y;
            transform.localPosition = new Vector3(pos.x, newY * height, pos.z);
            yield return null;
        }
    }
}
