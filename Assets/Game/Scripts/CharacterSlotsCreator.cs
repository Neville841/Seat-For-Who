using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotsCreator : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField] ContentSizeFitter contentSizeFitter;
    [SerializeField] Transform infoTextParent;
    [SerializeField] GameObject characterSlotPrefab;
    [SerializeField] Transform characterSlotContent;
    [SerializeField] DragRagdoll dragRagdoll;

    [SerializeField] CharacterSO[] characterSos;
    void Start()
    {
        foreach (CharacterSO character in characterSos)
        {
            GameObject slot = Instantiate(characterSlotPrefab, characterSlotContent);
            slot.GetComponent<ButtonSwipeDetector>().SetContent(character, horizontalLayoutGroup, contentSizeFitter, infoTextParent, dragRagdoll);
        }
    }
}