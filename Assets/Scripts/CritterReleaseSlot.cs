using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CritterReleaseSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    const string SLOT_DESCRIPTION = "Drag and drop a critter here to release them back into the wild.";
    public delegate void OnDragToRelease(CritterRosterItem rosterItem);
    public OnDragToRelease DropReleaseEvent;

    public delegate void OnHover(string message);
    public OnHover HoverEvent;
    public event Action HoverEventExit;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CritterRosterItem rosterItem = eventData.pointerDrag.GetComponent<CritterRosterItem>();
            DropReleaseEvent?.Invoke(rosterItem);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEvent?.Invoke(SLOT_DESCRIPTION);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverEventExit?.Invoke();
    }
}
