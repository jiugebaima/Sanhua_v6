using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试卡片生成类 - 用于在编辑器中手动生成卡片
/// </summary>
public class TestCardSpawner : MonoBehaviour
{
    [Header("生成配置")]
    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;  // 生成位置
    [SerializeField] private CardType _cardType = CardType.Agent;    // 卡牌类型
    [SerializeField] private CardTag _cardTag = CardTag.Agent;       // 卡牌标签
    
    [Header("快捷操作")]
    [SerializeField] private bool _spawnOnStart = false;  // 是否在Start时自动生成
    [SerializeField] public List<SynthesisRecipe> synthesisRecipes;
    
    private ICardFactory _cardFactory;
    
    private void Start()
    {
        // 从 GameRoot 获取 ICardFactory
        if (GameRoot.Instance != null)
        {
            _cardFactory = GameRoot.Instance.cardFactory;
        }
        else
        {
            Debug.LogError("[TestCardSpawner] GameRoot 实例不存在！");
            return;
        }
        
        // 如果勾选了自动生成
        if (_spawnOnStart)
        {
            CreateCard();
        }
    }
    
    /// <summary>
    /// 根据当前配置生成卡片
    /// 可在 Inspector 中右键调用
    /// </summary>
    [ContextMenu("Create Card")]
    public void CreateCard()
    {
        if (_cardFactory == null)
        {
            if (GameRoot.Instance != null)
            {
                _cardFactory = GameRoot.Instance.cardFactory;
            }
            else
            {
                Debug.LogError("[TestCardSpawner] CardFactory 为空！");
                return;
            }
        }
        
        // 生成卡片
        CardComponent card = _cardFactory.CreateCard(_cardType, _cardTag,_spawnPosition,synthesisRecipes);
        
        if (card != null)
        {
            // 设置位置
            card.transform.position = _spawnPosition;
            
            Debug.Log($"[TestCardSpawner] 生成卡片: Type={_cardType}, Tag={_cardTag}, 位置={_spawnPosition}");
        }
        else
        {
            Debug.LogError($"[TestCardSpawner] 生成卡片失败: Type={_cardType}, Tag={_cardTag}");
        }
    }
    
    /// <summary>
    /// 使用指定参数生成卡片
    /// </summary>
    public CardComponent CreateCard(CardType type, CardTag tag, Vector3? position = null)
    {
        if (_cardFactory == null)
        {
            if (GameRoot.Instance != null)
            {
                _cardFactory = GameRoot.Instance.cardFactory;
            }
            else
            {
                Debug.LogError("[TestCardSpawner] CardFactory 为空！");
                return null;
            }
        }
        
        // 生成卡片
        CardComponent card = _cardFactory.CreateCard(type, tag,_spawnPosition);
        
        if (card != null)
        {
            // 设置位置（使用传入位置或默认生成点）
            Vector3 pos = position ?? _spawnPosition;
            card.transform.position = pos;
            
            Debug.Log($"[TestCardSpawner] 生成卡片: Type={type}, Tag={tag}, 位置={pos}");
        }
        return card;
    }
    
    // 在 Inspector 中显示生成点位置
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_spawnPosition, 0.5f);
        
        // 绘制箭头指示生成方向
        Gizmos.DrawRay(_spawnPosition, Vector3.up * 1f);
    }
}