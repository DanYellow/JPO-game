using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaveButton : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private UnityEvent<Transform> onPositionSet;

    [SerializeField]
    private Transform playerPosition;

    [SerializeField]
    private GameObject pushButton;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private void Start()
    {
        transform.position = new Vector3(
            startPosition.position.x,
            startPosition.position.y,
            startPosition.position.z
        );

        onPositionSet.Invoke(playerPosition);
    }

    public void OnGroundPound(GameObject player)
    {
        StartCoroutine(MoveObject(player));
    }

    IEnumerator MoveObject(GameObject player)
    {
        float timeElapsed = 0;

        Vector3 startPos = pushButton.transform.position;
        Vector3 endPos = pushButton.transform.position - (Vector3.up * 0.1f);

        // while (timeElapsed < duration)
        // {
        //     timeElapsed += Time.deltaTime;
        //     pushButton.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed);

        //     yield return null;
        // }

        pushButton.transform.position = endPos;
        timeElapsed = 0;
        while (timeElapsed < playerData.root.groundPoundCooldown)
        {
            timeElapsed += Time.deltaTime;
            pushButton.transform.position = Vector3.Lerp(endPos, startPos, timeElapsed);

            yield return null;
        }
        pushButton.transform.position = startPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.transform.parent = pushButton.transform;
    }

    private void OnCollisionExit(Collision other)
    {
        other.transform.parent = null;
    }
}
