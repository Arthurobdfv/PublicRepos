using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform m_firstFloor,m_secondFloor,m_thirdFloor;
    public GameObject m_elevator;
    public float m_elevatorSpeed;
    public Floor m_elevatorFloor;
    private bool m_elevatorMoving;

    void Start(){
        ClickHandler.OnButtonClick += MoveElevatorTo;
    }

    public void MoveElevatorTo(Floor floorRequest){
        if(!m_elevatorMoving && floorRequest != m_elevatorFloor){
            StartCoroutine(MoveElevator(floorRequest));
        }
        if(m_elevatorFloor == floorRequest){
            CallElevatorArrived(floorRequest);
        }
    }

    public IEnumerator MoveElevator(Floor floor){
        Transform destination;
        switch(floor){
            case Floor.FirstFloor:
                destination = m_firstFloor;
            break;
            case Floor.SecondFloor:
                destination = m_secondFloor;
            break;
            case Floor.ThirdFloor:
                destination = m_thirdFloor;
            break;
            default:
            yield break;
        }
        m_elevatorMoving = true;
        float time = 0f;
        while(time < 1){
            m_elevator.transform.position = Vector3.Lerp(m_elevator.transform.position, destination.position, time);
            time += Time.deltaTime * 1/m_elevatorSpeed;
            Mathf.Clamp(time,0f,1f);
            yield return null;
        }
        m_elevatorFloor = floor;
        m_elevatorMoving = false;
        CallElevatorArrived(floor);
    }

    public delegate void ElevatorArrived(Floor floor);

    public static event ElevatorArrived OnElevatorArrived;

    void CallElevatorArrived(Floor floor){
        if(OnElevatorArrived != null)OnElevatorArrived(floor);
    }
}

public enum Floor{
    FirstFloor,
    SecondFloor,
    ThirdFloor
}
