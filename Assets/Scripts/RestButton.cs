using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    const string DESCRIPTION = "Make camp and rest to heal all of your critters 50%.";
    public delegate void OnBtnHover(string message);
    public OnBtnHover BtnHoverEvent;
    public event Action BtnHoverEventEnd;
    public event Action BtnClickEvent;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        BtnClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BtnHoverEvent?.Invoke(DESCRIPTION);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BtnHoverEventEnd?.Invoke();
    }
}
