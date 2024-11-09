using System.Collections.Generic;
using UnityEngine;
public class Horse : Piece
{
    private CellPosition[] _singleMovement = new CellPosition[] 
    { new CellPosition(-1, -2), new CellPosition(-1, 2), new CellPosition(1, -2), new CellPosition(1, 2), 
      new CellPosition(2, -1), new CellPosition(-2, -1), new CellPosition(-2, 1), new CellPosition(2, 1),
    };
    public Horse(bool whiteOrBlack, Sprite pieceSprite) : base(whiteOrBlack, pieceSprite) { }

    public override List<CellPosition> FindMoves(int[,] board, CellPosition position)
    {
        List<CellPosition> possibleMoves = new List<CellPosition>();
        SingleMovement(board, position, _singleMovement, possibleMoves);
        return possibleMoves;
    }
}