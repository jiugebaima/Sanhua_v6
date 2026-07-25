using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class CardComponent : MonoBehaviour, IDragable
{
    [Header("卡牌基础属性")]
    public BaseCardData baseCardData;

    [Header("堆叠配置")]
    [SerializeField] private float _cardOffset = -0.5f;
    [SerializeField] private float _moveDuration = 0.01f;
    [SerializeField] private float _zOffset = 0.01f;
    // 堆叠引用
    public StackController stackController { get; set; }

    public CardNodeController cardNodeController;
    //组件引用
    public SortingGroup sortingGroup;

    // UI 处理器
    [SerializeField]private CardUIHandler _uiHandler;
    private DropController _dropController;
    private RegionController _regionController;
    public bool IsDragging => _uiHandler.IsDragging;

    [SerializeField] private LayerMask _cardLayerMask = 1 << 0;



    public float CardOffset => _cardOffset;
    public float MoveDuration => _moveDuration;
    public float ZOffset => _zOffset;

    private void Awake()
    {

        sortingGroup = GetComponent<SortingGroup>();
        if (sortingGroup == null)
            sortingGroup = gameObject.AddComponent<SortingGroup>();


        if (_uiHandler == null)
        {
            _uiHandler = new CardUIHandler(this);
        }
        else
        {
            _uiHandler.setSelf(this);
        }
        _dropController = new DropController(this, Camera.main, _cardLayerMask);
        cardNodeController = new CardNodeController(this);
        _regionController = new RegionController(this);
    }

    #region  ---------- IDragable 接口 ----------

    public void OnPointerDown()
    {
        //UI层开始拖
        _uiHandler.BeginDrag();

        cardNodeController.UpdateSortOrder(100);
        cardNodeController.DisableNextCardCollider();
    }

    public void OnPointerDrag(Vector3 screenPosition)
    {
        _uiHandler.UpdateDrag(screenPosition);
        cardNodeController.UpdateChainPosition();
    }

    public void OnPointerUp(Vector3 screenPos)
    {
        //动画
        _uiHandler.EndDrag(); // end导致动画停止，待改
        // 返回命中的新卡
        CardComponent targetCard = _dropController.ProcessDrop(screenPos);

        cardNodeController.LinkToTargetCard(targetCard);
        cardNodeController.UpdateSortOrder(0);
        cardNodeController.GetHeadCard(this).cardNodeController.UpdateZPosition();//链式处理z轴
        
        cardNodeController.totheEndPosition(_uiHandler.TargetPosition);

        _regionController.DetectRegion();
        // 启用Collider
        cardNodeController.EnableChainColliders();
    }
    #endregion

    #region ---------- 位置管理 ----------

    /// <summary>
    /// 移动到目标位置
    /// </summary>
    public void MoveTo(Vector3 targetPos, float duration, bool kill = false)
    {
        _uiHandler.MoveTo(targetPos, duration);
    }
    #endregion

    public StackController CreateStackController()
    {
        stackController = new StackController(this,baseCardData);
        return stackController;
        // return null;
    }

    public void DestoryStackController()
    {
        stackController.DestoryStackController();
        stackController = null;
        // return null;
    }

    public void updateUI()
    {
        // Debug.Log("updateUI");
        _uiHandler.updateText();
    }

    public void sendSSTtoStackManager(SynthesisStatTable SST)
    {
        stackController.trySynthesis(SST);
    }
}