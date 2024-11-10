using UnityEngine;
public class BoardContoller : MonoBehaviour 
{
    [SerializeField] private BoardView _boardView;
    [SerializeField] private Board _board;

    public void Init(BoardSettings boardSettings, BoardStyleSettings boardStyleSettings)
    {
        Vector2Int boardSize = new Vector2Int(boardSettings.BoardHeight, boardSettings.BoardWidth);

        _boardView.Init(boardSize, boardStyleSettings);

        PieceFactory pieceFactory = new PieceFactory(boardStyleSettings);
        _board = new Board(pieceFactory, boardSize, boardSettings.Arrangement, boardSettings.WhiteOrBlackMove, _boardView.ÑreateBoard());
    }
}
