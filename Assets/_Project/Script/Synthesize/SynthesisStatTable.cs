using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡链中各牌数量统计表
public class SynthesisStatTable
{
    public Dictionary<CardTag, int> SSTable;
    public SynthesisStatTable()
    {
        SSTable = new Dictionary<CardTag, int>();
    }

    public void AddTag(CardTag tag)
    {
        if (!SSTable.TryAdd(tag, 1))
        {
            SSTable[tag]++;
        }

    }

    public void PrintTable()
    {
        foreach (var kvp in SSTable)
        {
            Debug.Log($"[SynthesisStatTable] Tag: {kvp.Key}, 数量: {kvp.Value}");
        }
    }

}