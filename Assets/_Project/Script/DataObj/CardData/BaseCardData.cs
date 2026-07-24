using UnityEngine;


// [CreateAssetMenu(fileName = "CardData", menuName = "DataObj/CardData")]

// } CardData : ScriptableObject
[System.Serializable]
public abstract class BaseCardData
{
    // ------------------------
    // card的相关信息
    // ------------------------
     
    public int cardId = 0;
    public string name;
    public string introduce;
    public CardType cardType;
    public CardTag cardTag;
    public SynthesisRecipeTable synthesisRecipeTable;
    public bool canMerge; 

}


public enum CardType
{
    Agent,
    mission,
    resource,
}

public enum CardTag
{
    Match,
    Agent,
    Mission,
    Resource_1,
    Resource_2,
    Resource_3,
}