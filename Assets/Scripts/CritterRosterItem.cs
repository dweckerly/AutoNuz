using System;
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
    public RectTransform xpBar;

    public Canvas canvas;
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    Vector2 originalPosition;

    public event Action OnDragEvent;
    public event Action OnDragEventEnd;

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
        xpBar.localScale = new Vector3((float)critter.Xp / (float)critter.neededXp, 1f, 1f);
    }

    public void SetAnchoredPosition(Vector2 pos)
    {
        rectTransform.anchoredPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        OnDragEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        ResetPosition();
        OnDragEventEnd?.Invoke();
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}
