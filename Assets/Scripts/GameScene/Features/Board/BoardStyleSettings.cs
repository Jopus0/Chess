using UnityEngine;

[CreateAssetMenu(fileName = "new BoardStyleSettings", menuName = "BoardStyleSettings")]
public class BoardStyleSettings : ScriptableObject
{
    public Color WhiteColor;
    public Color BlackColor;

    public Color CheckCellColor;
    public Color MateCellColor;

    public Sprite MoveOutlineSprite;
    public Sprite AttackOutlineSprite;

    public Color MoveOutlineColor;
    public Color AttackOutlineColor;

    public Sprite WhiteKingSprite;
    public Sprite BlackKingSprite;

    public Sprite WhitePawnSprite;
    public Sprite BlackPawnSprite;

    public Sprite WhiteHorseSprite;
    public Sprite BlackHorseSprite;

    public Sprite WhiteBishopSprite;
    public Sprite BlackBishopSprite;

    public Sprite WhiteRookSprite;
    public Sprite BlackRookSprite;

    public Sprite WhiteQueenSprite;
    public Sprite BlackQueenSprite;
}
