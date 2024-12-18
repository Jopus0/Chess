using UnityEngine;

public class BoardView: MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Transform boardTransform;

    [SerializeField] private Vector2 _upperLeftPoint;
    [SerializeField] private Vector2 _lowerRightPoint;

    [SerializeField] private float _baseCellSize;

    private Cell[,] _cells;
    private Vector2Int _boardSize;
    private Vector2 _screenSize;
    private BoardStyleSettings _boardStyleSettings;
    public void Init(Vector2Int boardSize, BoardStyleSettings boardStyleSettings)
    {
        _boardSize = boardSize;
        _boardStyleSettings = boardStyleSettings;

        _screenSize = new Vector2(Screen.width, Screen.height);
    }
    public Cell[,] ÑreateBoard()
    {
        _cells = new Cell[_boardSize.x, _boardSize.y];

        float midX = (_upperLeftPoint.x + _lowerRightPoint.x) / 2f * _screenSize.x;
        float midY = (_upperLeftPoint.y + _lowerRightPoint.y) / 2f * _screenSize.y;

        float boardWidth = Mathf.Abs(_lowerRightPoint.x - _upperLeftPoint.x) * _screenSize.x;
        float boardHeight = Mathf.Abs(_upperLeftPoint.y - _lowerRightPoint.y) * _screenSize.y;

        float cellSize = Mathf.Min(boardWidth / _boardSize.x, boardHeight / _boardSize.y);
        float fieldRation = Mathf.Max(_lowerRightPoint.x - _upperLeftPoint.x, _upperLeftPoint.y - _lowerRightPoint.y);
        float baseCellSize = _baseCellSize * fieldRation;

        float offset = cellSize / 2f;
        float startX = midX - (_boardSize.x * cellSize) / 2f + offset;
        float startY = midY + (_boardSize.y * cellSize) / 2f - offset;
        Vector2 startPosition = new Vector2(startX, startY);

        for (int i = 0; i < _boardSize.x; i++)
            for(int j = 0; j < _boardSize.y; j++)
                _cells[i,j] = CreateCell(startPosition, cellSize, baseCellSize, i, j);

        return _cells;
    }
    private Cell CreateCell(Vector2 startPosition, float cellSize, float baseCellSize, int row, int column)
    {
        Cell cell = Instantiate(_cellPrefab, boardTransform.transform);

        float positionX = startPosition.x + (column * cellSize);
        float positionY = startPosition.y - (row * cellSize);
        Vector2 cellPosition = new Vector2(positionX / _screenSize.x, positionY / _screenSize.y);

        bool isWhiteOrBlack = (row + column) % 2 == 0;
        Color cellColor = isWhiteOrBlack ? _boardStyleSettings.WhiteColor : _boardStyleSettings.BlackColor;
        Color indexColor = isWhiteOrBlack ? _boardStyleSettings.BlackColor : _boardStyleSettings.WhiteColor;

        string rowIndex = column == _boardSize.x - 1 ? (_boardSize.y - row).ToString() : null;
        string columnIndex = row == _boardSize.y - 1 ? ((char)('a' + column)).ToString() : null;

        cell.Init(new CellPosition(row, column));
        cell.CellView.SetPosition(cellPosition, baseCellSize);
        cell.CellView.SetBaseColor(cellColor);
        if(rowIndex != null)
            cell.CellView.SetRowIndex(rowIndex, indexColor);
        if (columnIndex != null)
            cell.CellView.SetColumnIndex(columnIndex, indexColor);

        return cell;
    }
    public void SetCellColor(CellPosition position, bool mateOrCheck)
    {
        Color color = mateOrCheck ? _boardStyleSettings.MateCellColor : _boardStyleSettings.CheckCellColor;
        _cells[position.Row, position.Column].CellView.SetColor(color);
    }
    public void SetCellOutline(CellPosition position, bool moveOrAttack)
    {
        Sprite outlineSprite = moveOrAttack ? _boardStyleSettings.MoveOutlineSprite : _boardStyleSettings.AttackOutlineSprite;
        Color outlineColor = moveOrAttack ? _boardStyleSettings.MoveOutlineColor : _boardStyleSettings.AttackOutlineColor;
        _cells[position.Row, position.Column].CellView.SetOutline(outlineSprite, outlineColor);
    }
    public void ClearCellsOutline()
    {
        foreach (var cell in _cells)
        {
            cell.CellView.ClearOutline();
        }
    }    
    public void ClearCellsColor()
    {
        foreach (var cell in _cells)
        {
            cell.CellView.ClearColor();
        }
    }
}
