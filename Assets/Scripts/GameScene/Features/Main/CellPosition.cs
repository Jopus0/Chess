[System.Serializable]
public struct CellPosition
{
    public int Row;
    public int Column;
    public CellPosition(int row = 0, int column = 0)
    {
        Row = row;
        Column = column;
    }
}
