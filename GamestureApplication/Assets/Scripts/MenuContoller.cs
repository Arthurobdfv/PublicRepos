using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContoller : MonoBehaviour
{
    public AudioClip[] m_audioClips;

    public ShowText m_textToShow;
    private Coroutine m_textShow;
    public float m_timeToDisplay;

    public GameObject m_popupBox;
    private GameObject m_popupInstance;

    public GameObject m_canvas;

    // Start is called before the first frame update
    void Start()
    {  
       m_timeToDisplay = 3f; 
    }

    public void AudioButtonClick(){
        if(m_audioClips.Length == 0){
            Debug.LogWarning(string.Format("No audioclips on {0}", gameObject.name));
            return;
        }
        AudioManager.PlayFromRandomClips(m_audioClips);
    }

    public void ShowTextClick(){
        m_textToShow.ShowTextForSeconds(3f);
    }


    public void ShowPopup(){
        m_popupInstance = Instantiate(m_popupBox,m_canvas.transform);
    }
}
