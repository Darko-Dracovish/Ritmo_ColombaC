using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.1f;
    public float duration = 0.15f;

    private Vector3 originalScale;
    private bool isUI;

    void Start()
    {
        originalScale = transform.localScale;
        isUI = GetComponent<RectTransform>() != null;
    }

    void OnMouseEnter()
    {
        if (isUI) return;
        if (UIBlocker.isBlocking) return;
        ScaleUp();
    }

    void OnMouseExit()
    {
        if (isUI) return;
        ScaleDown();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isUI) return;
        if (UIBlocker.isBlocking) return;
        ScaleUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isUI) return;
        ScaleDown();
    }

    void ScaleUp()
    {
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutBack).SetLink(gameObject);
    }

    void ScaleDown()
    {
        transform.DOKill();
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack).SetLink(gameObject);
    }
}
