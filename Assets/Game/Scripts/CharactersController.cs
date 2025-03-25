using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharactersController : MonoBehaviour
{
    [Inject] private DiContainer _container;

    [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField] ContentSizeFitter contentSizeFitter;
    [SerializeField] Transform infoTextParent;
    [SerializeField] GameObject characterSlotPrefab;
    [SerializeField] Transform characterSlotContent;
    [SerializeField] DragRagdoll dragRagdoll;
    [SerializeField] ScrollRect scroll;
    [SerializeField] CharacterSO[] characterSos;
    [SerializeField] internal List<GameCharacters> gameCharacters;
    [SerializeField] internal List<GameCharacters> completedCharacters;

    void Start()
    {
        for (int i = 0; i < characterSos.Length; i++)
        {
            GameObject slot = _container.InstantiatePrefab(characterSlotPrefab, characterSlotContent);
            slot.GetComponent<ButtonSwipeDetector>().SetContent(characterSos[i], horizontalLayoutGroup, contentSizeFitter, infoTextParent, dragRagdoll, scroll);
            gameCharacters[i].name = characterSos[i].characterName;
            gameCharacters[i].content = slot.transform;
        }
    }
    public void SetCompletedCharacter(Seat seat)
    {
        completedCharacters.Add(new GameCharacters
        {
            name = seat.character.characterSO.characterName,
            content = null,
            character = seat.character,
            seats = new List<Seat> { seat }
        });

    }
}
[Serializable]
public class GameCharacters
{
    [SerializeField] internal string name;
    [SerializeField] internal Transform content;
    [SerializeField] internal CharacterBehaviour character;
    [SerializeField] internal List<Seat> seats;
}