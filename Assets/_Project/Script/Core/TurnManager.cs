using System;
using UnityEngine;

namespace CardGame.Core
{
    /// <summary>
    /// 回合管理器 - 仅负责广播“回合结束”信号
    /// </summary>
    public class TurnManager
    {
        // 当玩家点击“结束回合”按钮时触发
        public event Action OnTurnEnded;

        /// <summary>
        /// 结束当前回合（由UI按钮调用）
        /// 所有“回合结算”逻辑（倒计时、抽牌、AI等）通过订阅 OnTurnEnded 事件执行
        /// </summary>
        public void EndTurn()
        {
            Debug.Log("[TurnManager] 回合结束，开始统一结算...");
            OnTurnEnded?.Invoke();
        }
    }
}