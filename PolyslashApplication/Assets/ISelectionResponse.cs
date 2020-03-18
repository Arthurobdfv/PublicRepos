using UnityEngine;

internal interface ISelectionResponse
{
    void OnSelect(Transform selection);
    void OnDeselect(Transform selection);
}

internal interface IInteractible
{
    void OnClick();
}