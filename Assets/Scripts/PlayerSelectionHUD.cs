using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelectionHUD : MonoBehaviour
{
    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    private TextMeshProUGUI readyToPlay;

    [SerializeField]
    private TextMeshProUGUI controls;

    private PlayerInput playerInput;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private VoidEventChannel onPlayerInputReadyEvent;

    private void Awake()
    {
        playerName.SetText($"{playerData.GetName()} - CPU");
        playerImage.sprite = playerData.image;

        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultActionMap = "Game";

        playerData.isCPU = true;

        for (var i = 0; i < playerData.listSpritesCharactersKeysNames.Count(); i++)
        {
            string key = playerData.listSpritesCharactersKeysNames[i];
            controls.SetText(
                controls.text.Replace($"PLACEHOLDER_KEY_{i}", $"<sprite name=\"{key}\">")
            );
            readyToPlay.SetText(
                readyToPlay.text.Replace($"PLACEHOLDER_KEY_{i}", $"<sprite name=\"{key}\">")
            );
        }
    }

    public void OnActivatePlayer(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            playerName.SetText(playerData.GetName());
            readyToPlay.SetText("<color=#00a100><b>OK !</b></color>");
            readyToPlay.fontSize = 28;
            playerData.isCPU = false;
            onPlayerInputReadyEvent.Raise();
        }
    }
}
