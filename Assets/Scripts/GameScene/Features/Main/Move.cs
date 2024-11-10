[System.Serializable]
public struct Move
{
    public CellPosition From;
    public CellPosition To;
    public Move(CellPosition from = new CellPosition(), CellPosition to = new CellPosition())
    {
        From = from;
        To = to;
    }
}
