using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_mouseSensitivity = 100f;
    public Transform m_playerBody;
    float m_limitRotation = 90f;
    float m_xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        m_xRotation -= mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -m_limitRotation, m_limitRotation);     
        transform.localRotation = Quaternion.Euler(m_xRotation,0f,0f);
        m_playerBody.Rotate(Vector3.up * mouseX);

    }
}
