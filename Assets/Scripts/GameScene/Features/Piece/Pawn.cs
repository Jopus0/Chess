using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class Pawn : Piece
{
    private CellPosition[] _singleMovement;
    private CellPosition[] _doubleSingleMovement;
    private CellPosition[] _singleAttackMovement;
    private int doubleMoveRow;
    public Pawn(bool whiteOrBlack, Sprite pieceSprite) : base(whiteOrBlack, pieceSprite) 
    {
        if (WhiteOrBlack)
        {
            _singleMovement = new CellPosition[] { new CellPosition(-1, 0) };
            _singleAttackMovement = new CellPosition[] { new CellPosition(-1, -1), new CellPosition(-1, 1) };
            doubleMoveRow = 6;
        }
        else
        {
            _singleMovement = new CellPosition[] { new CellPosition(1, 0) };
            _singleAttackMovement = new CellPosition[] { new CellPosition(1, -1), new CellPosition(1, 1) };
            doubleMoveRow = 1;
        }
    }
    public override List<CellPosition> FindMoves(int[,] board, CellPosition position)
    {
        List<CellPosition> possibleMoves = new List<CellPosition>();

        SingleMovement(board, position, _singleMovement, possibleMoves);
        SingleAttackMovement(board, position, _singleAttackMovement, possibleMoves);
        return possibleMoves;
    }
    private int GetMovesCount(CellPosition position)
    {
        if (position.Row == doubleMoveRow)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
    protected override void SingleMovement(int[,] board, CellPosition position, CellPosition[] moves, List<CellPosition> possibleMoves)
    {
        int boardHeight = board.GetLength(0);
        int boardWidth = board.GetLength(1);
        int cellValue = board[position.Row, position.Column];

        foreach (var move in moves)
        {
            CellPosition newPosition = new CellPosition(position.Row + move.Row, position.Column + move.Column);
            int movesCount = GetMovesCount(position);
            for (int i = 0; i < movesCount; i++)
            {
                if (IsValidPosition(newPosition, boardWidth, boardHeight) && IsCellEmpty(board[newPosition.Row, newPosition.Column]))
                {
                    possibleMoves.Add(newPosition);
                }
                else
                {
                    break;
                }
                newPosition.Row += move.Row;
                newPosition.Column += move.Column;
            }
        }
    }
    private void SingleAttackMovement(int[,] board, CellPosition position, CellPosition[] moves, List<CellPosition> possibleMoves)
    {
        int boardHeight = board.GetLength(0);
        int boardWidth = board.GetLength(1);
        int cellValue = board[position.Row, position.Column];

        foreach (var move in moves)
        {
            CellPosition newPosition = new CellPosition(position.Row + move.Row, position.Column + move.Column);
            if (IsValidPosition(newPosition, boardWidth, boardHeight) &&
                !IsCellEmpty(board[newPosition.Row, newPosition.Column]) &&
                !IsPieceSameColor(cellValue, board[newPosition.Row, newPosition.Column]))
            {
                possibleMoves.Add(newPosition);
            }
        }
    }
}
