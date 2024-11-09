using UnityEngine;

[CreateAssetMenu(fileName = "new BoardSettings", menuName = "BoardSettings")]
public class BoardSettings : ScriptableObject
{
    public int BoardWidth;
    public int BoardHeight;
    public PiecePosition[] Arrangement;
}

[System.Serializable]
public class PiecePosition
{
    public Enums.PieceType PieceType;
    public CellPosition Position;
    public bool WhiteOrBlack;
}
