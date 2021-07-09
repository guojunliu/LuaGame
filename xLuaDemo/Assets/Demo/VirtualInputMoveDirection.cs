using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

/// <summary>
/// author  :   jave.lin
/// date    :   2020.02.12
/// 虚拟摇杆 - 移动方向
/// </summary>
[LuaCallCSharp]
public class VirtualInputMoveDirection : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler,
        IEndDragHandler
{
    [Header("虚拟摇杆的底图")]
    [SerializeField] private RawImage touchBg;
    [Header("虚拟摇杆滑动方向的滑动器图")]
    [SerializeField] private RawImage touchTracker;

    [Header("无效的中间区块占比:0.0f~1.0f")]
    public float deadZone = 0.2f;

    // 限制滑动的最大半径，以背景图的宽度的一半来定
    private float limitRadius;
    // 鼠标按下时，记录背景图的世界坐标位置
    private Vector3 msDownBgWorldPos;
    private Vector2 msDownBgWorldPosHelper;

    // 重置滑动器图的alpha值
    private void ResetTrackerAlpha(float a)
    {
        var c = touchTracker.color;
        c.a = a;
        touchTracker.color = c;
    }
    // 重置滑动器图的位置
    private void ResetTrackerPos()
    {
        touchTracker.transform.localPosition = touchBg.transform.localPosition;
    }
    // start 时，以背景图的宽度的一半作为限制滑动器移动的半径距离，并重置滑动器的alpha为：0.2f
    private void Start()
    {
        limitRadius = touchBg.rectTransform.rect.width * 0.5f;
        ResetTrackerAlpha(0.2f);
    }
    // 当有鼠标、触点按下时，记录背景图的世界坐标位置
    public void OnPointerDown(PointerEventData eventData)
    {
        msDownBgWorldPos = touchBg.transform.TransformPoint(0, 0, 0);
        msDownBgWorldPosHelper.Set(msDownBgWorldPos.x, msDownBgWorldPos.y);
        ResetTrackerAlpha(1f);
    }
    // 触点单开时，重置滑动器的alpha
    public void OnPointerUp(PointerEventData eventData)
    {
        ResetTrackerAlpha(0.2f);
    }
    // 拖动时更新滑动器的位置
    public void OnDrag(PointerEventData eventData)
    {
        // 获取世界坐标下的总偏移向量
        var deltaWorldPos = eventData.position - msDownBgWorldPosHelper;
        var mag = deltaWorldPos.magnitude;
        // 超过deadZone才算作有效的位移摇杆
        if ((mag / limitRadius) > deadZone)
        {
            // 当总偏移向量模大于限制半径，则将向量模重置到半径限制的值
            if (mag > limitRadius) deltaWorldPos = deltaWorldPos.normalized * limitRadius;
            // 将总偏移的向量作为滑动器的局部坐标设置即可
            touchTracker.transform.localPosition = deltaWorldPos;
        }
        // 否则无效的位移将重置滑动器的位置
        else
        {
            ResetTrackerPos();
        }
    }
    // 拖动结束时重置滑动器图的位置
    public void OnEndDrag(PointerEventData eventData)
    {
        ResetTrackerPos();
    }
    // 获取输入的方向向量
    public Vector2 GetDirection()
    {
        return touchTracker.transform.localPosition.normalized;
    }
    // 获取输入的方向向量的模:0.0~1.0f
    public float GetLenPercent()
    {
        return (touchTracker.transform.localPosition.magnitude / limitRadius);
    }
}