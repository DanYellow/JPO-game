using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Canvas rankCanvas;

    [Header("Scriptable Objects"), SerializeField]
    public PlayerData playerData;

    [SerializeField]
    private VoidEventChannel onPlayerReadyEvent;

    private void Awake()
    {
        playerData.gameObject = gameObject;
        GetComponentInChildren<Light>().transform.name = $"Light{playerData.id}";

        rankCanvas = GetComponentInChildren<Canvas>();
        rankCanvas.transform.name = $"RankCanvas{playerData.id}";
        rankCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (playerData.id == PlayerID.Player2 || playerData.id == PlayerID.Player3)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public List<(Vector3 Position, float GradeAngle)> GetAttackDirections()
    {
        int nbColliders = 4;
        List<(Vector3, float)> listDirections = new List<(Vector3, float)>();

        for (int i = 0; i <= nbColliders; i += 2)
        {
            float val = Mathf.Lerp(0, -Mathf.PI / 2, (float)i / nbColliders);

            switch (playerData.id)
            {
                case PlayerID.Player2:
                    val = Mathf.Lerp(Mathf.PI, 3 * Mathf.PI / 2, (float)i / nbColliders);
                    break;
                case PlayerID.Player3:
                    val = Mathf.Lerp(Mathf.PI / 2, Mathf.PI, (float)i / nbColliders);
                    break;
                case PlayerID.Player4:
                    val = Mathf.Lerp(0, Mathf.PI / 2, (float)i / nbColliders);
                    break;
                default:
                    break;
            }

            var vertical = Mathf.Sin(val);
            var horizontal = Mathf.Cos(val);

            var spawnDir = new Vector3(horizontal, 0, vertical);
            listDirections.Add((spawnDir, val));
        }

        return listDirections;
    }

    public void UnparentChildren()
    {
        StartCoroutine(UnparentChildrenCoroutine());
    }

    private IEnumerator UnparentChildrenCoroutine()
    {
        yield return Helpers.GetWait(0.75f);
        GetComponentInChildren<Light>().transform.parent = null;
        rankCanvas.transform.parent = null;
        onPlayerReadyEvent.Raise();
    }
}
