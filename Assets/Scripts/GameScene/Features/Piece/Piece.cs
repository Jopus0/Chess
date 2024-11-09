using System.Collections.Generic;
using UnityEngine;
public abstract class Piece
{
    public bool WhiteOrBlack { get; protected set; }
    public Sprite PieceSprite { get; protected set; }
    public bool Value { get; protected set; }
    public bool IsMoved { get; set; }
    public Piece(bool whiteOrBlack, Sprite pieceSprite)
    {
        WhiteOrBlack = whiteOrBlack;
        PieceSprite = pieceSprite;
    }
    public abstract List<CellPosition> FindMoves(int[,] board, CellPosition position);
    protected bool IsValidPosition(CellPosition position, int boardWidth, int boardHeight)
    {
        return position.Row >= 0 && position.Row < boardHeight && position.Column >= 0 && position.Column < boardWidth;
    }
    protected bool IsPieceSameColor(int cellValue1, int cellValue2)
    {
        return cellValue1 * cellValue2 > 0;
    }
    protected bool IsCellEmpty(int cellValue)
    {
        return cellValue == (int)Enums.PieceType.None;
    }
    protected virtual void SingleMovement(int[,] board, CellPosition position, CellPosition[] moves, List<CellPosition> possibleMoves)
    {
        int boardHeight = board.GetLength(0);
        int boardWidth = board.GetLength(1);
        int cellValue = board[position.Row, position.Column];

        foreach (var move in moves)
        {
            CellPosition newPosition = new CellPosition(position.Row + move.Row, position.Column + move.Column);
            if (IsValidPosition(newPosition, boardWidth, boardHeight) && !IsPieceSameColor(cellValue, board[newPosition.Row, newPosition.Column]))
            {
                possibleMoves.Add(newPosition);
            }
        }
    }
    protected virtual void MultipleMovement(int[,] board, CellPosition position, CellPosition[] moves, List<CellPosition> possibleMoves)
    {
        int boardHeight = board.GetLength(0);
        int boardWidth = board.GetLength(1);
        int cellValue = board[position.Row, position.Column];

        foreach (var move in moves)
        {
            CellPosition newPosition = new CellPosition(position.Row + move.Row, position.Column + move.Column);

            while (IsValidPosition(newPosition, boardWidth, boardHeight))
            {
                int newCellValue = board[newPosition.Row, newPosition.Column];

                if (IsCellEmpty(newCellValue))
                {
                    possibleMoves.Add(newPosition);
                }
                else if (!IsPieceSameColor(cellValue, newCellValue))
                {
                    possibleMoves.Add(newPosition);
                    break;
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
}
