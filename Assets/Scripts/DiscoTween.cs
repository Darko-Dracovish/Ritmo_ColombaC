using UnityEngine;
using DG.Tweening;

public class DiscoTween : MonoBehaviour
{
    public float movdisco;
    public float movetime;

    void Start()
    {
        transform.DOMoveY(movdisco, movetime).SetEase(Ease.OutBack);
        // Llamar cuando se activen los settings
    }

    void Update()
    {
        // Al apretar botón "?" activa overlay
    }
}
