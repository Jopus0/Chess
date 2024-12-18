using System;
using UnityEngine;
public class Cell : MonoBehaviour
{
    public CellView CellView;

    private CellPosition _position;
    private Piece _piece;

    public event Action<CellPosition> OnClick;
    public void Init(CellPosition position)
    {
        _position = position;
        CellView.Init();
    }
    public Piece GetPiece()
    {
        return _piece;
    }
    public void ChangePiece(Piece piece)
    {
        _piece = piece;
        if (piece == null)
        {
            CellView.ChangePiece(null);
            return;
        }
        CellView.ChangePiece(piece.PieceSprite);
    }
    private void OnMouseDown()
    {
        OnClick?.Invoke(_position);
    }
    private void OnMouseEnter()
    {
        CellView.Highlight(true);
    }
    private void OnMouseExit()
    {
        CellView.Highlight(false);
    }
}
