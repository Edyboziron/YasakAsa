using UnityEngine;
public enum CardType { Attack, Heal, RestoreStability, Shield, Stun } // Stun eklendi
[CreateAssetMenu(fileName = "NewCard", menuName = "Card System/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int value;
    public int manaCost; // Bu artýk Stability maliyeti olarak çalýþacak
    public CardType cardType;
}