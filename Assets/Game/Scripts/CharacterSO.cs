using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "CharacterSO/New Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] internal string characterName;
    [SerializeField] internal Sprite characterSprite;
    [SerializeField] internal GameObject characterPrefab;
    [SerializeField] private SeatType seatType;
    [SerializeField] internal string[] infos;
    public SeatType SeatType => seatType;
}