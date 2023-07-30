using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISpriteAnimation : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private Sprite[] spriteArray;
    [SerializeField, Range(0, 5f)]
    private float speed = .02f;
    private Coroutine animCoroutine;
    private bool IsDone;
    private int currentIndex = 0;

    [SerializeField]
    private bool playOnStart = true;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if(playOnStart) {
            Play();
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        IsDone = false;
        animCoroutine = StartCoroutine(PlayCo());
    }
    public void Stop()
    {
        IsDone = true;
        StopCoroutine(animCoroutine);
    }

    IEnumerator PlayCo()
    {
        yield return new WaitForSeconds(speed);

        image.sprite = spriteArray[currentIndex];
        currentIndex = (currentIndex + 1) % spriteArray.Length;

        if (IsDone == false)
            animCoroutine = StartCoroutine(PlayCo());
    }
}
