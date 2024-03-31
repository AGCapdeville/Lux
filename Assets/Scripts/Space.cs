using System;

public class Space
{
    // Properties
    public int SpaceNumber { get; set; } // Represents the space number
    public GamePiece[] Contents { get; set; } // Represents the space number

    // Constructor
    public Space(int spaceNumber, GamePiece[] piecesToBePlacedHere = null)
    {
        SpaceNumber = spaceNumber;
        Contents = piecesToBePlacedHere;
    }
}
