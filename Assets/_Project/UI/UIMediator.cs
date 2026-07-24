using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 中介者：处理 UI 按钮事件并转发至核心逻辑
/// </summary>
public class UIMediator : MonoBehaviour
{  // 结束回合按钮
    private void Awake()
    {
        // 确保 GameRoot 已初始化
        if (GameRoot.Instance == null)
        {
            Debug.LogError("[UIMediator] GameRoot 未找到，UI 事件将无法正常工作");
            return;
        }

    
    }

    // private void OnDestroy()
    // {
        
    // }

    // ---------- 私有事件处理方法 ----------

    public void endTurn()
    {
        // 1. 可选：校验当前是否可结束回合（如正在播放动画）
        // 2. 调用核心逻辑
        if (GameRoot.Instance != null && GameRoot.Instance.TurnManager != null)
        {
            GameRoot.Instance.TurnManager.EndTurn();
            // Debug.Log("[UIMediator] 回合结束请求已转发至 TurnManager");
        }
        else
        {
            Debug.LogError("[UIMediator] TurnManager 不可用，无法结束回合");
        }
    }
}