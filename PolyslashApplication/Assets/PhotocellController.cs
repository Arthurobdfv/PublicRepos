using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotocellController : MonoBehaviour
{
    void OnTriggerEnter(Collider col){
        if(col.CompareTag("Player")) CallOnPhotocellChange(true);
    }

    void OntriggerExit(Collider col){
        if(col.CompareTag("Player")) CallOnPhotocellChange(false);
    }

    public delegate void PhotocellChangedEvent(bool playerIn);
    public static event PhotocellChangedEvent OnPhotocellChanged;

    private void CallOnPhotocellChange(bool playerIn){
        if(OnPhotocellChanged!=null)OnPhotocellChanged(playerIn);
    }
}
