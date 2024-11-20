using System.Collections.Generic;
using UnityEngine;

public class PlayerIA : MonoBehaviour
{
    private List<Vector3> listAttackDirections;
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    private BoxCollider bc;

    private Vector3 startPos;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        listAttackDirections = GetComponent<Player>().GetAttackDirections();

        startPos = new Vector3(
            transform.position.x,
            transform.position.y - (bc.bounds.size.y / 4),
            transform.position.z
        );
    }
    private void FixedUpdate()
    {
        foreach (var direction in listAttackDirections)
        {
            Vector3 endPos = direction * 2.5f;

            if (Physics.Linecast(startPos, endPos, out RaycastHit hitInfo, playerData.damageLayer))
            {
                Debug.Log(hitInfo.transform.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        bc = GetComponent<BoxCollider>();
        Vector3 startPos = new Vector3(
            transform.position.x,
            transform.position.y - (bc.bounds.size.y / 4),
            transform.position.z
        );

        foreach (var direction in listAttackDirections)
        {
            Vector3 endPos = direction * 2.5f;

            Debug.DrawLine(startPos, startPos + endPos, Color.red);
        }
    }
}
