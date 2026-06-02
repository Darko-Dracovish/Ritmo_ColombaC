using UnityEngine;
using DG.Tweening;

public class DiscoTween : MonoBehaviour
{
    public float movdisco;
    public float movetime;
    void Start()
    {
        transform.DOMoveY(movdisco, movetime).SetEase(Ease.OutBack);
        //DEBE SER CUANDO SE LLAME A LOS SETTINGS, LLEVA A CAMARA AJUSTES Y ENCIMA EL DISCO CON AJUSTES
    }

    // Update is called once per frame
    void Update()
    {
        //AL APRETAR BOTON "?" ACTIVA OVERLAY, PLANO + OSCURO PARA DIFERENCIAR DEL FONDO Y ENCIMA LA ANIM
    }
}
