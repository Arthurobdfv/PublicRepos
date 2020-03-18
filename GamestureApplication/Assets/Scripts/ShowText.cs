using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    private bool m_isShowing;

    public void ShowTextForSeconds(float seconds){
        if(m_isShowing)
            return;
        gameObject.SetActive(true);
        StartCoroutine(TextShowing(seconds));
    }

    private IEnumerator TextShowing(float t){
        m_isShowing = true;
        float elapsedTime=0f;
        while(elapsedTime < t){
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        m_isShowing = false;
        gameObject.SetActive(false);
    }
}
