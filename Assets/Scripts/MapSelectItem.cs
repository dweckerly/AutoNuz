using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MapSelectItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    const float SCALE_INCREASE = 1.2f;
    private RectTransform rectTransform;

    public LocationData locationData;
    public Image locationImage;
    public TMP_Text locationName;

    public event Action LocationSelectEvent;
    public delegate void OnLocationHover(LocationData locationData);
    public event OnLocationHover LocationHoverEvent;
    public event Action LocationHoverEventEnd;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LocationSelectEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(SCALE_INCREASE, SCALE_INCREASE, SCALE_INCREASE);
        LocationHoverEvent?.Invoke(locationData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        LocationHoverEventEnd?.Invoke();
    }
}
