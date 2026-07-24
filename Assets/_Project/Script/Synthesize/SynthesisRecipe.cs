using System.Collections.Generic;
using UnityEngine;

//合成配方配置
[System.Serializable]
[CreateAssetMenu(fileName = "CardData", menuName = "DataObj/SynthesisRecipe")]
public class SynthesisRecipe : ScriptableObject
{
    public List<SynthesizeMetaData> SMetaList;
    public int ConsumeTurns = 1;
    public CardType ProductCardType;
    public CardTag ProductCardTag;
    //productcreatecardInfo

    public SynthesisRecipe()
    {
        SMetaList = new List<SynthesizeMetaData>();
    }
}
