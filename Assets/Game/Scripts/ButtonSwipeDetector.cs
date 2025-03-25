using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSwipeDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float triggerThreshold = 100f;
    private Vector2 startTouchPos;
    private bool isPressing = false;
    private int originalSiblingIndex;
    private bool isDragging = false;

    [SerializeField] CharacterSO characterSO;
    [SerializeField] GameObject buttonContent;
    HorizontalLayoutGroup layoutGroup;
    ContentSizeFitter sizeFitter;
    DragRagdoll dragRagdoll;

    [Header("Info")]
    Transform infoTextParent;
    [SerializeField] GameObject infoText;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] List<InfoText> InfoTexts;

    public void SetContent(CharacterSO characterSO, HorizontalLayoutGroup horizontalLayoutGroup, ContentSizeFitter contentSizeFitter, Transform infoTextParent, DragRagdoll dragRagdoll)
    {
        this.characterSO = characterSO;
        layoutGroup = horizontalLayoutGroup;
        sizeFitter = contentSizeFitter;
        this.infoTextParent = infoTextParent;
        this.dragRagdoll = dragRagdoll;

        characterImage.sprite = characterSO.characterSprite;
        characterNameText.text = characterSO.characterName;
        foreach (string info in characterSO.infos)
        {
            GameObject infoText = Instantiate(this.infoText, infoTextParent);
            InfoText infoTextCs = infoText.GetComponent<InfoText>();
            infoTextCs.SetText(info);
            InfoTexts.Add(infoTextCs);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        isDragging = false;
        startTouchPos = eventData.position; // Parmağın başlangıç noktasını al
        originalSiblingIndex = transform.GetSiblingIndex(); // Sibling indexi kaydet
        DisableLayout();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isPressing) return;

        isDragging = true;
        transform.position = eventData.position; // X ve Y ekseninde parmağı takip et

        float deltaY = eventData.position.y - startTouchPos.y;
        if (deltaY >= triggerThreshold)
        {
            TriggerFunction();
            isPressing = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;

        isPressing = false;
        if (buttonContent.activeSelf)
            ResetCardPosition();
        else
            Invoke("ResetCardPosition", 1f);
    }

    private void TriggerFunction()
    {
        buttonContent.SetActive(false);
        GameObject character = Instantiate(characterSO.characterPrefab);
        CharacterBehaviour characterBehaviour = character.GetComponent<CharacterBehaviour>();

        character.transform.position = GetMouseWorldPosition(characterBehaviour);
        dragRagdoll.SetCharacter(characterBehaviour, characterSO, CompleteCharacter);
    }
    public Vector3 GetMouseWorldPosition(CharacterBehaviour characterBehaviour)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(characterBehaviour.head.position).z; // Derinlik korunsun
        Vector3 returnedPos = Camera.main.ScreenToWorldPoint(mousePos);
        returnedPos.y = dragRagdoll.offset.y - 1.5f;
        returnedPos.z = returnedPos.z + dragRagdoll.offset.z;
        returnedPos.x = returnedPos.x + dragRagdoll.offset.x;
        return returnedPos;
    }
    public void CompleteCharacter()
    {
        foreach (var item in InfoTexts)
        {
            item.InfoCompleted();
        }
        EnableLayout();
        Destroy(gameObject);
    }

    private void ResetCardPosition()
    {
        buttonContent.SetActive(true);
        transform.SetSiblingIndex(originalSiblingIndex);
        EnableLayout();
    }

    private void DisableLayout()
    {
        if (layoutGroup) layoutGroup.enabled = false;
        if (sizeFitter) sizeFitter.enabled = false;
    }

    private void EnableLayout()
    {
        if (layoutGroup) layoutGroup.enabled = true;
        if (sizeFitter) sizeFitter.enabled = true;
    }
}
