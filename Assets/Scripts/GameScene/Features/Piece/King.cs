using System.Collections.Generic;
using UnityEngine;
public class King : Piece
{
    private CellPosition[] _singleMovement;
    public King(bool whiteOrBlack, Sprite pieceSprite) : base(whiteOrBlack, pieceSprite)
    {
        _singleMovement = new CellPosition[] {
            new CellPosition(-1, -1), new CellPosition(-1, 0), new CellPosition(-1, 1), new CellPosition(1, -1),
            new CellPosition(1, 0), new CellPosition(1, 1), new CellPosition(0, -1), new CellPosition(0, 1),
        };
    }
    public override List<CellPosition> FindMoves(int[,] board, CellPosition position)
    {
        List<CellPosition> possibleMoves = new List<CellPosition>();
        SingleMovement(board, position, _singleMovement, possibleMoves);
        return possibleMoves;
    }
}