using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSwipeDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float triggerThreshold = 100f; // X birim, ne kadar yukarı çekerse void çalışsın
    private Vector2 startTouchPos;
    private bool isPressing = false;
    [SerializeField] DragRagdoll dragRagdoll;
    [SerializeField] CharacterSO characterSO;


    [Header("Info")]
    [SerializeField] Transform infoTextParent;
    [SerializeField] GameObject infoText;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] List<InfoText> InfoTexts;
    void Start()
    {
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
        startTouchPos = eventData.position; // Parmağın başlangıç noktasını al
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isPressing) return;

        float deltaY = eventData.position.y - startTouchPos.y; // Ne kadar yukarı kaydırdı
        if (deltaY >= triggerThreshold)
        {
            TriggerFunction();
            isPressing = false; // Bir kere çalıştırıp durdur
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false; // Parmağı kaldırınca sıfırla
    }

    private void TriggerFunction()
    {
        GameObject character = Instantiate(characterSO.characterPrefab);
        dragRagdoll.SetCharacter(character.GetComponent<CharacterBehaviour>(), characterSO, CompleteCharacter);
    }
    public void CompleteCharacter()
    {
        foreach (var item in InfoTexts)
        {
            item.InfoCompleted();
        }
        Destroy(gameObject);
    }
}
