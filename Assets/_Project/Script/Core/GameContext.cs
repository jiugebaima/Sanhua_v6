using System.Collections.Generic;

namespace CardGame.Core
{
    /// <summary>
    /// 游戏运行时数据上下文（所有可变状态）
    /// </summary>
    public class GameContext
    {
        // 所有堆叠控制器（由StackController在Awake时注册，OnDestroy时注销）
        private List<StackController> _allStacks = new List<StackController>();
        public IReadOnlyList<StackController> AllStacks => _allStacks;

        // 所有卡牌组件（可选，用于全局查询）
        private List<CardComponent> _allCards = new List<CardComponent>();
        public IReadOnlyList<CardComponent> AllCards => _allCards;

        // ---------- 堆叠注册/注销 ----------
        public void RegisterStack(StackController stack)
        {
            if (stack != null && !_allStacks.Contains(stack))
            {
                _allStacks.Add(stack);
            }
        }

        public void UnregisterStack(StackController stack)
        {
            if (stack != null && _allStacks.Contains(stack))
            {
                _allStacks.Remove(stack);
            }
        }

        // ---------- 卡牌注册/注销 ----------
        public void RegisterCard(CardComponent card)
        {
            if (card != null && !_allCards.Contains(card))
            {
                _allCards.Add(card);
            }
        }

        public void UnregisterCard(CardComponent card)
        {
            if (card != null && _allCards.Contains(card))
            {
                _allCards.Remove(card);
            }
        }
    }
}