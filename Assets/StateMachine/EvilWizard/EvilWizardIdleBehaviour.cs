using UnityEngine.Rendering.Universal;
using UnityEngine;

public class EvilWizardIdleBehaviour : StateMachineBehaviour
{
    private Light2D stickLight;
    private float defaultLightIntensity;

    private EvilWizard evilWizard;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        evilWizard = animator.GetComponent<EvilWizard>();

        stickLight = animator.GetComponentInChildren<Light2D>();
        stickLight.enabled = true;
        defaultLightIntensity = stickLight.intensity;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // stickLight.intensity = Mathf.Lerp(defaultLightIntensity, 3.5f, Mathf.PingPong(Time.time / 2, 1));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stickLight.intensity = defaultLightIntensity;
        stickLight.enabled = false;
        evilWizard.StopAllCoroutines();
    }
}
