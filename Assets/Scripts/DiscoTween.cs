using UnityEngine;
using DG.Tweening;

public class DiscoTween : MonoBehaviour
{
    public float movdisco;
    public float movetime;

    void Start()
    {
        transform.DOLocalMoveY(movdisco, movetime).SetEase(Ease.OutBack).SetLink(gameObject);
    }

    void Update()
    {
        // AL APRETAR BOTON "?" ACTIVA OVERLAY
    }
}
