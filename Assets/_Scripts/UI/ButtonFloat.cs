using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
/// <summary>
/// 选字漂浮
/// </summary>
public class ButtonFloat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float upDis;
    public float speed;
    private Vector3 oriPos;
    float targetY;
    float oriY;
    Tween tw;
    public void OnPointerEnter(PointerEventData eventData)
    {
        oriPos = transform.localPosition;
        targetY = oriPos.y + upDis;
        oriY = oriPos.y;
        tw =  transform.DOLocalMoveY(targetY, speed).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tw =  transform.DOLocalMoveY(oriY, speed).SetUpdate(true);
    }
}
