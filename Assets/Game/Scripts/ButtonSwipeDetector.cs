using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ButtonSwipeDetector : MonoBehaviour, IPointerDownHandler
{
    [Inject] private DiContainer _container;
    public float triggerThreshold = 100f;
    private Vector2 startTouchPos;
    [SerializeField] bool isPressing = false;
    [SerializeField] bool isDragging = false;
    private int originalSiblingIndex;

    [SerializeField] CharacterSO characterSO;
    [SerializeField] GameObject buttonContent;
    HorizontalLayoutGroup layoutGroup;
    ContentSizeFitter sizeFitter;
    ScrollRect scrollRect;
    DragRagdoll dragRagdoll;

    [Header("Info")]
    Transform infoTextParent;
    [SerializeField] GameObject infoText;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] List<InfoText> InfoTexts;

    public void SetContent(CharacterSO characterSO, HorizontalLayoutGroup horizontalLayoutGroup, ContentSizeFitter contentSizeFitter, Transform infoTextParent, DragRagdoll dragRagdoll, ScrollRect scrollRect)
    {
        this.characterSO = characterSO;
        layoutGroup = horizontalLayoutGroup;
        sizeFitter = contentSizeFitter;
        this.infoTextParent = infoTextParent;
        this.dragRagdoll = dragRagdoll;
        this.scrollRect = scrollRect;
        characterImage.sprite = characterSO.characterSprite;
        characterNameText.text = characterSO.characterName;
        foreach (string info in characterSO.infos)
        {
            GameObject infoText = _container.InstantiatePrefab(this.infoText, infoTextParent);
            InfoText infoTextCs = infoText.GetComponent<InfoText>();
            infoTextCs.SetText(info);
            InfoTexts.Add(infoTextCs);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!layoutGroup.enabled) return;

        isPressing = true;
        isDragging = false;
        startTouchPos = eventData.position; // Parmağın başlangıç noktasını al
        originalSiblingIndex = transform.GetSiblingIndex(); // Sibling indexi kaydet
        StartCoroutine(CheckXY());
    }
    IEnumerator CheckXY()
    {
        yield return new WaitForSeconds(.1f);
        Vector2 mousePosition2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 delta = mousePosition2D - startTouchPos; // Toplam kayma miktarı

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            isPressing = false;
            isDragging = false;
            EnableLayout();
            ResetCardPosition();

            yield break;
        }
        else
        {
            isDragging = true;
            DisableLayout();
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging && !isPressing) return;
            isDragging = false;
            isPressing = false;
            if (buttonContent.activeSelf)
                ResetCardPosition();
            else
            {
                if (dragRagdoll.selectedSeat) Invoke("ResetCardPosition", 2f);
                else ResetCardPosition();
            }
        }

        if (!isPressing) return;
        if (!isDragging)
            return;

        float deltaY = Input.mousePosition.y - startTouchPos.y;
        if (Mathf.Abs(deltaY) <= 30 && !buttonContent.activeSelf) return;
        else if (buttonContent.activeSelf) DisableLayout();

        transform.position = Input.mousePosition;
        if (deltaY >= triggerThreshold)
        {
            TriggerFunction();
            isPressing = false;
        }

    }
    private void TriggerFunction()
    {
        buttonContent.SetActive(false);
        GameObject character = _container.InstantiatePrefab(characterSO.characterPrefab);
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
        if (scrollRect) scrollRect.enabled = false;
    }

    private void EnableLayout()
    {
        if (layoutGroup) layoutGroup.enabled = true;
        if (sizeFitter) sizeFitter.enabled = true;
        if (scrollRect) scrollRect.enabled = true;
    }
    public void ScrollToTarget()
    {
        float contentWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        float viewportWidth = scrollRect.viewport.rect.width;

        float targetX = Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x);

        float newNormalizedPos = targetX / (contentWidth - viewportWidth);

        if (transform.GetSiblingIndex() == 0)
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(0);
        else scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(newNormalizedPos);

    }
}