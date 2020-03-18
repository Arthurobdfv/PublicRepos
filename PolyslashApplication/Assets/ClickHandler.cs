using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour, IInteractible
{
    [SerializeField] private Floor m_floorReference;
    public void OnClick(){
        CallOnButtonClick(m_floorReference);
    }

    public delegate void ButtonClicked(Floor floorRef);

    public static event ButtonClicked OnButtonClick;

    private void CallOnButtonClick(Floor floorRef){
        if(OnButtonClick!=null) OnButtonClick(floorRef);
    }
}
