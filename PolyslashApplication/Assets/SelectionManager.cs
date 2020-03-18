using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string m_interactibleTag = "Interactible";
    public Transform m_selection;

    private ISelectionResponse m_response;

    private void Awake(){
        m_response = GetComponent<ISelectionResponse>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_selection != null && m_selection.CompareTag(m_interactibleTag)){
           m_response.OnDeselect(m_selection);
        }
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit)){
            m_selection = hit.transform;
        }
        if(m_selection != null && m_selection.CompareTag(m_interactibleTag)){
            m_response.OnSelect(m_selection);
        }

        if(Input.GetMouseButtonDown(0)){
            var interaction = m_selection.GetComponent<IInteractible>();
            if(interaction!=null){
                interaction.OnClick();
            }
        }

    }


}
