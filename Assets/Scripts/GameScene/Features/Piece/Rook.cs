using System.Collections.Generic;
using UnityEngine;
public class Rook : Piece
{
    private CellPosition[] _multipleMovement;
    public Rook(bool whiteOrBlack, Sprite pieceSprite) : base(whiteOrBlack, pieceSprite)
    {
        _multipleMovement = new CellPosition[] {
            new CellPosition(-1, 0), new CellPosition(1, 0),
            new CellPosition(0, -1), new CellPosition(0, 1),
        };
    }
    public override List<CellPosition> FindMoves(int[,] board, CellPosition position)
    {
        List<CellPosition> possibleMoves = new List<CellPosition>();
        MultipleMovement(board, position, _multipleMovement, possibleMoves);
        return possibleMoves;
    }
}