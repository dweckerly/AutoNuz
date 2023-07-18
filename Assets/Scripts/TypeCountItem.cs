using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TypeCountItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ElementalType type;
    public Image icon;
    public TMP_Text count;

    public delegate void OnTypeCountHover(ElementalType type);
    public OnTypeCountHover TypeCountHoverEvent;
    public event Action TypeCountHoverExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TypeCountHoverEvent.Invoke(type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TypeCountHoverExitEvent.Invoke();
    }
}
