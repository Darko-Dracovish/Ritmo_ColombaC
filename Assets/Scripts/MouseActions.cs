using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

public class MouseActions : MonoBehaviour
{
    public UnityEvent onMouseOver, onMouseExit, onMouseEnter;

    private void OnMouseEnter()
    {
        onMouseEnter.Invoke();
    }

    private void OnMouseOver()
    {
        onMouseOver.Invoke();
    }

    private void OnMouseExit()
    {
        onMouseExit.Invoke();
    }


}
