using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                var checkerPiece = hit.transform.gameObject.GetComponent<CheckerPiece>();
                if(checkerPiece != null && checkerPiece.PieceColor == GameController.CurrentRound) checkerPiece.CallPieceClicked();

                var checkerTile = hit.transform.gameObject.GetComponent<CheckerTile>();
                if(checkerTile != null) checkerTile.CallOnTileClicked();
            }
        }
    }
}
