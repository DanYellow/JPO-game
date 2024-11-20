using System.Collections.Generic;
using UnityEngine;

public class PlayerIA : MonoBehaviour
{
    private List<(Vector3 Position, float RadiansAngle)> listAttackDirections;
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    private BoxCollider bc;

    private Vector3 startPos;

    private PlayerControls playerControls;

    private int factor = 1;
    RaycastHit hitInfo;
    bool hasACloseRangeAttack;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        playerControls = GetComponent<PlayerControls>();
        listAttackDirections = GetComponent<Player>().GetAttackDirections();

        startPos = new Vector3(
            transform.position.x,
            transform.position.y - (bc.bounds.size.y / 2),
            transform.position.z
        );

        if (playerData.id == PlayerID.Player2 || playerData.id == PlayerID.Player3)
        {
            factor = -1;
        }
    }
    private void FixedUpdate()
    {
        // if (playerData.id != PlayerID.Player4)
        // {
        //     return;
        // }

        float length = 4.85f;
        foreach (var direction in listAttackDirections)
        {
            // hasACloseRangeAttack = Physics.BoxCast(
            //     new Vector3(transform.position.x + bc.size.x * length / 2 * factor, bc.bounds.center.y / 4, bc.bounds.center.z),
            //     new Vector3(bc.size.x * length, bc.size.y / 4, bc.size.z),
            //     transform.forward.normalized,
            //     out hitInfo,
            //     Quaternion.Euler(0, 0, direction.RadiansAngle * Mathf.Rad2Deg),
            //     length,
            //     playerData.damageLayer
            // );

            // print(direction.RadiansAngle * Mathf.Rad2Deg);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.05f, playerData.damageLayer);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                playerControls.Jump();
                // Debug.Log(hitColliders[i].transform.name);
             }


            // if (Physics.SphereCast(transform.position, 2.05f, Vector3.zero, out RaycastHit hitInfo, 0, playerData.damageLayer))
            // {
            //     playerControls.Jump();
            //     Debug.Log(hitInfo.transform.name);
            // }

            // if (Physics.Linecast(startPos, direction.Position * length, out RaycastHit hitInfo, playerData.damageLayer))
            // {
            //     // playerControls.Jump();
            //     Debug.Log(hitInfo.transform.name);
            // }
        }
    }

    private void OnDrawGizmos()
    {
        // if (playerData.id != PlayerID.Player4)
        // {
        //     return;
        // }

        // bc = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.05f);
        // float length = 4.85f;
        // Vector3 startPos = new Vector3(
        //     transform.position.x,
        //     transform.position.y - (bc.bounds.size.y / 2),
        //     transform.position.z
        // );

        // foreach (var direction in listAttackDirections)
        // {
        //     Vector3 endPos = direction.Position * length;

        //     Debug.DrawLine(startPos, startPos + endPos, Color.red);
        // }

        // Gizmos.DrawWireCube(
        //     new Vector3(transform.position.x + (bc.size.x * length / 2), bc.bounds.center.y / 4, bc.bounds.center.z),
        //     new Vector3(bc.size.x * length, bc.size.y / 4, bc.size.z)
        // );
    }
}
