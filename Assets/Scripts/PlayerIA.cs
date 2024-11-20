using System.Collections.Generic;
using UnityEngine;

public class PlayerIA : MonoBehaviour
{
    private List<Vector3> listAttackDirections;
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    private BoxCollider bc;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        listAttackDirections = GetComponent<Player>().GetAttackDirections();
    }
    private void FixedUpdate()
    {
        // if (Physics.Linecast(meshRenderer.sharedMesh.bounds.min, meshRenderer.sharedMesh.bounds.min + Vector3.right * 2, out RaycastHit hitInfo, playerData.damageLayer))
        // {
        //     Debug.Log(hitInfo.transform.name);
        // }
    }

    private void OnDrawGizmos()
    {

        //  Vector2 startCast = new Vector2(sr.bounds.center.x + (transform.right.normalized.x * (sr.bounds.size.x / 2)), sr.bounds.center.y);
        // Vector3 startPos = new Vector3(
        //     (transform.position).x + (bc.bounds.size.x / 2),
        //     transform.position.y - (bc.bounds.size.y / 3),
        //     (transform.position).z
        // );
        // Debug.DrawLine(startPos, startPos + transform.right.normalized * 5, Color.red);

        foreach (var direction in listAttackDirections)
        {
            var startPos = new Vector3(
                transform.position.x, 
                transform.position.y - (bc.bounds.size.y / 4),
                transform.position.z
            );

            var endPos = direction * 2;

            Debug.DrawLine(startPos, startPos + endPos, Color.red);
        }

        // for (int i = 0; i <= nbColliders; i += 2)
        // {
        //     float val = Mathf.Lerp(0, -Mathf.PI / 2, (float)i / nbColliders);

        //     switch (playerData.id)
        //     {
        //         case PlayerID.Player2:
        //             val = Mathf.Lerp(Mathf.PI, 3 * Mathf.PI / 2, (float)i / nbColliders);
        //             break;
        //         case PlayerID.Player3:
        //             val = Mathf.Lerp(Mathf.PI / 2, Mathf.PI, (float)i / nbColliders);
        //             break;
        //         case PlayerID.Player4:
        //             val = Mathf.Lerp(0, Mathf.PI / 2, (float)i / nbColliders);
        //             break;
        //         default:
        //             break;
        //     }

        //     var vertical = Mathf.Sin(val);
        //     var horizontal = Mathf.Cos(val);

        //     var startPos = new Vector3(
        //         transform.position.x, 
        //         transform.position.y - (bc.bounds.size.y / 4),
        //         transform.position.z
        //     );

        //     var endPos = new Vector3(horizontal, 0, vertical) * 2;
        //     Debug.DrawLine(startPos, startPos + endPos, Color.red);
        // }
    }
}
