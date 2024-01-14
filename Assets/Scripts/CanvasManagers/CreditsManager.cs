using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    public VoidEventChannel onShowCredits;

    [SerializeField]
    private GameObject creditsUI;

    [SerializeField]
    private StringEventChannel onPlayerInputMapChange;

    [SerializeField]
    private Vector2Value lastCheckpoint;

    private void Awake()
    {
        creditsUI.SetActive(false);
    }

    private void ShowCredits() {
        lastCheckpoint.CurrentValue = new Vector2(-15.02f, -3.43f);
        creditsUI.SetActive(true);
        onPlayerInputMapChange.Raise(ActionMapName.UIGameOverAndCredits);
        EventSystemExtensions.UpdateSelectedGameObject(creditsUI.GetComponentInChildren<Button>().gameObject);
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (creditsUI != null && creditsUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            creditsUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && creditsUI.activeInHierarchy)
        {
            creditsUI.GetComponentInChildren<Button>().Select();
        }
    }

    private void OnEnable() {
        onShowCredits.OnEventRaised += ShowCredits;
    }

    private void OnDisable() {
        onShowCredits.OnEventRaised -= ShowCredits;
    }
}
