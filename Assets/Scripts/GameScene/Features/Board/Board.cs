using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Board
{
    private PieceFactory _pieceFactory;

    private Vector2Int _boardSize;
    private Cell[,] _cells;
    private int[,] _board;
    private bool _whiteOrBlackMove;

    private CellPosition _selectedCell;
    private List<CellPosition> _possibleMoves;

    private List<Move> _moveHistory;

    private CellPosition _takeOnPassCell;
    public Board(PieceFactory pieceFactory,Vector2Int boardSize, PiecePosition[] arrangement, bool whiteOrBlackMove, Cell[,] cells)
    {
        _whiteOrBlackMove = whiteOrBlackMove;

        _selectedCell = new CellPosition(-1, -1);
        _takeOnPassCell = new CellPosition(-1, -1);
        _possibleMoves = new List<CellPosition>();

        _moveHistory = new List<Move>();

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
        ChangeOutline(false);

        bool isMoveDone = false;
        if (_selectedCell.Row >= 0)
        {
            isMoveDone = MovePiece(position);
        }
        if(!isMoveDone)
        {
            SelectPiece(position);
        }
    }
    private bool IsRightTurn(int cellValue)
    {
        return (cellValue > 0 && _whiteOrBlackMove) || (cellValue < 0 && !_whiteOrBlackMove);
    }
    private bool IsCellEmpty(int cellValue)
    {
        return cellValue == (int)Enums.PieceType.None;
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
    private bool IsSamePiece(int cellValue, Enums.PieceType pieceType)
    {
        return Mathf.Abs(cellValue) == (int)pieceType;
    }
    private void ResetSelect()
    {
        _selectedCell.Row = -1;
        _selectedCell.Column = -1;
    }
    private void ResetTakeOnPass()
    {
        _takeOnPassCell.Row = -1;
        _takeOnPassCell.Column = -1;
    }
    private bool MovePiece(CellPosition position)
    {
        if (!IsMovePossible(position))
        {
            return false;
        }

        _board[position.Row, position.Column] = _board[_selectedCell.Row, _selectedCell.Column];
        _board[_selectedCell.Row, _selectedCell.Column] = (int)Enums.PieceType.None;

        _cells[position.Row, position.Column].ChangePiece(_cells[_selectedCell.Row, _selectedCell.Column].GetPiece());
        _cells[_selectedCell.Row, _selectedCell.Column].ChangePiece(null);

        TakeOnPass(position);

        _moveHistory.Add(new Move(_selectedCell, position));
        ResetSelect();

        _whiteOrBlackMove = !_whiteOrBlackMove;

        return true;
    }
    private void SelectPiece(CellPosition position)
    {
        if (IsCellEmpty(_board[position.Row, position.Column]) || !IsRightTurn(_board[position.Row, position.Column]))
        {
            ResetSelect();
            return;
        }

        _selectedCell = position;
        Piece piece = _cells[position.Row, position.Column].GetPiece();
        _possibleMoves = piece.FindMoves(_board, position);

        CanTakeOnPass(position);

        ChangeOutline(true);
    }
    private void CanTakeOnPass(CellPosition position)
    {
        if(!IsSamePiece(_board[position.Row, position.Column], Enums.PieceType.Pawn) || _moveHistory.Count <= 0)
        {
            ResetTakeOnPass();
            return;
        }

        Move lastMove = _moveHistory[_moveHistory.Count - 1];
        bool isPawn = IsSamePiece(_board[lastMove.To.Row, lastMove.To.Column], Enums.PieceType.Pawn);
        if(isPawn && Mathf.Abs(lastMove.From.Row - lastMove.To.Row) == 2 && position.Row == lastMove.To.Row)
        {
            _takeOnPassCell.Row = (lastMove.From.Row + lastMove.To.Row) / 2;
            _takeOnPassCell.Column = lastMove.From.Column;
            _possibleMoves.Add(_takeOnPassCell);
        }
        else
        {
            ResetTakeOnPass();
        }
    }

    private void TakeOnPass(CellPosition position)
    {
        if (position.Row != _takeOnPassCell.Row || position.Column != _takeOnPassCell.Column)
        {
            return;
        }
        CellPosition lastPawnPosition = _moveHistory[_moveHistory.Count - 1].To;
        _board[lastPawnPosition.Row, lastPawnPosition.Column] = (int)Enums.PieceType.None;
        _cells[lastPawnPosition.Row, lastPawnPosition.Column].ChangePiece(null);
    }
    private void ChangeOutline(bool showOrClose)
    {
        foreach(var possibleMove in _possibleMoves)
        {
            bool moveOrAttack = IsCellEmpty(_board[possibleMove.Row, possibleMove.Column]);
            _cells[possibleMove.Row, possibleMove.Column].ChangeOutline(showOrClose, moveOrAttack);
        }
    }
}