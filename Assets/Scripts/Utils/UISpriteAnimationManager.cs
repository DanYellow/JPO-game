using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UISpriteAnimationItem
{
    [HideInInspector]
    public Image image;
    [SerializeField]
    private Sprite[] spriteArray;

    [SerializeField, Range(0, 5f)]
    private float speed = .02f;

    public bool playOnStart = true;

    [HideInInspector]
    public int currentIndex = 0;

    public new string name;

    // private Coroutine animCoroutine;
    private bool IsDone;

    private float Duration;
    public float duration
    {
        get
        {
            return spriteArray.Length * this.speed;
        }
        // set
        // {
        //     this.Duration = value;
        // }
    }

    // public void Play()
    // {
    //     IsDone = false;
    //     animCoroutine = StartCoroutine(PlayCo());
    // }

    // IEnumerator PlayCo()
    // {
    //     yield return new WaitForSeconds(speed);

    //     image.sprite = spriteArray[currentIndex];
    //     currentIndex = (currentIndex + 1) % spriteArray.Length;

    //     if (IsDone == false)
    //         animCoroutine = StartCoroutine(PlayCo());
    // }

    // public void Stop()
    // {
    //     IsDone = true;
    //     StopCoroutine(animCoroutine);
    // }
}

[RequireComponent(typeof(Image))]
public class UISpriteAnimationManager : MonoBehaviour
{
    private Image image;
    [SerializeField]

    private bool playOnStart = true;

    [SerializeField]
    private List<UISpriteAnimationItem> uiSpriteAnimationItemArray;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (playOnStart)
        {
            var uiSpriteAnimationItem = uiSpriteAnimationItemArray.First((item) => item.playOnStart == true);
            if (uiSpriteAnimationItem != null)
            {
                Play(uiSpriteAnimationItem.name);
            }
        }
    }

    [ContextMenu("Play")]
    public void Play(string animationName)
    {
        var uiSpriteAnimationItem = uiSpriteAnimationItemArray.First((item) => item.name == animationName);
        if (uiSpriteAnimationItem != null)
        {
            uiSpriteAnimationItem.image = image;
            // uiSpriteAnimationItem.Play();
        }
    }
    public void Stop(string animationName)
    {
        var uiSpriteAnimationItem = uiSpriteAnimationItemArray.First((item) => item.name == animationName);
        if (uiSpriteAnimationItem != null)
        {
            // uiSpriteAnimationItem.Play();
        }
    }
}
