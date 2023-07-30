using System.Collections;
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

    public string name;
    private bool isRunning;

    public bool looping = false;

    public float cycleDuration
    {
        get
        {
            return spriteArray.Length * this.speed;
        }
    }

    public IEnumerator Play(System.Action callback)
    {
        if (looping)
        {
            isRunning = true;
            while (isRunning)
            {
                yield return new WaitForSeconds(speed);

                image.sprite = spriteArray[currentIndex];
                currentIndex = (currentIndex + 1) % spriteArray.Length;
            }
        }
        else
        {
            foreach (var item in spriteArray)
            {
                yield return new WaitForSeconds(speed);

                image.sprite = item;
            }
            yield return new WaitForSeconds(speed);

            callback();
        }
    }

    public void Stop()
    {
        isRunning = false;
    }
}

[RequireComponent(typeof(Image))]
public class UISpriteAnimationManager : MonoBehaviour
{
    private Image image;
    [SerializeField]

    private bool playOnStart = true;

    [SerializeField]
    private UISpriteAnimationItem[] uiSpriteAnimationItemArray;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (playOnStart)
        {
            var uiSpriteAnimationItem = uiSpriteAnimationItemArray
                .DefaultIfEmpty(null)
                .FirstOrDefault((item) => item.playOnStart == true);

            if (uiSpriteAnimationItem != null)
            {
                Play(uiSpriteAnimationItem.name);
            }
        }
    }

    public void Play(string animationName, System.Action callback = null)
    {
        var uiSpriteAnimationItem = uiSpriteAnimationItemArray
            .DefaultIfEmpty(null)
            .FirstOrDefault((item) => item.name == animationName);

        if (uiSpriteAnimationItem != null)
        {
            StopAllCoroutines();
            uiSpriteAnimationItem.image = image;
            StartCoroutine(uiSpriteAnimationItem.Play(callback ?? (() => {})));
        }
    }

    public void Stop(string animationName)
    {
        var uiSpriteAnimationItem = uiSpriteAnimationItemArray
            .DefaultIfEmpty(null)
            .FirstOrDefault((item) => item.name == animationName);

        if (uiSpriteAnimationItem != null)
        {
            uiSpriteAnimationItem.Stop();
        }
    }

    public float GetDurationForAnimation(string animationName)
    {
        var uiSpriteAnimationItem = uiSpriteAnimationItemArray
            .DefaultIfEmpty(null)
            .FirstOrDefault((item) => item.name == animationName);

        if (uiSpriteAnimationItem != null)
        {
            return uiSpriteAnimationItem.cycleDuration;
        }

        return 0;
    }
}
