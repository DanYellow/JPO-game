using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=yiTF4rJu6tY
public class MeteorEffectV2 : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float delayBeforeDeath;

    private Vector3 initScale;

    private void Awake() {
        initScale = transform.localScale;
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeDeath);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        print("wave " + other.transform.name);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 meshScale = transform.localScale;
        float growth = speed * Time.deltaTime;
        transform.localScale = new Vector3(
            meshScale.x + growth,
            meshScale.y + growth,
            meshScale.z + growth
        );
    }

    private void OnDisable() {
        transform.localScale = initScale;
    }
}
