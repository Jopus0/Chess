using Unity.VisualScripting;
using UnityEngine;
public class BoardContoller : MonoBehaviour 
{
    [SerializeField] private BoardSettings _boardSettings;
    [SerializeField] private BoardStyleSettings _boardStyleSettings;

    [SerializeField] private BoardView _boardView;
    [SerializeField] private Board _board;

    private void Start()
    {
        Vector2Int boardSize = new Vector2Int(_boardSettings.BoardHeight, _boardSettings.BoardWidth);

        _boardView.Init(boardSize, _boardStyleSettings);

        PieceFactory pieceFactory = new PieceFactory(_boardStyleSettings);
        _board = new Board(pieceFactory, boardSize, _boardSettings.Arrangement, _boardView.ÑreateBoard());
    }
}
