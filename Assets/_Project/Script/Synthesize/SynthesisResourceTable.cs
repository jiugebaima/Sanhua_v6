using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//配方表与合成匹配
public class SynthesisRecipeTable
{
    public List<SynthesisRecipe> SRecipeList;

    public SynthesisRecipeTable()
    {
        SRecipeList = new List<SynthesisRecipe>();
    }


    /// <summary>
    /// 匹配配方
    /// </summary>
    /// <param name="sst"></param>
    /// <returns>命中的配方</returns>
    public SynthesisRecipe TryMatch(SynthesisStatTable sst)
    {
        // 遍历所有合成配方
        for (int i = 0; i < SRecipeList.Count; i++)
        {
            bool isMatch = true;
            SynthesisRecipe sr = SRecipeList[i];
            if (sr.SMetaList.Count != sst.SSTable.Count)
            {
                continue;
            }
            //遍历所有元配方
            foreach (var meta in sr.SMetaList)
            {
                //sst中各标签的统计数量
                int count = sst.SSTable.TryGetValue(meta.tag, out int value) ? value : 0;
                switch (meta.matchMode)
                {
                    default:// 未知模式视为exact
                    case SynthesisMatchMode.EXACT:
                        if (count != meta.requir_value)//不精确
                            isMatch = false;
                        break;

                    case SynthesisMatchMode.GT:
                        if (count <= meta.requir_value)//小于
                            isMatch = false;
                        break;

                    case SynthesisMatchMode.SCALE:
                        if (count < meta.requir_value ||
                            count % meta.requir_value != 0)// 条件：count 是 requir_value 的整数倍
                        {
                            isMatch = false;
                        }
                        break;
                }
                if (!isMatch)
                    break;
            }
            if (isMatch)
                return sr;
        }
        return null;//未匹配
    }

}





