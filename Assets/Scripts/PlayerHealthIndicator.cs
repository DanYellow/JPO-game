using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerHealthIndicator : MonoBehaviour
{
    [SerializeField]
    private Material blackAndWhiteMaterial;

    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private Image playerImageShadow;

    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    private TextMeshProUGUI playerControls;

    [SerializeField]
    private Outline playerNameOutline;

    [SerializeField]
    private List<Image> listImages = new List<Image> { };

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private PlayerIDEventChannel onPlayerHitEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerDeathEvent;

    private void Awake()
    {
        string playerFullname = playerData.GetName() + (playerData.isCPU ? " - CPU" : "");
        playerName.SetText(playerFullname);

        playerImage.sprite = playerData.image;
        playerImageShadow.sprite = playerData.image;
        playerNameOutline.effectColor = playerData.color;

        string filledHeartColor = "#AA0000";

        foreach (var image in listImages)
        {
            if (ColorUtility.TryParseHtmlString(filledHeartColor, out Color hexColor))
            {
                image.color = hexColor;
            }
        }

        for (var i = 0; i < playerData.listSpritesCharactersKeysNames.Count(); i++)
        {
            string key = playerData.listSpritesCharactersKeysNames[i];
            playerControls.SetText(
                playerControls.text.Replace($"PLACEHOLDER_KEY_{i}", $"<sprite name=\"{key}\">")
            );
        }

    }

    private void Start() {
        Debug.Log(playerControls.transform.parent);
        LayoutRebuilder.ForceRebuildLayoutImmediate(playerControls.transform.parent.GetComponent<RectTransform>());
    }

    private void OnEnable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerHit(PlayerID playerID)
    {
        if (playerID == playerData.id && playerData.nbLives > 0 && listImages[playerData.nbLives] != null)
        {
            listImages[playerData.nbLives].color = Color.white;
        }
    }

    private void OnPlayerDeath(PlayerID playerID)
    {
        if (playerID == playerData.id && playerImage.material != blackAndWhiteMaterial)
        {
            if (playerImage != null)
            {
                playerImage.material = blackAndWhiteMaterial;
            }

            if (listImages[0] != null)
            {
                listImages[0].color = Color.white;
            }
        }
    }

    private void OnDisable()
    {
        onPlayerHitEvent.OnEventRaised -= OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
    }
}
