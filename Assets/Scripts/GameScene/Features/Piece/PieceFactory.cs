using System.Collections.Generic;
public class PieceFactory
{
    private Dictionary<Enums.PieceType, Piece> _whitePieces;
    private Dictionary<Enums.PieceType, Piece> _blackPieces;
    public PieceFactory(BoardStyleSettings boardStyleSettings)
    {
        _whitePieces = new Dictionary<Enums.PieceType, Piece>()
        {
            { Enums.PieceType.None, null },
            { Enums.PieceType.King, new King(true, boardStyleSettings.WhiteKingSprite) },
            { Enums.PieceType.Pawn, new Pawn(true, boardStyleSettings.WhitePawnSprite) },
            { Enums.PieceType.Bishop, new Bishop(true, boardStyleSettings.WhiteBishopSprite) },
            { Enums.PieceType.Horse, new Horse(true, boardStyleSettings.WhiteHorseSprite) },
        };

        _blackPieces = new Dictionary<Enums.PieceType, Piece>()
        {
            { Enums.PieceType.None, null },
            { Enums.PieceType.King, new King(true, boardStyleSettings.BlackKingSprite) },
            { Enums.PieceType.Pawn, new Pawn(false, boardStyleSettings.BlackPawnSprite) },
            { Enums.PieceType.Bishop, new Bishop(false, boardStyleSettings.BlackBishopSprite) },
            { Enums.PieceType.Horse, new Horse(true, boardStyleSettings.BlackHorseSprite) },
        };
    }
    public Piece GetPieceByType(Enums.PieceType pieceType, bool whiteOrBlack)
    {
        if (whiteOrBlack)
        {
            return _whitePieces[pieceType];
        }
        else
        {
            return _blackPieces[pieceType];
        }
    }
}
