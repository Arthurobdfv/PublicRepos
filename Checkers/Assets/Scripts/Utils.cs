using System;
using UnityEngine;

public enum CheckerColor{
    None,
    White,
    Black,
}

public enum MoveType{
    Move,
    Capture
}


public class TurnKingEventArgs : EventArgs{
    public CheckerColor PieceColor;
    public GameObject CheckerPiece;

    public TurnKingEventArgs(CheckerColor _color, GameObject _gameObject)
    {
        PieceColor = _color;
        CheckerPiece = _gameObject;
    }
}

public class GameInfos{
    public static readonly int[,] CheckerPieces = {
        {1,0,1,0,1,0,1,0},
        {0,1,0,1,0,1,0,1},
        {1,0,1,0,1,0,1,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,2,0,2,0,2,0,2},
        {2,0,2,0,2,0,2,0},
        {0,2,0,2,0,2,0,2}
    };
}