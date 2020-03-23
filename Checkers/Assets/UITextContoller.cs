using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextContoller : MonoBehaviour
{
    TMPro.TMP_Text m_roundText;
    void Awake(){
        m_roundText = GetComponent<TMPro.TMP_Text>();
        GameController.OnRoundChange += ChangeRoundText;
    }

    void ChangeRoundText(CheckerColor _newColor){
        m_roundText.text = $"Round of {_newColor.ToString()}";
    }
}
