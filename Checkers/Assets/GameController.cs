using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_tilePrefab,m_piecePrefab;
    public int m_boardSize = 8;

    public CheckerTile [,] m_tiles;

    public Material m_evenMat, m_unevenMat;

    private CheckerPiece m_selectedPiece;
    private CheckerTile m_selectedTile;
    private List<CheckerTile> m_diagonalMoves = new List<CheckerTile>();

    public CheckerPiece SelectedPiece{
        get{return m_selectedPiece;}
        set{
            m_selectedPiece = value;
            DiagonalMoves = value == null ? new List<CheckerTile>() : GetDiagonals(SelectedTile);
        }
    }
    public CheckerTile SelectedTile{
        get{return m_selectedTile;}
        private set{
            m_selectedTile = value;
            SelectedPiece = value == null ? null : m_selectedTile.Piece;
        }
    }
    public List<CheckerTile> DiagonalMoves{
        get{return m_diagonalMoves;}
        set{
            UpdateEvents(value);
            HighlightPossibleMoves(false);
            m_diagonalMoves = value;
            HighlightPossibleMoves(true);
        }
    }
    private static CheckerColor m_currentRound;

    public static CheckerColor CurrentRound{
        get{return m_currentRound;}
        private set{
            m_currentRound = value;
            OnRoundChange(value);
        }
    }

    public List<GameObject> m_whiteTeam;
    public List<GameObject> m_blackTeam;


    // Start is called before the first frame update
    void Start()
    {
        CurrentRound = CheckerColor.White;
        CheckerTile.OnHoldingPieceClicked += PieceClickCommand;
        GenerateTiles();
    }

    void GenerateTiles(){
        m_tiles = new CheckerTile[m_boardSize,m_boardSize];
        for(int i=0; i< m_boardSize; i++){
            for(int j=0; j< m_boardSize; j++){
                var spawnPos = new Vector3(i,0,j);
                m_tiles[i,j] = Instantiate(m_tilePrefab,spawnPos,Quaternion.identity).GetComponent<CheckerTile>();
                m_tiles[i,j].SetMaterial(((i+j) % 2) == 0 ? m_evenMat : m_unevenMat);
                if(GameInfos.CheckerPieces[i,j] != (int)CheckerColor.None){
                    var _pieceGO = Instantiate(m_piecePrefab,new Vector3(i,.2f,j),Quaternion.identity);
                    m_tiles[i,j].Piece = _pieceGO.GetComponent<CheckerPiece>();
                    m_tiles[i,j].Piece.SetColor((CheckerColor)GameInfos.CheckerPieces[i,j]);
                }
            }
        }
    }

    public delegate void RoundChange(CheckerColor _newRound);

    public static event RoundChange OnRoundChange;

    void PieceClickCommand(CheckerTile _tile){
        SelectedTile = _tile;
    }

    void TileClickedToMove(CheckerTile _tile){
        foreach(var dg in DiagonalMoves){
            dg.Highlight(false);
        }
        StartCoroutine(SelectedPiece.MoveTo(_tile, () => {
            Debug.Log("Piece Reached Destination");
            _tile.Piece = SelectedPiece;
            RoundEnd();
        }));
    }

    void RoundEnd(){
        CurrentRound = CurrentRound == CheckerColor.Black ? CheckerColor.White : CheckerColor.Black;
        SelectedPiece = null;
        SelectedTile = null;
    }

    List<CheckerTile> GetDiagonals(CheckerTile _tile){
        var diagonals = new List<CheckerTile>();
        var tileCoord = new GridPoint(_tile.transform.position);
        if(tileCoord.X + 1 <= 7){
            if(tileCoord.Z + 1 <= 7)
                diagonals.Add(m_tiles[tileCoord.X+1,tileCoord.Z+1]);
            if(tileCoord.Z - 1 >= 0)
                diagonals.Add(m_tiles[tileCoord.X+1,tileCoord.Z-1]);
        }
        if(tileCoord.X - 1 >= 0){
            if(tileCoord.Z + 1 <= 7)
                diagonals.Add(m_tiles[tileCoord.X-1,tileCoord.Z+1]);
            if(tileCoord.Z - 1 >= 0)
                diagonals.Add(m_tiles[tileCoord.X-1,tileCoord.Z-1]);
        }

        return diagonals.Where(d => d.Piece == null).ToList();
    }

    void UpdateEvents(List<CheckerTile> _newTiles){
        if(m_diagonalMoves.Count != 0)
            foreach(var diag in m_diagonalMoves){
                diag.OnTileClicked -= TileClickedToMove;
            }
        foreach(var diag in _newTiles){
            diag.OnTileClicked += TileClickedToMove;
        }
    }

    void HighlightPossibleMoves(bool _highlight){
        foreach(var dg in m_diagonalMoves){
            dg.Highlight(_highlight);
        }
    }
}

public class GridPoint{
    private int m_x,m_z;
    public int X{
        get{return m_x;}
    }

    public int Z{
        get{return m_z;}
    }

    public GridPoint(Vector3 _position)
    {
        m_x = Convert.ToInt32(_position.x);
        m_z = Convert.ToInt32(_position.z);
    }
}
