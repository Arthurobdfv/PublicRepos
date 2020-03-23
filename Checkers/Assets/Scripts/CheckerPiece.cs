using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckerPiece : MonoBehaviour
{
    private CheckerColor m_pieceColor;
    private bool m_isKing;

    public CheckerColor PieceColor{
        get{return m_pieceColor;}
    }

    [SerializeField]
    private Renderer m_thisRenderer;

    [SerializeField]
    private Material m_blackMat,m_whiteMat;

    public bool IsKing{
        get{return m_isKing;}
        set{
            m_isKing = value;
            if(value == true){
                OnTurnedKing(this, new TurnKingEventArgs(PieceColor,gameObject));
            }
        }
    }

    public void TurnKing(){
        IsKing = true;
    }

    public IEnumerator MoveTo(CheckerTile _tile, Action _reachedTileCallback)
    {
        OnMovingStarted();
        float t = 0f;
        var startPos = transform.position;
        var endPos = new Vector3(_tile.transform.position.x, transform.position.y, _tile.transform.position.z);
        while(t < 1f)
        {
            transform.position = Vector3.Lerp(startPos,endPos,t); 
            t+= Time.deltaTime;
            yield return null;
        }
        _reachedTileCallback.Invoke();
    }

    public void SetColor(CheckerColor _pieceColor){
        m_pieceColor = _pieceColor;
        if(_pieceColor == CheckerColor.Black){
            m_thisRenderer.material = m_blackMat;
        }
        else{
            m_thisRenderer.material = m_whiteMat;
        }
    }

    public void CallPieceClicked(){
        OnPieceClicked();
    }

    public delegate void StartedMoving();

    public event StartedMoving OnMovingStarted;

    public delegate void TurnedKing(object sender, TurnKingEventArgs args);

    public static event TurnedKing OnTurnedKing;

    public delegate void PieceClicked();

    public event PieceClicked OnPieceClicked; 
}
