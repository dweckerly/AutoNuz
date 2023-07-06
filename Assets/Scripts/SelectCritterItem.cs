using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SelectCritterItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    const float SCALE_INCREASE = 1.2f;

    public CritterData critterData;
    public Image critterImage;
    public TMP_Text critterName;

    private RectTransform rectTransform;

    public delegate void OnCritterHover(CritterData critterData);
    public event OnCritterHover CritterHoverEvent;
    public event Action CritterHoverEndEvent;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse is over " + critterData.CritterName);
        rectTransform.localScale = new Vector3(SCALE_INCREASE, SCALE_INCREASE, SCALE_INCREASE);
        CritterHoverEvent?.Invoke(critterData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse is no longer on " + critterData.CritterName);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        CritterHoverEndEvent?.Invoke();
    }
}