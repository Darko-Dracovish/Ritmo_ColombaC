using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MouseActions : MonoBehaviour
{
    public UnityEvent onMouseOver, onMouseExit, onMouseEnter;

    private void OnMouseEnter() => onMouseEnter.Invoke();
    private void OnMouseOver()  => onMouseOver.Invoke();
    private void OnMouseExit()  => onMouseExit.Invoke();
}
