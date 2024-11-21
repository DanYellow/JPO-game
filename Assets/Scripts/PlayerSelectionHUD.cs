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

    private PlayerInput playerInput;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private VoidEventChannel onPlayerInputReadyEvent;

    private void Awake()
    {
        playerName.SetText($"{playerData.GetName()} - CPU");
        playerImage.sprite = playerData.image;

        readyToPlay.SetText("<b>Appuyer sur x\npour rejoindre la partie</b>");

        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultActionMap = "Game";

        playerData.isCPU = true;
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
