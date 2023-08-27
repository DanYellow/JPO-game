using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    [SerializeField]
    private GameObject heartUI;

    public GameObject playerHUDUI;
    private List<GameObject> listHeartsUI = new List<GameObject>();

    [SerializeField]
    private Image timeBar;

    [SerializeField]
    private FloatValue timeBarValue;

    public float decreaseTimeBarStep = 0.005f;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;
    private UnityAction<bool> onPause;
    private bool isGamePaused = false;

    private GameObject barContainer;

    private void Awake()
    {
        playerHUDUI.SetActive(true);
        barContainer = playerHUDUI.transform.Find("BarContainer").gameObject;
        barContainer.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        isHurtVoidEventChannel.OnEventRaised += HeartLost;

        onPause = (bool isPaused) =>
        {
            isGamePaused = isPaused;
        };
        onTogglePauseEvent.OnEventRaised += onPause;
    }

    public void StartGame()
    {
        timeBar.fillAmount = timeBarValue.CurrentValue;
        // playerStatsValue.nbCurrentLifes = playerStatsValue.nbMaxLifes;
        barContainer.SetActive(true);
        FillHearts();
        StartCoroutine(DecreaseTimeBar());
    }

    private void FillHearts()
    {
        // RectTransform rectHeartUI = heartUI.GetComponent<RectTransform>();
        // float xOffset = 10;
        // float startPosX = playerHUDUI.GetComponent<RectTransform>().rect.xMin + rectHeartUI.rect.width;

        // for (int i = 0; i < playerStatsValue.nbCurrentLifes; i++)
        // {
        //     Vector2 nextPos = new Vector2(
        //         (rectHeartUI.rect.width * i) + (xOffset * (i + 1)) + startPosX,
        //         480
        //     );
        //     GameObject heartLife = Instantiate(heartUI, nextPos, Quaternion.identity);
        //     heartLife.transform.SetParent(playerHUDUI.transform, false);
        //     listHeartsUI.Add(heartLife);
        // }
        // AnimateLastHeart();
    }

    private void HeartLost()
    {
        if (listHeartsUI.Count > 0)
        {
            GameObject lastHeartLife = listHeartsUI[listHeartsUI.Count - 1];
            UISpriteAnimationManager uiAnimator = lastHeartLife.GetComponent<UISpriteAnimationManager>();
            uiAnimator.Play("destroy", () => {
                Destroy(lastHeartLife);
            });
            listHeartsUI.RemoveAt(listHeartsUI.Count - 1);

            AnimateLastHeart();
        }
    }

    private void AnimateLastHeart()
    {
        if (listHeartsUI.Count == 0) return;

        GameObject lastHeartLife = listHeartsUI[listHeartsUI.Count - 1];
        lastHeartLife = listHeartsUI[listHeartsUI.Count - 1];
        UISpriteAnimationManager uiAnimator = lastHeartLife.GetComponent<UISpriteAnimationManager>();
        uiAnimator.Play("lastItem");
    }

    IEnumerator DecreaseTimeBar()
    {
        // while (timeBar.fillAmount > 0 && playerStatsValue.nbCurrentLifes > 0)
        // {
        //     if (!isGamePaused)
        //     {
        //         timeBarValue.CurrentValue -= decreaseTimeBarStep;
        //         timeBar.fillAmount = timeBarValue.CurrentValue;
        //     }
            yield return null;
        // }
        // StopAllCoroutines();
        // onPlayerDeathVoidEventChannel.Raise();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // FillHearts();
    }

    private void OnDisable()
    {
        isHurtVoidEventChannel.OnEventRaised -= HeartLost;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        onTogglePauseEvent.OnEventRaised -= onPause;
    }

    private void OnValidate()
    {
        // FillHearts();
    }

}
