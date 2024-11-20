using System.Collections.Generic;
using UnityEngine;

public class PlayerIA : MonoBehaviour
{
    private List<Vector3> listAttackDirections;
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    private BoxCollider bc;

    private Vector3 startPos;

    private PlayerControls playerControls;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        playerControls = GetComponent<PlayerControls>();
        listAttackDirections = GetComponent<Player>().GetAttackDirections();

        startPos = new Vector3(
            transform.position.x,
            transform.position.y - (bc.bounds.size.y / 4),
            transform.position.z
        );
    }
    private void FixedUpdate()
    {
        if(playerData.id != PlayerID.Player1) {
            return;
        }
        foreach (var direction in listAttackDirections)
        {
            Vector3 endPos = direction * 1.85f;

            if (Physics.Linecast(startPos, endPos, out RaycastHit hitInfo, playerData.damageLayer))
            {
                playerControls.Jump();
                // Debug.Log(hitInfo.transform.name);
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
            Vector3 endPos = direction * 1.85f;

            Debug.DrawLine(startPos, startPos + endPos, Color.red);
        }
    }
}
