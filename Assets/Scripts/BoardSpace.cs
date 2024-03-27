using System;

public class BoardSpace
{
    // Properties
    public int SpaceNumber { get; set; } // Represents the space number
    public GamePiece[] Contents { get; set; } // Represents the space number

    // Constructor
    public BoardSpace(int spaceNumber, GamePiece[] piecesToBePlacedHere = null)
    {
        SpaceNumber = spaceNumber;
        Contents = piecesToBePlacedHere;
    }
}
