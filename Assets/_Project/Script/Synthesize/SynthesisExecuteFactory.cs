using System;
using System.Diagnostics;

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

    public SynthesisExecuteFactory CteateNewCard()
    {
        synthesisAction += () =>
        {
            Debug.Print("CteateNewCard");
        };
        return this;
    }

    public SynthesisExecuteFactory ChangeRegionState()
    {
        synthesisAction += () =>
        {
            Debug.Print("ChangeRegionState");
        };
        return this;
    }

    public SynthesisExecuteFactory AddHook(Action method)
    {
        synthesisAction += method;
        return this;
    }


}