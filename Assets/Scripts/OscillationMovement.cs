using System.Collections;
using UnityEngine;

public class OscillationMovement : MonoBehaviour
{
    bool AmActive = true;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.N) && AmActive) {
            AmActive = false;
            StartCoroutine(Move());
        }
    }
    public IEnumerator Move(float height = 0.05f, float speed = 1.25f)
    {
        float timeOffset = 0; //Random.Range(0.01f, 0.03f) * (Mathf.PI / 2);
        
        while (true)
        {
            Vector3 pos = transform.localPosition;
            Debug.Log(pos);
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * speed) + pos.y;
            //set the object's Y to the new calculated Y
            transform.localPosition = new Vector3(pos.x, newY * height, pos.z);
            yield return null;
        }
    }
}
