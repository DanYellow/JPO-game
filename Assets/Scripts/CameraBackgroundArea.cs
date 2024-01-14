using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundArea : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> listBackgrounds = new List<GameObject>();

    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        listBackgrounds.ForEach((item) =>
        {
            item.SetActive(false);
        });
    }

    private void Update()
    {
        // Vector3 nextPosition = new Vector3(player.transform.position.x, background.transform.position.y, background.transform.position.z);
        // background.transform.position = nextPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            listBackgrounds.ForEach((item) =>
            {
                item.SetActive(true);
            });
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
            if (iDamageable.GetHealth() > 0)
            {
                listBackgrounds.ForEach((item) =>
                {
                    item.SetActive(false);
                });
            }
        }
    }
}
