using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using TMPro;
/// <summary>
/// 卡牌UI处理器 - 管理所有视觉和交互相关的逻辑
/// </summary>
[System.Serializable]
public class CardUIHandler
{
    [SerializeField] public TMP_Text tagText;
    [SerializeField] public TMP_Text nameText;


    private CardComponent _selfCardComponent;
    private Transform _transform => _selfCardComponent.transform;
    private SortingGroup _sortingGroup =>_selfCardComponent.sortingGroup;
    private Vector3 _originalScale => new Vector3(1,1,1);
    private Camera _mainCamera => Camera.main;


    private Tweener _moveTweener;   // 移动动画（支持 ChangeEndValue）
    private Tween _scaleTween;      // 缩放动画（包含可能的 Sequence）

    private Vector3 _targetPosition;
    public Vector3 TargetPosition => _targetPosition;

    public bool IsDragging { get; private set; }

    public CardUIHandler(CardComponent cp)
    {
        _selfCardComponent = cp;
        _sortingGroup.sortingOrder = 0;
    }

    public void BeginDrag()
    {
        if (IsDragging) return;
        IsDragging = true;

        _scaleTween?.Kill();
        _scaleTween = _transform.DOScale(_originalScale * 1.1f, 0.15f).SetEase(Ease.OutBack);
    }

    public void UpdateDrag(Vector3 screenPosition)
    {
        if (!IsDragging) return;

        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, 10f)
        );
        MoveTo(worldPos, 0.05f); // 快速跟随鼠标
    }

    public void EndDrag()
    {
        if (!IsDragging) return;
        IsDragging = false;

        // 如果缩放动画正在播放，等待它完成后再执行回弹
        if (_scaleTween != null && _scaleTween.IsPlaying())
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_scaleTween);
            seq.Append(_transform.DOScale(_originalScale, 0.2f).SetEase(Ease.OutBack));
            _scaleTween = seq;  // 替换为新的 Sequence
        }
        else
        {
            _scaleTween?.Kill();
            _scaleTween = _transform.DOScale(_originalScale, 0.2f).SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 移动到目标世界坐标（仅修改 xy，z 保持不变）。
    /// </summary>
    /// <param name="targetWorldPos">目标世界坐标</param>
    /// <param name="duration">移动持续时间（秒）</param>
    /// <param name="needKill">是否强制杀死当前动画并重新创建（默认 false）</param>
    public void MoveTo(Vector3 targetWorldPos, float duration = 0.01f, bool needKill = false)
    {
        // 只使用目标的 XY，保持当前 Z 不变
        Vector3 targetPos = new Vector3(targetWorldPos.x, targetWorldPos.y, _transform.position.z);

        // 始终更新目标位置记录
        _targetPosition = targetPos;

        if (needKill)
        {
            _moveTweener?.Kill();
            _moveTweener = _transform.DOMove(targetPos, duration)
                                     .SetEase(Ease.Linear)
                                     .SetAutoKill(false);
        }
        else
        {
            if (_moveTweener != null && _moveTweener.IsPlaying())
            {
                _moveTweener.ChangeEndValue(targetPos, duration, true);
            }
            else
            {
                _moveTweener?.Kill();
                _moveTweener = _transform.DOMove(targetPos, duration)
                                         .SetEase(Ease.Linear)
                                         .SetAutoKill(false);
            }
        }
    }

    public void SetSortingOrder(int order)
    {
        _sortingGroup.sortingOrder = order;
    }

    public int GetSortingOrder()
    {
        return _sortingGroup.sortingOrder;
    }

    public void ResetSortingOrder()
    {
        _sortingGroup.sortingOrder = 0;
    }

    public void DOTweenCleanup()
    {
        _moveTweener?.Kill();
        _scaleTween?.Kill();
    }

    public void updateText()
    {
        // Debug.Log("改值");
        switch (_selfCardComponent.baseCardData.cardTag)
        {
            case CardTag.Agent:
                
                tagText.text = "agent";
                break;

            case CardTag.Match:
                tagText.text = "match";
                break;

            case CardTag.Mission:
                tagText.text = "mission";
                break;

            case CardTag.Resource_1:
                tagText.text = "resource_1";
                break;

            case CardTag.Resource_2:
                tagText.text = "resource_2";
                break;

            case CardTag.Resource_3:
                tagText.text = "resource_3";
                break;

            default:
                tagText.text = "unknown";
                break;
        }

        nameText.text = _selfCardComponent.baseCardData.name;
        

    }

    public void setSelf(CardComponent cp)
    {
        _selfCardComponent = cp;
    }

}