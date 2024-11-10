using System;
using UnityEngine;
public class Cell : MonoBehaviour
{   
    [SerializeField] private CellView _cellView;

    private CellPosition _position;
    private Piece _piece;

    public event Action<CellPosition> OnClick;
    public void Init(CellPosition position)
    {
        _position = position;
        _cellView.Init();
    }
    public void DrawCell(BoardStyleSettings boardStyleSettings, Vector2 position, float size, Color color, string columnIndex, string rowIndex, Color indexColor)
    {
        _cellView.SetPosition(position, size);
        _cellView.SetIndices(columnIndex, rowIndex, indexColor);
        _cellView.SetColor(color);
        _cellView.SetOutline(boardStyleSettings.MoveOutlineSprite, boardStyleSettings.AttackOutlineSprite);
        _cellView.SetOutlineColor(boardStyleSettings.MoveOutlineColor, boardStyleSettings.AttackOutlineColor);
    }
    public Piece GetPiece()
    {
        return _piece;
    }
    public void ChangePiece(Piece piece)
    {
        if(piece == null )
        {
            _cellView.ChangePiece(null);
            return;
        }
        _piece = piece;
        _cellView.ChangePiece(piece.PieceSprite);
    }
    public void ChangeOutline(bool showOrClose, bool moveOrAttack)
    {
        _cellView.ChangeOutline(showOrClose, moveOrAttack);
    }
    private void OnMouseDown()
    {
        OnClick?.Invoke(_position);
    }
    private void OnMouseEnter()
    {
        _cellView.OnHighlight(true);
    }
    private void OnMouseExit()
    {
        _cellView.OnHighlight(false);
    }
}
