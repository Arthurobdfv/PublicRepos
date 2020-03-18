using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBg : MonoBehaviour
{

    float m_length,m_startPos;
    GameObject m_camera;
    public float m_parllaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main.gameObject;
        m_startPos = transform.position.x;
        m_length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var temp = (m_camera.transform.position.x * (1- m_parllaxEffect));
        var dist = (m_camera.transform.position.x * m_parllaxEffect);

        transform.position = new Vector3(m_startPos + dist, transform.position.y,transform.position.z);

        if(temp > m_startPos+m_length) m_startPos += m_length;
        else if(temp < m_startPos - m_length) m_startPos -= m_length;
    }
}
