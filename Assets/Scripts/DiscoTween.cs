using UnityEngine;
using DG.Tweening;

public class DiscoTween : MonoBehaviour
{
    public float movdisco;
    public float movetime;

    void Start()
    {
        transform.DOMoveY(movdisco, movetime).SetEase(Ease.OutBack);
    }

    void Update()
    {
        // AL APRETAR BOTON "?" ACTIVA OVERLAY
    }
}
