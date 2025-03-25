using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Seat : MonoBehaviour
{
    [Inject] CharactersController characterController;
    [SerializeField] internal SeatType seatType;
    [SerializeField] internal Transform characterPos;
    [SerializeField] internal CharacterBehaviour character;

    private void Start()
    {
        if (character) gameObject.layer = 0;
    }
    public void SetCharacter(CharacterBehaviour characterBehaviour)
    {
        character = characterBehaviour;
        gameObject.layer = 0;
        characterController.SetCompletedCharacter(this);
    }
}
