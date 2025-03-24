using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] internal SeatType seatType;
    [SerializeField] internal Transform characterPos;
    [SerializeField] internal CharacterBehaviour character;

    public void SetCharacter(CharacterBehaviour characterBehaviour)
    {
        character=characterBehaviour;
        gameObject.layer = 0;
    }
}
