using System.Collections.Generic;
using UnityEngine;
public class Bishop : Piece
{
    private CellPosition[] _multipleMovement = new CellPosition[] { new CellPosition(-1, -1), new CellPosition(-1, 1),
    new CellPosition(1, -1), new CellPosition(1, 1)};
    public Bishop(bool whiteOrBlack, Sprite pieceSprite) : base(whiteOrBlack, pieceSprite) { }

    public override List<CellPosition> FindMoves(int[,] board, CellPosition position)
    {
        List<CellPosition> possibleMoves = new List<CellPosition>();
        MultipleMovement(board, position, _multipleMovement, possibleMoves);
        return possibleMoves;
    }
}