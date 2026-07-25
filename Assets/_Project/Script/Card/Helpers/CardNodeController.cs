using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

//管理链表
public class CardNodeController
{
    //111

    public CardComponent PreviousCard { get; set; }
    public CardComponent NextCard { get; set; }

    public bool hasPreCard => PreviousCard != null;
    public bool hasNextCard => NextCard != null;

    // 是否是根卡（堆叠底部）
    public bool IsRootCard => PreviousCard == null;
    // 是否是顶部卡
    public bool IsTopCard => NextCard == null;

    private CardComponent _selfCardComponent;
    private StackController _stackController => _selfCardComponent.stackController;

    public CardNodeController(CardComponent component)
    {
        _selfCardComponent = component;
    }

    //主对外方法
    public void LinkToTargetCard(CardComponent targetCard)
    {
        //命中
        if (targetCard != null)
        {
            if (targetCard == PreviousCard)
            {//是前卡

            }
            else
            {   //另一张卡
                if (PreviousCard != null)
                {
                    UnLinkPreCard();//解绑前卡
                }
                //取最后一张卡
                CardComponent endcard = GetEndCard(targetCard);
                LinkPreCard(endcard);//链接新卡
            }

            _selfCardComponent.MoveTo(PreviousCard.transform.position + Vector3.up * PreviousCard.CardOffset, PreviousCard.MoveDuration);
        }

        else
        {
            if (PreviousCard != null)
            {
                UnLinkPreCard();//解绑前卡
            }
            if (CanCreateStackController())
            {
                //创建stackController
                CreateStackController();
            }
        }

        if (PreviousCard != null || NextCard != null)
        {
            CardComponent endCard = GetEndCard(this._selfCardComponent);
            SynthesisStatTable sst = new SynthesisStatTable();
            CollectionTagToSST(sst);
        }

    }

    #region 链表方法
    /// <summary>解绑前卡</summary>
    public void UnLinkPreCard()
    {
        CardComponent cp = PreviousCard;
        cp.cardNodeController.NextCard = null;
        PreviousCard = null;
        if (cp.cardNodeController.CanDestoryStackController())
        {
            cp.DestoryStackController();
        }
    }

    /// <summary>链接前卡</summary>
    public void LinkPreCard(CardComponent precard)
    {
        this.PreviousCard = precard;
        if (CanDestoryStackController())
        {
            DestoryStackController();
        }
        precard.cardNodeController.LinkNextCard(_selfCardComponent);

        // precard.UpdateChainPosition();
    }

    public void LinkNextCard(CardComponent nextcard)
    {
        this.NextCard = nextcard;
        if (CanCreateStackController())
        {
            //创建stackController
            CreateStackController();
        }
    }

    public void LinkBetween()
    {
        //等待写
    }


    /// <summary>
    /// 获取指定卡片所在链的最后一张卡
    /// </summary>
    public CardComponent GetEndCard(CardComponent startCard)
    {
        if (startCard == null) return null;

        CardComponent endcard = startCard;

        while (endcard.cardNodeController?.NextCard != null)
        {
            endcard = endcard.cardNodeController.NextCard;
        }

        return endcard;
    }

    /// <summary>
    /// 获取指定卡片所在链的头卡（第一张卡）
    /// </summary>
    public CardComponent GetHeadCard(CardComponent startCard)
    {
        if (startCard == null) return null;

        CardComponent headCard = startCard;

        while (headCard.cardNodeController.PreviousCard != null)
        {
            headCard = headCard.cardNodeController.PreviousCard;
        }

        return headCard;
    }

    #endregion

    #region 链式更新方法

    /// <summary>更新下一张卡的Z位置（比当前卡小_zOffset）</summary>
    public void UpdateZPosition()//链式更新下一张卡的Z
    {//链式更新所有z
        float z_value;
        //决定基础z值
        if (PreviousCard == null)
        {
            z_value = 0;
        }
        else
        {
            z_value = PreviousCard.transform.position.z - _selfCardComponent.ZOffset;
        }
        //改值
        Vector3 pos = _selfCardComponent.transform.position;
        pos.z = z_value;
        _selfCardComponent.transform.position = pos;

        if (NextCard != null)
        {

            NextCard.cardNodeController.UpdateZPosition();
        }
    }
    /// <summary>链式更新所有sortorder</summary><param name="adjust">修正值，抬起时填100，放下时填0</param>
    public void UpdateSortOrder(int adjust)
    {//链式更新所有sortorder
        int updateSortOrder;
        //决定基础z值
        if (PreviousCard == null)
        {
            updateSortOrder = 0 + adjust;
        }
        else
        {
            updateSortOrder = PreviousCard.sortingGroup.sortingOrder + 1 + adjust;
        }
        //改值
        _selfCardComponent.sortingGroup.sortingOrder = updateSortOrder;

        if (NextCard != null)
        {
            NextCard.cardNodeController.UpdateSortOrder(adjust);
        }
    }

    /// <summary>链式禁用子卡的collidor</summary>
    public void DisableNextCardCollider()
    {//链式禁用子卡collider
        if (NextCard != null)
        {
            Collider2D nextCollider = NextCard.GetComponent<Collider2D>();
            if (nextCollider != null)
            {
                nextCollider.enabled = false;
            }

            // 继续递归关闭后续卡的 Collider2D
            NextCard.cardNodeController.DisableNextCardCollider();
        }
    }

    /// <summary>
    /// 链式启用本卡及后续所有卡的 Collider2D（还原方法）
    /// </summary>
    public void EnableChainColliders()
    {//链式启用本卡及后续所有卡的 Collider2D
        Collider2D myCollider = _selfCardComponent.GetComponent<Collider2D>();
        if (myCollider != null)
        {
            myCollider.enabled = true;
        }

        if (NextCard != null)
        {
            NextCard.cardNodeController.EnableChainColliders();
        }
    }
    /// <summary>
    /// 链式更新位置
    /// </summary>
    public void UpdateChainPosition()
    {
        if (NextCard != null)
        {
            // 位移
            Vector3 nextPos = _selfCardComponent.transform.position + Vector3.up * _selfCardComponent.CardOffset;
            NextCard.MoveTo(nextPos, _selfCardComponent.MoveDuration);
            NextCard.cardNodeController.UpdateChainPosition();
        }
    }

    public void totheEndPosition(Vector3 currentTargetPos)
    {
        if (NextCard != null)
        {
            // 位移
            Vector3 nextPos = currentTargetPos + Vector3.up * _selfCardComponent.CardOffset;
            NextCard.MoveTo(nextPos, _selfCardComponent.MoveDuration);
            NextCard.cardNodeController.totheEndPosition(nextPos);
        }
    }

    #endregion

    #region StackController相关
    public bool CanCreateStackController()
    {
        if (PreviousCard == null && NextCard != null && _stackController == null)
        {
            return true;
        }
        return false;
    }

    public bool CanDestoryStackController()
    {
        if ((PreviousCard == null && NextCard == null && _stackController != null) || (PreviousCard != null && _stackController != null))
        {
            return true;
        }
        return false;
    }

    private void DestoryStackController()
    {
        _selfCardComponent.DestoryStackController();
    }

    private void CreateStackController()
    {
        _selfCardComponent.CreateStackController();
    }
    #endregion

    public void CollectionTagToSST(SynthesisStatTable sst)
    {
        sst.AddTag(_selfCardComponent.baseCardData.cardTag);
        
        if (IsRootCard)
        {
            _selfCardComponent.sendSSTtoStackManager(sst);
            return;
        }
        else
        {
            PreviousCard.cardNodeController.CollectionTagToSST(sst);
        }
    }

}
