using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BoardContoller : MonoBehaviour 
{
    [SerializeField] private BoardView _boardView;
    [SerializeField] private Board _board;

    private Cell[,] _cells;
    private bool _isMate;
    private void OnDisable()
    {
        _board.Select -= OnSelect;
        _board.Move -= OnMove;
        _board.Check -= OnCheck;
        _board.Mate -= OnMate;

        foreach (var cell in _cells)
        {
            cell.OnClick -= OnCellClick;
        }
    }
    public void Init(BoardSettings boardSettings, BoardStyleSettings boardStyleSettings)
    {
        _isMate = false;
        Vector2Int boardSize = new Vector2Int(boardSettings.BoardHeight, boardSettings.BoardWidth);

        _boardView.Init(boardSize, boardStyleSettings);
        _cells = _boardView.ÑreateBoard();

        PieceFactory pieceFactory = new PieceFactory(boardStyleSettings);
        _board = new Board(pieceFactory, boardSize, boardSettings.Arrangement, boardSettings.WhiteOrBlackMove, _cells);

        _board.Select += OnSelect;
        _board.Move += OnMove;
        _board.Check += OnCheck;
        _board.Mate += OnMate;

        foreach (var cell in _cells)
        {
            cell.OnClick += OnCellClick;
        }
    }

    public void OnCellClick(CellPosition position)
    {
        if(!_isMate)
            _board.CellClick(position);
    }
    public void OnSelect(CellPosition position, List<CellPosition> possibleMoves)
    {
        _boardView.ClearCellsOutline();
        bool moveOrAttack;
        foreach (var possibleMove in possibleMoves)
        {
            moveOrAttack = _cells[possibleMove.Row, possibleMove.Column].GetPiece() == null;
            _boardView.SetCellOutline(possibleMove, moveOrAttack);
        }
    }
    public void OnMove(CellPosition from, CellPosition to)
    {
        _boardView.ClearCellsOutline();
        _boardView.ClearCellsColor();
    }
    public void OnCheck(CellPosition position)
    {
        _boardView.SetCellColor(position, false);
    }
    public void OnMate(CellPosition position)
    {
        _isMate = true;
        _boardView.SetCellColor(position, true);
    }
}
