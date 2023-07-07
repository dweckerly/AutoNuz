using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CritterRosterSlot : MonoBehaviour, IDropHandler
{
    public RectTransform rectTransform;
    public CritterRosterItem critterRosterItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop: " + gameObject.name);
        if (eventData.pointerDrag != null)
        {
            CritterRosterItem rosterItem = eventData.pointerDrag.GetComponent<CritterRosterItem>();
            if (critterRosterItem != null) 
            {
                CritterRosterSlot slot = critterRosterItem.critterRosterSlot;
                critterRosterItem.SetSlot(rosterItem.critterRosterSlot);
                rosterItem.SetSlot(slot);
                critterRosterItem = rosterItem;
                
            }
        }
    }
}
