using UnityEngine;
using TMPro;

// https://www.youtube.com/watch?v=i8AvUMEjlgg
// https://forum.unity.com/threads/clickable-link-within-a-text.910226/
// <link="test" test="foo">Helllo</link>
public class LinkTagEventInvoker : MonoBehaviour
{
    private TMP_Text textbox;

    public static event System.Action<string> LinkFound;

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(CheckForLinkTags);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(CheckForLinkTags);
    }

    private void Awake()
    {
        textbox = GetComponent<TMP_Text>();
    }

    private void CheckForLinkTags(Object obj)
    {
        int nbOfLinksTags = textbox.textInfo.linkCount;

        if (nbOfLinksTags == 0)
        {
            return;
        }

        for (var i = 0; i < nbOfLinksTags; i++)
        {
            TMP_LinkInfo linkTagInfo = textbox.textInfo.linkInfo[i];
            // textbox.textInfo.textComponent.SetText("eezrzerz");
            // linkTagInfo.SetT
            // print("fefeefe 111 " + );
            // print("fefeefe 111 " + linkTagInfo.GetHashCode());
            // print("fefeefe 111 " + linkTagInfo.GetLinkText());
            LinkFound?.Invoke(linkTagInfo.GetLinkID());
        }
    }
}