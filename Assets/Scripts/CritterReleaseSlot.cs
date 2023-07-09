using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CritterReleaseSlot : MonoBehaviour, IDropHandler
{
    public delegate void OnDragToRelease(CritterRosterItem rosterItem);
    public OnDragToRelease DropReleaseEvent;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CritterRosterItem rosterItem = eventData.pointerDrag.GetComponent<CritterRosterItem>();
            DropReleaseEvent?.Invoke(rosterItem);
        }
    }
}
