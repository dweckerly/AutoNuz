using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CritterRosterSlot : MonoBehaviour, IDropHandler
{
    public RectTransform rectTransform;
    public CritterRosterItem critterRosterItem;
    public delegate void OnCritterSwap(Critter c1, Critter c2);
    public OnCritterSwap CritterSwapEvent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CritterRosterItem rosterItem = eventData.pointerDrag.GetComponent<CritterRosterItem>();
            if (critterRosterItem != null) 
            {
                CritterSwapEvent?.Invoke(rosterItem.critter, critterRosterItem.critter);
            }
            else
            {
                rosterItem.ResetPosition();
            }
        }
    }
}
