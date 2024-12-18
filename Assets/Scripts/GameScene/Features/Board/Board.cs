using System;
using System.Collections.Generic;
using UnityEngine;
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
    private List<CellPosition> _castleCell;

    public event Action<CellPosition, List<CellPosition>> Select;
    public event Action<CellPosition, CellPosition> Move;
    public event Action<CellPosition> Check;
    public event Action<CellPosition> Mate;
    public Board(PieceFactory pieceFactory,Vector2Int boardSize, PiecePosition[] arrangement, bool whiteOrBlackMove, Cell[,] cells)
    {
        _whiteOrBlackMove = whiteOrBlackMove;

        _selectedCell = new CellPosition(-1, -1);
        _takeOnPassCell = new CellPosition(-1, -1);
        _castleCell = new List<CellPosition>();
        _possibleMoves = new List<CellPosition>();

        _moveHistory = new List<Move>();

        _board = new int[boardSize.x, boardSize.y];

        _boardSize = boardSize;
        _pieceFactory = pieceFactory;
        _cells = cells;

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
    public void CellClick(CellPosition position)
    {
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
    private bool CheckPieceColor(int cellValue, bool whiteOrBlack)
    {
        return (whiteOrBlack && cellValue > 0) || (!whiteOrBlack && cellValue < 0);
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

        Piece movingPiece = _cells[_selectedCell.Row, _selectedCell.Column].GetPiece();
        movingPiece.IsMoved = true;

        if (!Castle(position))
        {
            _board[position.Row, position.Column] = _board[_selectedCell.Row, _selectedCell.Column];
            _board[_selectedCell.Row, _selectedCell.Column] = (int)Enums.PieceType.None;

            _cells[position.Row, position.Column].ChangePiece(movingPiece);
            _cells[_selectedCell.Row, _selectedCell.Column].ChangePiece(null);

            TakeOnPass(position);
        }

        _moveHistory.Add(new Move(_selectedCell, position));
        Move?.Invoke(_selectedCell, position);
        ResetSelect();

        _whiteOrBlackMove = !_whiteOrBlackMove;

        CellPosition kingPosition = FindPiece(Enums.PieceType.King, _whiteOrBlackMove, _board)[0];
        if (IsCellChecked(kingPosition, _board))
        {
            Check?.Invoke(kingPosition);
        }
        if(IsMate(_whiteOrBlackMove))
        {
            Mate?.Invoke(kingPosition);
        }

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
        _possibleMoves = AvaluableMoves(_possibleMoves, position);
        CanCastle(position);

        Select?.Invoke(position, _possibleMoves);
    }

    private void CanCastle(CellPosition position)
    {
        _castleCell.Clear();
        if (!IsSamePiece(_board[position.Row, position.Column], Enums.PieceType.King) || 
            _cells[position.Row, position.Column].GetPiece().IsMoved || IsCellChecked(position, _board))
        {
            return;
        }

        List<CellPosition> rookPositions = FindPiece(Enums.PieceType.Rook, _whiteOrBlackMove, _board);
        foreach(var rookPosition in rookPositions)
        {
            if (_cells[rookPosition.Row, rookPosition.Column].GetPiece().IsMoved)
            {
                continue;
            }

            int minColumn = position.Column > rookPosition.Column ? rookPosition.Column : position.Column;
            int maxColumn = position.Column > rookPosition.Column ? position.Column : rookPosition.Column;
            bool isRangeEmpty = true;
            for (int i = minColumn + 1; i < maxColumn; i++)
            {
                if (_board[position.Row, i] != (int)Enums.PieceType.None || IsCellChecked(new CellPosition(position.Row, i) ,_board))
                {
                    isRangeEmpty = false;
                    break;
                }
            }
            if (isRangeEmpty)
            {
                _castleCell.Add(rookPosition);
                _possibleMoves.Add(rookPosition);
            }
        }
    }
    private void CanTakeOnPass(CellPosition position)
    {
        ResetTakeOnPass();
        if (!IsSamePiece(_board[position.Row, position.Column], Enums.PieceType.Pawn) || _moveHistory.Count <= 0)
        {
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
    }
    private bool Castle(CellPosition position)
    {
        if(!_castleCell.Contains(position))
        {
            return false;
        }

        int kingMove = position.Column > _selectedCell.Column ? 2 : -2;
        int rookMove = position.Column > _selectedCell.Column ? 1 : -1;

        _board[_selectedCell.Row, _selectedCell.Column + kingMove] = _board[_selectedCell.Row, _selectedCell.Column];
        _board[_selectedCell.Row, _selectedCell.Column] = (int)Enums.PieceType.None;
        _cells[_selectedCell.Row, _selectedCell.Column + kingMove].ChangePiece(_cells[_selectedCell.Row, _selectedCell.Column].GetPiece());
        _cells[_selectedCell.Row, _selectedCell.Column].ChangePiece(null);

        _board[_selectedCell.Row, _selectedCell.Column + rookMove] = _board[position.Row, position.Column];
        _board[position.Row, position.Column] = (int)Enums.PieceType.None;
        _cells[_selectedCell.Row, _selectedCell.Column + rookMove].ChangePiece(_cells[position.Row, position.Column].GetPiece());
        _cells[position.Row, position.Column].ChangePiece(null);

        return true;
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
    private List<CellPosition> FindPiece(Enums.PieceType pieceType, bool whiteOrBlack, int[,] board)
    {
        List<CellPosition> piecePositions = new List<CellPosition>();
        for (int i = 0; i < _boardSize.x; i++)
        {
            for(int j = 0; j <  _boardSize.y; j++)
            {
                if (IsSamePiece(board[i,j], pieceType) && CheckPieceColor(board[i, j], whiteOrBlack))
                {
                    piecePositions.Add(new CellPosition(i, j));
                }
            }
        }
        return piecePositions;
    }
    private List<CellPosition> GetAllPosibleMove(bool whiteOrBlack, int[,] board)
    {
        List<CellPosition> allPossibleMoves = new List<CellPosition>();
        for (int i = 0; i < _boardSize.x; i++)
        {
            for (int j = 0; j < _boardSize.y; j++)
            {
                if (CheckPieceColor(board[i, j], whiteOrBlack))
                {
                    allPossibleMoves.AddRange(_cells[i, j].GetPiece().FindMoves(board, new CellPosition(i, j)));
                }
            }
        }
        return allPossibleMoves;
    }
    private bool IsCellChecked(CellPosition position ,int[,] board)
    {
        List<CellPosition> allPossibleMoves = GetAllPosibleMove(!_whiteOrBlackMove, board);
        return allPossibleMoves.Contains(position);
    }
    private List<CellPosition> AvaluableMoves(List<CellPosition> possibleMoves, CellPosition selectedCell)
    {
        List<CellPosition> newPossibleMoves = new List<CellPosition>();
        int[,] tempBoard = new int[_boardSize.x, _boardSize.y];
        for (int i = 0; i < _boardSize.x; i++)
        {
            for (int j = 0; j < _boardSize.y; j++)
            {
                tempBoard[i, j] = _board[i, j];
            }
        }

        foreach (var possibleMove in possibleMoves)
        {
            int selectedValue = tempBoard[selectedCell.Row, selectedCell.Column];
            tempBoard[selectedCell.Row, selectedCell.Column] = (int)Enums.PieceType.None;
            int moveValue = tempBoard[possibleMove.Row, possibleMove.Column];
            tempBoard[possibleMove.Row, possibleMove.Column] = selectedValue;

            List<CellPosition> kingPosition = FindPiece(Enums.PieceType.King, _whiteOrBlackMove, tempBoard);
            if (!IsCellChecked(kingPosition[0],tempBoard))
            {
                newPossibleMoves.Add(possibleMove);
            }
            tempBoard[selectedCell.Row, selectedCell.Column] = selectedValue;
            tempBoard[possibleMove.Row, possibleMove.Column] = moveValue;
        }
        return newPossibleMoves;
    }
    
    private bool IsMate(bool whiteOrBlack)
    {
        List<CellPosition> possibleMoves = new List<CellPosition>();
        CellPosition position = new CellPosition();
        for (int i = 0; i < _boardSize.x; i++)
        {
            for(int j = 0; j < _boardSize.y; j++)
            {
                if (CheckPieceColor(_board[i,j], whiteOrBlack))
                {
                    position.Row = i;
                    position.Column = j;
                    Piece piece = _cells[i, j].GetPiece();

                    possibleMoves = AvaluableMoves(piece.FindMoves(_board, position), position);

                    if (possibleMoves.Count > 0)
                    {
                        return false;  
                    }
                }
            }
        }
        return true;
    }
}