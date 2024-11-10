using UnityEngine;

public class BoardView: MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Transform boardTransform;

    [SerializeField] private Vector2 _upperLeftPoint;
    [SerializeField] private Vector2 _lowerRightPoint;

    private Vector2Int _boardSize;
    private Vector2 screenSize;
    private BoardStyleSettings _boardStyleSettings;
    public void Init(Vector2Int boardSize, BoardStyleSettings boardStyleSettings)
    {
        _boardSize = boardSize;
        _boardStyleSettings = boardStyleSettings;

        screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
    }
    public Cell[,] ÑreateBoard()
    {
        Cell[,] board = new Cell[_boardSize.x, _boardSize.y];

        float midX = (_upperLeftPoint.x + _lowerRightPoint.x) / 2f * screenSize.x;
        float midY = (_upperLeftPoint.y + _lowerRightPoint.y) / 2f * screenSize.y;

        float boardWidth = Mathf.Abs(_lowerRightPoint.x - _upperLeftPoint.x) * screenSize.x;
        float boardHeight = Mathf.Abs(_upperLeftPoint.y - _lowerRightPoint.y) * screenSize.y;

        float cellSize = Mathf.Min(boardWidth / _boardSize.x, boardHeight / _boardSize.y);

        float offset = cellSize / 2f;
        float startX = midX - (_boardSize.x * cellSize) / 2f + offset;
        float startY = midY + (_boardSize.y * cellSize) / 2f - offset;
        Vector2 startPosition = new Vector2(startX, startY);

        for (int i = 0; i < _boardSize.x; i++)
        {
            for(int j = 0; j < _boardSize.y; j++)
            {
                board[i,j] = CreateCell(startPosition, cellSize, i, j);
            }
        }
        return board;
    }
    private Cell CreateCell(Vector2 startPosition, float cellSize, int row, int column)
    {
        Cell cell = Instantiate(_cellPrefab, boardTransform.transform);

        float positionX = startPosition.x + (column * cellSize);
        float positionY = startPosition.y - (row * cellSize);
        Vector2 cellPosition = new Vector2(positionX / screenSize.x, positionY / screenSize.y);

        Color cellColor;
        Color indexColor;
        if ((row + column) % 2 == 0)
        {
            cellColor = _boardStyleSettings.WhiteColor;
            indexColor = _boardStyleSettings.BlackColor;
        }
        else
        {
            cellColor = _boardStyleSettings.BlackColor;
            indexColor = _boardStyleSettings.WhiteColor;
        }

        string rowIndex = null;
        string columnIndex = null;
        if (column == _boardSize.x - 1)
        {
            int index = _boardSize.y - row;
            rowIndex = index.ToString();
        }
        if (row == _boardSize.y - 1)
        {
            char index = (char)('a' + column);
            columnIndex = index.ToString();
        }

        cell.Init(new CellPosition(row, column));
        cell.DrawCell(_boardStyleSettings, cellPosition, cellSize, cellColor, columnIndex, rowIndex, indexColor);
      
        return cell;
    }
}
