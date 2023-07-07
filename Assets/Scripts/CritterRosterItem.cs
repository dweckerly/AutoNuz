using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CritterRosterItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Critter critter;
    public Image image;
    public TMP_Text level;
    public RectTransform healthBar;
    public TMP_Text healthText;

    public Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    Vector2 originalPosition;

    public CritterRosterSlot critterRosterSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void PopulateDetails(Critter _critter)
    {
        critter = _critter;
        image.sprite = critter.data.CritterSprite;
        level.text = critter.Level.ToString();
        healthBar.localScale = new Vector3((float)critter.currentHp / (float)critter.Hp, 1f, 1f);
        healthText.text = critter.currentHp + "/" + critter.Hp;
    }

    public void SetSlot(CritterRosterSlot slot)
    {
        critterRosterSlot = slot;
        slot.critterRosterItem = this;
        rectTransform.anchoredPosition = slot.rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPosition;
    }
}
