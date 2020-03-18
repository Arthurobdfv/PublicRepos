using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]Transform m_doorClosed, m_doorOpen, m_door;
    bool doorIsOpen,doorIsChanging, playerOnPhotocell;
    [SerializeField]AudioClip[] m_doorSounds;

    public bool PlayerOnPhotocell{
        get{return playerOnPhotocell;}
        private set{
            if(value != playerOnPhotocell){
                playerOnPhotocell = value;
                DoorControl(playerOnPhotocell ? DoorCommand.OpenDoor : DoorCommand.CloseDoor);
            }
        }
    }
    void Start()
    {
        ElevatorController.OnElevatorArrived += ElevatorArrivedAtDestination;
        ClickHandler.OnButtonClick += ElevatorChangeFloorRequest;
        PhotocellController.OnPhotocellChanged += PhotocellChanged;
        doorIsOpen = false;
        doorIsChanging=false;
    }

    void ElevatorArrivedAtDestination(Floor i){
        DoorControl(DoorCommand.OpenDoor);
    }

    void ElevatorChangeFloorRequest(Floor i){
        DoorControl(DoorCommand.CloseDoor);
    }

    void PhotocellChanged(bool playerInCell){
        PlayerOnPhotocell = playerInCell;
    }

    void DoorControl(DoorCommand doorCommand){
        if(!doorIsChanging)StartCoroutine(ChangeDoor(doorCommand));        
    }

    public IEnumerator ChangeDoor (DoorCommand doorCommand){
        Transform doorDestination;
        float time = 0f;
        switch (doorCommand){
            case DoorCommand.CloseDoor:
            doorDestination = m_doorClosed;
            doorIsOpen = false;
            break;
            case DoorCommand.OpenDoor:
            doorDestination = m_doorOpen;
            doorIsOpen = true;
            break;
            default:
            yield break;
        }
        AudioManager.PlayFromRandomClips(m_doorSounds);
        while(time < 1){
            doorIsChanging = true;
            m_door.position = Vector3.Lerp(m_door.position, doorDestination.position, time);
            time += Time.deltaTime;
            Mathf.Clamp(time,0f,1f);
            yield return null;
        }
        doorIsChanging = false;
    }
}

public enum DoorCommand{
    OpenDoor,
    CloseDoor
}
