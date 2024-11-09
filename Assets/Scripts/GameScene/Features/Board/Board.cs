using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Board
{
    private PieceFactory _pieceFactory;

    private Vector2Int _boardSize;
    private Cell[,] _cells;
    private int[,] _board;

    private CellPosition _selectedCell;
    private List<CellPosition> _possibleMoves;
    public Board(PieceFactory pieceFactory,Vector2Int boardSize, PiecePosition[] arrangement, Cell[,] cells)
    {
        _selectedCell = new CellPosition();
        _possibleMoves = new List<CellPosition>();

        _board = new int[boardSize.x, boardSize.y];

        _boardSize = boardSize;
        _pieceFactory = pieceFactory;
        _cells = cells;

        foreach (var cell in cells)
        {
            cell.OnClick += OnCellClick;
        }

        PlacePieces(arrangement);
    }
    private void PlacePieces(PiecePosition[] arrangement)
    {
        for (int i = 0; i < _boardSize.x; i++)
        {
            for (int j = 0; j < _boardSize.y; j++)
            {
                _board[i, j] = (int)Enums.PieceType.None;
            }
        }

        foreach (var piecePosition in arrangement)
        {
            Piece piece = _pieceFactory.GetPieceByType(piecePosition.PieceType, piecePosition.WhiteOrBlack);
            _cells[piecePosition.Position.Row, piecePosition.Position.Column].ChangePiece(piece);
            if(piecePosition.WhiteOrBlack)
            {
                _board[piecePosition.Position.Row, piecePosition.Position.Column] = (int)piecePosition.PieceType;
            }
            else
            {
                _board[piecePosition.Position.Row, piecePosition.Position.Column] = -(int)piecePosition.PieceType;
            }
        }
    }
    public void OnCellClick(CellPosition position)
    {
        MovePiece(position);
        ShowOutline(false);

        if (_board[position.Row, position.Column] == 0)
        {
            _selectedCell = new CellPosition();
            _possibleMoves = new List<CellPosition>();
            return;
        }

        _selectedCell = position;
        Piece piece = _cells[position.Row, position.Column].GetPiece();
        _possibleMoves = piece.FindMoves(_board, position);
        ShowOutline(true);
    }
    private bool IsMovePossible(CellPosition position)
    {
        bool isMovePossible = false;
        foreach (var possibleMove in _possibleMoves)
        {
            if (position.Row == possibleMove.Row && position.Column == possibleMove.Column)
            {
                isMovePossible = true;
            }
        }
        return isMovePossible;
    }
    private void MovePiece(CellPosition position)
    {
        if (!IsMovePossible(position))
        {
            return;
        }

        _board[position.Row, position.Column] = _board[_selectedCell.Row, _selectedCell.Column];
        _board[_selectedCell.Row, _selectedCell.Column] = (int)Enums.PieceType.None;

        _cells[position.Row, position.Column].ChangePiece(_cells[_selectedCell.Row, _selectedCell.Column].GetPiece());
        _cells[_selectedCell.Row, _selectedCell.Column].ChangePiece(null);
    }
    private void ShowOutline(bool showOrClose)
    {
        foreach(var possibleMove in _possibleMoves)
        {
            bool moveOrAttack = _board[possibleMove.Row, possibleMove.Column] == (int)Enums.PieceType.None;
            _cells[possibleMove.Row, possibleMove.Column].ChangeOutline(showOrClose, moveOrAttack);
        }
    }
}
