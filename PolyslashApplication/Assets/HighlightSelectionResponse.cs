using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HighlightSelectionResponse : MonoBehaviour, ISelectionResponse
{
    public Material m_highlightMaterial;
    private Material m_defaultMaterial;

    public void OnDeselect(Transform selection){
        var selectionRenderer = selection.GetComponent<Renderer>();
        if(selectionRenderer != null){
            selectionRenderer.material = m_defaultMaterial;
        }
    }

    public void OnSelect(Transform selection){
        var selectionRenderer = selection.GetComponent<Renderer>();
        m_defaultMaterial = selectionRenderer.material;
        if(selectionRenderer != null){
            selectionRenderer.material = m_highlightMaterial;
        }
    }
}

