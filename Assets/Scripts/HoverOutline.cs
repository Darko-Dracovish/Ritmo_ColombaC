using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    public Color outlineColor = Color.white;
    public float outlineScale = 1.08f;

    private SpriteRenderer outlineRenderer;

    void Start()
    {
        SpriteRenderer original = GetComponent<SpriteRenderer>();
        if (original == null) return;

        GameObject outline = new GameObject("Outline");
        outline.transform.SetParent(transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localScale = Vector3.one * outlineScale;

        outlineRenderer = outline.gameObject.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = original.sprite;
        outlineRenderer.color = outlineColor;
        outlineRenderer.sortingLayerName = original.sortingLayerName;
        outlineRenderer.sortingOrder = original.sortingOrder - 1;
        outlineRenderer.enabled = false;
    }

    void OnMouseEnter()
    {
        if (SettingsPanel.isOpen) return;
        if (outlineRenderer != null) outlineRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        if (outlineRenderer != null) outlineRenderer.enabled = false;
    }
}
