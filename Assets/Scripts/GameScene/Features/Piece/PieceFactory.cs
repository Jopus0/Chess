using System;
using System.Collections.Generic;

public class PieceFactory
{
    private Dictionary<Enums.PieceType, Func<bool, Piece>> _pieceCreators;

    public PieceFactory(BoardStyleSettings boardStyleSettings)
    {
        _pieceCreators = new Dictionary<Enums.PieceType, Func<bool, Piece>>()
        {
            { Enums.PieceType.None, _ => null },
            { Enums.PieceType.King, white => new King(white, white ? boardStyleSettings.WhiteKingSprite : boardStyleSettings.BlackKingSprite) },
            { Enums.PieceType.Pawn, white => new Pawn(white, white ? boardStyleSettings.WhitePawnSprite : boardStyleSettings.BlackPawnSprite) },
            { Enums.PieceType.Bishop, white => new Bishop(white, white ? boardStyleSettings.WhiteBishopSprite : boardStyleSettings.BlackBishopSprite) },
            { Enums.PieceType.Horse, white => new Horse(white, white ? boardStyleSettings.WhiteHorseSprite : boardStyleSettings.BlackHorseSprite) },
            { Enums.PieceType.Rook, white => new Rook(white, white ? boardStyleSettings.WhiteRookSprite : boardStyleSettings.BlackRookSprite) },
            { Enums.PieceType.Queen, white => new Queen(white, white ? boardStyleSettings.WhiteQueenSprite : boardStyleSettings.BlackQueenSprite) },
        };
    }

    public Piece GetPieceByType(Enums.PieceType pieceType, bool whiteOrBlack)
    {
        if (_pieceCreators.TryGetValue(pieceType, out var creator))
        {
            return creator(whiteOrBlack);
        }
        throw new ArgumentException($"Invalid piece type: {pieceType}");
    }
}
