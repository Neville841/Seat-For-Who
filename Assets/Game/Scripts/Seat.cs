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
    public bool CheckSeats(CharacterBehaviour characterBehaviour)
    {
        var gameCharacter = characterController.gameCharacters.Find(c => c.name == characterBehaviour.characterSO.characterName);
        if (gameCharacter != null)
        {
            if (gameCharacter.seats.Contains(this))
            {
                Debug.Log("gameCharacter Contains");
                return true;
            }
            else
            {
                Debug.Log("Sandalye yok");
                return false;
            }

        }
        else
        {
            Debug.Log("gameCharacter null");
            return false;
        }
    }
    public void SetCharacter(CharacterBehaviour characterBehaviour)
    {
        character = characterBehaviour;
        gameObject.layer = 0;
        characterController.SetCompletedCharacter(this);
    }
}
