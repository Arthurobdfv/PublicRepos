using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBox : MonoBehaviour
{

    private Animator m_thisAnim;
    // Start is called before the first frame update
    void Start()
    {
        m_thisAnim = GetComponent<Animator>();
    }

    public void OnCloseClick(){
        m_thisAnim.SetTrigger("OnClose");
    }

    public void DestroyAfterAnimation(){
        Destroy(gameObject);
    }
}
