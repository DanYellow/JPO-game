using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuUI;

    [SerializeField]
    private GameObject mainMenuLight;

    // [SerializeField]
    // private GameObject mainMenuLight;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onStartGame;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        mainMenuLight.SetActive(false);
        onStartGame.Raise();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
