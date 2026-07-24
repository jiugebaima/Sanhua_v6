using UnityEngine;

/// <summary>
/// 卡牌拖拽处理器 - 封装所有拖拽逻辑
/// </summary>
public class CardDragHandler
{
    private IDragable _currentDraggingObj;
    private LayerMask _cardLayerMask;

    public CardDragHandler(LayerMask cardLayerMask, bool debugMode = false, bool mouseDebugMode = false)
    {
        _cardLayerMask = cardLayerMask;
    }

    public bool IsDragging => _currentDraggingObj != null;

    public void OnMouseDown()
    {
        // IDragable clickedObj = RaycastForCard();
        IDragable clickedObj = RaycastForObj();

        if (clickedObj != null)
        {
            _currentDraggingObj = clickedObj;
            _currentDraggingObj.OnPointerDown();

        }
        else
        {
            //空处理待写
        }
    }

    public void OnMouseDrag()
    {
        if (_currentDraggingObj == null) return;

        _currentDraggingObj.OnPointerDrag(Input.mousePosition);
    }

    public void OnMouseUp()
    {
        if (_currentDraggingObj == null) return;

        _currentDraggingObj.OnPointerUp(Input.mousePosition);

        _currentDraggingObj = null;
    }

    /// <summary>
    /// 射线检测卡片,弃用
    /// </summary>
    private CardComponent RaycastForCard()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, _cardLayerMask);

        if (hit.collider != null)
        {
            CardComponent card = hit.collider.GetComponent<CardComponent>();


            if (card != null)
            {
                return card;
            }
        }

        return null;
    }


    ///射线检测接口
    private IDragable RaycastForObj()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, _cardLayerMask);

        if (hit.collider != null)
        {
            IDragable dragable = hit.collider.GetComponent<IDragable>();
            if (dragable != null)
            {
                return dragable;
            }
        }

        return null;
    }
}