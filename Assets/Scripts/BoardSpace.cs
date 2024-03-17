using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoardSpace
{
    // Properties
    public int SpaceNumber { get; set; } // Represents the space number
    public GamePiece[] Contents { get; set; } // Represents the space number

    // Constructor
    public BoardSpace(int spaceNumber, GamePiece[] piecesToBePlacedHere)
    {
        SpaceNumber = spaceNumber;
        Contents = piecesToBePlacedHere;
    }
}
