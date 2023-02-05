using System.Collections;
using UnityEngine;

public class OscillationMovement : MonoBehaviour
{
    bool AmActive = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && AmActive)
        {
            AmActive = false;
            StartCoroutine(Move());
        }

         Vector3 pos = transform.localPosition;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * 1.75f) + pos.y;
        if (gameObject.name == "forearm-front")
            Debug.Log("pos" + pos.y);

        //set the object's Y to the new calculated Y
        transform.localPosition = new Vector3(pos.x, newY * 0.03f, pos.z);
    }
    
    public IEnumerator Move(float height = 0.03f, float speed = 1.75f)
    {
        while (true)
        {
            Vector3 pos = transform.localPosition;
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * speed) + pos.y;
            if (gameObject.name == "forearm-front")
                Debug.Log("pos" + pos.y);

            //set the object's Y to the new calculated Y
            transform.localPosition = new Vector3(pos.x, newY * height, pos.z);
            yield return null;
        }
    }
}
