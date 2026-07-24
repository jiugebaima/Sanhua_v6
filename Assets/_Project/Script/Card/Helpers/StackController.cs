using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 堆叠逻辑控制器判定合成逻辑，进度与产物
/// </summary>
/// 
/// 收集SST
/// 从card中获取配方进行比对
/// 比对后根据命中的配方创建事件
/// 挂载每回合-1在回合计数器上
/// 触发执行器
public class StackController
{
    [Header("堆叠配置")]
    [SerializeField] private Vector3 _spawnOffset = new Vector3(1.5f, 0.5f, 0); // 生成偏移


    // 核心数据
    private CardComponent _selfCardComponent ;
    public BaseCardData cardData;
    public SynthesisRecipeTable SRTable;
    public SynthesisRecipe targetRecipe;

    private int _turnCount;
    public Action synthesisExecuter;

    /// <summary>
    /// 构造方法,将当前 StackController 初始化为指定卡牌的根堆叠
    /// </summary>
    public StackController(CardComponent cardComponent,BaseCardData cd)
    {
        Debug.Log("create StackController");
        _selfCardComponent = cardComponent;
        cardData = cd;
        SRTable = cardData.synthesisRecipeTable;
    }

    public void DestoryStackController()
    {
        Debug.Log("Destory StackController");
        RemoveAttachToTurnManager();
    }
    
    //对外接口,
    // 1获取配方，
    // 2如果获取则创建合成器
    // 向Turnmanager注册 倒数countDown函数
    // countdown函数到条件后触发合成器
    public void trySynthesis(SynthesisStatTable SST)
    {
        //重置状态
        resetExecutState();
        Debug.Log("trySynthesis");


        // if (getTargetRecipe(SST))
        if (true)
        {
            //从工厂获取合成执行器(此处为测试)
            synthesisExecuter += () =>
            {
              Debug.Log("合成");
              resetExecutState();
            };
            //向Turnmanager添加自己的合成执行器
            _turnCount = 3;
            AttachToTurnManager();
        }
    }

    /// <summary>
    /// 获取执行配方
    /// </summary>
    /// <param name="SST">记录表</param>
    /// <returns></returns>
    private bool getTargetRecipe(SynthesisStatTable SST)
    {
        SynthesisRecipe matchResult = SRTable.TryMatch(SST);
        if(matchResult == null)
        {
            return false;
        }
        targetRecipe = matchResult;

        return true;
    }

    private void AttachToTurnManager()
    {
        Debug.Log("AttachToTurnManager");
        GameRoot.Instance.TurnManager.OnTurnEnded+=CountDown;
    }

    private void RemoveAttachToTurnManager()
    {
        Debug.Log("RemoveAttachToTurnManager");
        GameRoot.Instance.TurnManager.OnTurnEnded-=CountDown;
    }

    private void CountDown()
    {
        _turnCount-=1;
        if(_turnCount < 1)
        {
            _turnCount = 0;
            synthesisExecuter?.Invoke();
        }
    }

    public void resetExecutState()
    {
        RemoveAttachToTurnManager();
        synthesisExecuter = null;
    }
   
}