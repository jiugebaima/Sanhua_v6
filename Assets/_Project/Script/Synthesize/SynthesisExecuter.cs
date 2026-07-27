using System;
using UnityEngine;

public class SynthesisExecuter
{
    private int _currentProgress;
    private int _targetProgress;
    private int _progressChange;

    public Action executer;
    public SynthesisExecuter(int targetProgress,int progressChange,Action executer)
    {
        _targetProgress = targetProgress;
        _progressChange = progressChange;
        executer += executer;
    }

    /// <summary>
    /// 回合行为
    /// </summary>
    /// <returns>是否触发合成方法</returns>
    public bool TurnEnd()
    {
        _currentProgress += _progressChange;
        if(_currentProgress >= _targetProgress)
        {
            executer?.Invoke();//触发
            return true;
        }
        return false;
    }
}
