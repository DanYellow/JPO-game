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
        GetComponentInChildren<Light>().transform.name = $"Light{playerData.id.ToString()}";
    }

    private void Start()
    {
        StartCoroutine(UnparentLight());

        if (playerData.id == PlayerID.Player2 || playerData.id == PlayerID.Player3)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private IEnumerator UnparentLight()
    {
        yield return null;
        GetComponentInChildren<Light>().transform.parent = null;
        rankCanvas = GetComponentInChildren<Canvas>();
        rankCanvas.transform.parent = null;
        rankCanvas.transform.name = $"RankCanvas{playerData.id.ToString()}";
        rankCanvas.gameObject.SetActive(false);
    }
}
