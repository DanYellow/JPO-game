using System.Collections;
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

    public void UnparentChildren()
    {
        StartCoroutine(UnparentChildrenCoroutine());
    }

    private IEnumerator UnparentChildrenCoroutine() {
        yield return Helpers.GetWait(0.75f);
        GetComponentInChildren<Light>().transform.parent = null;
        rankCanvas.transform.parent = null;
        onPlayerReadyEvent.Raise();
    }
}
