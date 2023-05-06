using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 绑定在按钮上，实现相关动效
/// </summary>
public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isUp = true;

    //[SerializeField] private float speed;
    private float speed = 0.8f;
    private float scale = 1.2f;
    //[SerializeField] private Ease ease;
    private Vector3 orignScale;
    private void Awake()
    {
        orignScale = transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(orignScale * scale, speed).SetEase(Ease.OutElastic).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(orignScale, speed).SetEase(Ease.OutElastic).SetUpdate(true);
    }
}
