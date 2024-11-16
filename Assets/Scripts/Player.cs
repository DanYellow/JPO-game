using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    public Canvas rankCanvas;

    private void Awake()
    {
        GetComponentInChildren<Light>().transform.name = $"Light{playerData.id.ToString()}";

        if (playerData.id == PlayerID.Player2 || playerData.id == PlayerID.Player3)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }

    private void Start()
    {
        StartCoroutine(UnparentLight());
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
