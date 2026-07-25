using System;
using UnityEngine;

/// <summary>
/// 合成方法执行工厂
/// </summary>
public class SynthesisExecuteFactory
{
    public Action synthesisAction = null;
    public Action CreateExecutor()
    {
        return synthesisAction;
    }

    public SynthesisExecuteFactory CteateNewCard(CardTag cardTag,CardType cardType,Vector3 createPosition)
    {
        synthesisAction += () =>
        {
            // Debug.Print("CteateNewCard");
            GameRoot.Instance.cardFactory.CreateCard(cardType,cardTag,createPosition);
        };
        return this;
    }

    public SynthesisExecuteFactory ChangeRegionState()
    {
        synthesisAction += () =>
        {
            Debug.Log("ChangeRegionState");
        };
        return this;
    }

    public SynthesisExecuteFactory AddHook(Action method)
    {
        synthesisAction += method;
        return this;
    }


}