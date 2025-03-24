using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI text;

    public void SetText(string info)
    {
        text.text = info;
    }
    public void InfoCompleted()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
        toggle.isOn = true;
        text.DOFade(.5f, .2f);
    }
}
