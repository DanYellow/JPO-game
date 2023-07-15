using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    [SerializeField]
    private GameObject heartUI;

    public GameObject playerHUDUI;
    public List<GameObject> listHeartsUI = new List<GameObject>();

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        isHurtVoidEventChannel.OnEventRaised += UpdateLifePoints;
        GenerateHearts();
    }

    private void GenerateHearts() 
    {
        RectTransform rectHeartUI = heartUI.GetComponent<RectTransform>();
        float xOffset = 10;
        float startPosX = playerHUDUI.GetComponent<RectTransform>().rect.xMin + rectHeartUI.rect.width;

        for (int i = 0; i < playerStatsValue.nbCurrentLifes; i++)
        {
            Vector2 nextPos = new Vector2(
                (rectHeartUI.rect.width * i) + (xOffset * (i + 1)) + startPosX,
                480
            );
            GameObject heartLife = Instantiate(heartUI, nextPos, Quaternion.identity);
            heartLife.transform.SetParent(playerHUDUI.transform, false);
            listHeartsUI.Add(heartLife);
        }
    }

    private void UpdateLifePoints()
    {
        GameObject lastHeartLife = listHeartsUI[listHeartsUI.Count - 1];
        Animator animator = lastHeartLife.GetComponent<Animator>();
        animator.SetTrigger("IsHurt");
        Destroy(lastHeartLife, animator.GetCurrentAnimatorStateInfo(0).length);
        listHeartsUI.RemoveAt(listHeartsUI.Count - 1);
    }

    private void OnDisable()
    {
        isHurtVoidEventChannel.OnEventRaised -= UpdateLifePoints;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // UpdateLifePoints();
    }
}
