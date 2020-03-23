using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerTile : MonoBehaviour
{
    private CheckerPiece m_holdingPiece;


    public Shader m_highLightedShader;
    private Shader m_stdShader;
    public Renderer m_thisMat;

    public CheckerPiece Piece{
        get{return (m_holdingPiece == null)? null : m_holdingPiece;}
        set{
            m_holdingPiece = value;
            if(m_holdingPiece != null){
                m_holdingPiece.OnMovingStarted += PieceLeftTile;
                m_holdingPiece.OnPieceClicked += HoldingPieceClick;
                };
        }
    }

    void Start(){
        m_stdShader = m_thisMat.material.shader;
    }

    public void Highlight(bool _highlight){
        m_thisMat.material.shader = _highlight? m_highLightedShader : m_stdShader;
    }

    public void SetMaterial(Material _mat){
        m_thisMat.material = _mat;
        var offset = Random.Range(0f,1f);
        m_thisMat.material.SetTextureOffset("_MainTex",new Vector2(offset,offset));
        m_thisMat.material.SetTextureOffset("_BumpMap",new Vector2(offset,offset));
    }

    void HoldingPieceClick(){
        OnHoldingPieceClicked(this);
    }

    void PieceLeftTile(){
        UnsubscribeFromPiece();
        m_holdingPiece = null;
    }

    void UnsubscribeFromPiece(){
        m_holdingPiece.OnMovingStarted -= PieceLeftTile;
        m_holdingPiece.OnPieceClicked -= HoldingPieceClick;
    }

    public delegate void TileClicked(CheckerTile _tile);

    public event TileClicked OnTileClicked;

    public void CallOnTileClicked(){
        if(OnTileClicked != null)OnTileClicked(this);
    }

    public delegate void HoldingPieceClicked(CheckerTile _sender);

    public static event HoldingPieceClicked OnHoldingPieceClicked;
}
