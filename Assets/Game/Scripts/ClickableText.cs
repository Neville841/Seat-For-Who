using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ClickableText : MonoBehaviour, IPointerClickHandler
{
    [Inject] CharactersController characterController;
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
            var gameCharacter = characterController.gameCharacters.Find(c => c.name == linkId);
            if (gameCharacter != null)
            {
                gameCharacter.content.DOKill();
                gameCharacter.content.GetComponent<ButtonSwipeDetector>().ScrollToTarget();
                gameCharacter.content.DOScale(Vector3.one * 1.2f, .2f)
                    .SetLoops(6, LoopType.Yoyo)
                    .OnComplete(() => gameCharacter.content.DOScale(Vector3.one, .2f));
                return;
            }
            gameCharacter = characterController.completedCharacters.Find(c => c.name == linkId);
            if (gameCharacter != null)
            {
                gameCharacter.character.transform.DOKill();
                gameCharacter.character.transform.DOScale(Vector3.one * 1.2f, .2f)
                    .SetLoops(6, LoopType.Yoyo)
                    .OnComplete(() => gameCharacter.character.transform.DOScale(Vector3.one, .2f));
            }
            else
            {
                Debug.LogWarning("Karakter hiçbir yerde bulunamadý: " + linkId);
            }
        }
    }
}