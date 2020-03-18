using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{   
    CharacterController m_controller;
    public float m_moveSpeed = 12f;
    public float m_gravityRate = -9.81f;
    Vector3 m_velocity;

    public Transform m_groundCheck;
    float m_groundDistance = .4f;
    public LayerMask m_groundMask;
    bool m_isGrounded;
    // Start is called before the first frame update
    void Awake()
    {
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ApplyGravity();
    }

    void ApplyGravity(){
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);
        if(m_isGrounded && m_velocity.y<0f){
            m_velocity.y = -2f;
        }
        m_velocity.y += m_gravityRate * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);
    }

    void MovePlayer(){
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");
        if(xDir != 0 || zDir != 0){
            Vector3 move = xDir*transform.right + zDir*transform.forward;
            m_controller.Move(move*m_moveSpeed*Time.deltaTime);
        }
    }
}
