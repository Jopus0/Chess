using System.Collections.Generic;
using UnityEngine;
public class Pawn : Piece
{
    private CellPosition[] _whiteSingleMovement = new CellPosition[] { new CellPosition(-1, 0) };
    private CellPosition[] _blackSingleMovement = new CellPosition[] { new CellPosition(1, 0) };

    private CellPosition[] _singleMovement;
    public Pawn(bool whiteOrBlack, Sprite pieceSprite) : base(whiteOrBlack, pieceSprite) 
    {
        if (WhiteOrBlack)
        {
            _singleMovement = _whiteSingleMovement;
        }
        else
        {
            _singleMovement = _blackSingleMovement;
        }
    }
    
    public override List<CellPosition> FindMoves(int[,] board, CellPosition position)
    {
        //if (_board[newRow, newColumn] * _board[position.Row, position.Column] < (int)Enums.PieceType.None)
        List<CellPosition> possibleMoves = new List<CellPosition>();
        SingleMovement(board, position, _singleMovement, possibleMoves);
        return possibleMoves;
    }
}
