using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform m_blackViewPos, m_whiteViewPos;

    bool m_isBusy;
    public float m_moveSpeed;
    public float m_rotationSpeed;

    public float m_zoomSpeed;

    public float m_minDistance,m_maxDistance;

    public float bounds = 50f;

    public GameObject m_camera;

    Vector3 offset;

    void Start(){
        m_camera = Camera.main.gameObject;
        offset = m_camera.transform.localPosition;
    }

    void Update(){
        CameraMovement();
    }

    void CameraMovement(){
        if(m_isBusy)
            return;
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        var distance = Vector3.Distance(m_camera.transform.position,transform.position);
        if(wheelInput != 0 && distance >= m_minDistance && distance <= m_maxDistance){
            var delta = wheelInput * m_zoomSpeed;
            if(distance - delta > m_maxDistance)
                delta = m_maxDistance - distance;
            if(distance - delta < m_minDistance)
                delta = distance - m_minDistance;
            m_camera.transform.Translate(0f,0f,delta);         
        }
        if(Input.GetKey(KeyCode.Mouse2)){
            var x = - Input.GetAxis("Mouse X");
            var z = - Input.GetAxis("Mouse Y");
            var deltaX = x * m_moveSpeed * Time.deltaTime;
            var deltaZ = z* m_moveSpeed * Time.deltaTime;
            if(Mathf.Abs(transform.position.x + deltaX) > bounds )
                deltaX = 0f;
            if(Mathf.Abs(transform.position.z + deltaZ) > bounds )
                deltaZ = 0f;
            transform.Translate(x * m_moveSpeed * Time.deltaTime, 0f, z* m_moveSpeed * Time.deltaTime);
        }
        if(Input.GetMouseButton(1)){
            var x = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, Time.deltaTime * m_rotationSpeed * x);
        }
    }

    void MoveRealDistance(){

    }

    public void MoveCamera(Transform _transform){
        if(!m_isBusy) StartCoroutine(MoveCameraTo(_transform));
    }

    IEnumerator MoveCameraTo(Transform _transform){
        float t = 0f;
        m_isBusy = true;
        while(t<1){
            transform.position = Vector3.Lerp(transform.position,_transform.position,t);
            transform.rotation = Quaternion.Lerp(transform.rotation, _transform.rotation, t);
            m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, offset,t);
            t+= Time.deltaTime;
            yield return null;
        }
        m_isBusy = false;
    }
}
