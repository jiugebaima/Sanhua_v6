using UnityEngine;

/// <summary>
/// 落点控制器 - 使用射线检测处理卡牌放置逻辑
/// </summary>
public class DropController
{
    private CardComponent _cardComponent;
    private Camera _camera;
    private LayerMask _cardLayerMask;

    public DropController(CardComponent cardComponent, Camera camera, LayerMask cardLayerMask)
    {
        _cardComponent = cardComponent;
        _camera = camera;
        _cardLayerMask = cardLayerMask;
    }

    /// <summary>
    /// 完整的放置流程，true堆叠到对象，false堆叠到空地
    /// </summary>
    public CardComponent ProcessDrop(Vector3 screenPos)
    {
        // 1. 射线检测目标卡片
        CardComponent targetCard = DetectTargetCard(screenPos);

        // 2. 拖到空地
        if (targetCard == null)
            return null;

        // 3. 拖到目标卡片上
        return targetCard;
    }

    /// <summary>
    /// 射线检测鼠标下方的目标卡片
    /// </summary>
    private CardComponent DetectTargetCard(Vector3 screenPos)
    {
        Vector2 rayOrigin = _camera.ScreenToWorldPoint(screenPos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.zero, 0f, _cardLayerMask);

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            CardComponent card = hit.collider.GetComponent<CardComponent>();
            if (card == null || card == _cardComponent) continue;
            return card;
        }
        return null;
    }
}

public class DropResult
{
    public bool isDrop;
    public CardComponent targetCard;

    public DropResult(bool isDrop, CardComponent targetCard)
    {
        this.isDrop = isDrop;
        this.targetCard = targetCard;
    }
}