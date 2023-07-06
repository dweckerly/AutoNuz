using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SelectCritterItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    const float SCALE_INCREASE = 1.2f;

    public Critter critter;
    public Image critterImage;
    public TMP_Text critterName;

    private RectTransform rectTransform;

    public delegate void OnCritterHover(Critter critter);
    public event OnCritterHover CritterHoverEvent;
    public event Action CritterHoverEndEvent;

    public event Action CritterSelectedEvent;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(SCALE_INCREASE, SCALE_INCREASE, SCALE_INCREASE);
        CritterHoverEvent?.Invoke(critter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        CritterHoverEndEvent?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CritterSelectedEvent?.Invoke();
    }
}
