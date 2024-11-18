using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    public PlayerData playerData;

    public Canvas rankCanvas;

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

    public void UnparentChildren()
    {
        StartCoroutine(UnparentChildrenCoroutine());
    }

    private IEnumerator UnparentChildrenCoroutine() {
        yield return null;
        GetComponentInChildren<Light>().transform.parent = null;
        rankCanvas.transform.parent = null;
    }
}
