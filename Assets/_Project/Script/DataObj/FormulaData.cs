using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "DataObj/FormulaData")]
public class FormulaData : ScriptableObject
{
    [Tooltip("合成需要的条件列表（CardType + 数量）")]
    public List<FormulaDictionary> formulaDictionaries;

    [Tooltip("合成后生成的卡牌数据")]
    public BaseCardData outputCardData;
    [Tooltip("配方命中后，延迟几个回合执行合成")]
    public int delayTurns = 1;
}


[Serializable]
public struct FormulaDictionary
{
    public CardType cardType;
    public int num;
    public bool needDestory;
}

