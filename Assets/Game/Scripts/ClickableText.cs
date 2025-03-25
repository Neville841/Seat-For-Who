using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableText : MonoBehaviour, IPointerClickHandler
{
    TMP_Text textComponent;
    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textComponent, Input.mousePosition, null);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textComponent.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();

            Debug.Log(linkId);
        }
    }
}