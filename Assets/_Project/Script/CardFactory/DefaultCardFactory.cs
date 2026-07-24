using System.Collections.Generic;
using UnityEngine;


public class DefaultCardFactory : ICardFactory
{
    private GameObject _cardPrefab;

    public DefaultCardFactory(GameObject cardPrefab)
    {
        _cardPrefab = cardPrefab;
    }

    public CardComponent CreateCard(CardType type, CardTag tag, Vector3 spawnPosition)
    {
        if (_cardPrefab == null)
        {
            Debug.LogError("[DefaultCardFactory] 预制体为空");
            return null;
        }

        GameObject go = Object.Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);

        CardComponent cardComponent = go.GetComponent<CardComponent>();

        if (cardComponent != null)
        {
            // 功能测试创建资源卡
            ResourceCardData resourceCardData = new ResourceCardData(tag);
            cardComponent.baseCardData = resourceCardData;
            cardComponent.baseCardData.cardType = type;
            cardComponent.baseCardData.cardTag = tag;
        }
        cardComponent.updateUI();
        return cardComponent;
    }

    public CardComponent CreateCard(CardType type, CardTag tag, Vector3 spawnPosition ,List<SynthesisRecipe> ls)
    {
        if (_cardPrefab == null)
        {
            Debug.LogError("[DefaultCardFactory] 预制体为空");
            return null;
        }

        GameObject go = Object.Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);

        CardComponent cardComponent = go.GetComponent<CardComponent>();

        if (cardComponent != null)
        {
            // 创建有配方资源卡
            ResourceCardData resourceCardData = new ResourceCardData(tag);
            cardComponent.baseCardData = resourceCardData;
            cardComponent.baseCardData.cardType = type;
            cardComponent.baseCardData.cardTag = tag;
            resourceCardData.synthesisRecipeTable.SRecipeList = new List<SynthesisRecipe>(ls);
        }
        cardComponent.updateUI();
        return cardComponent;
    }
}

public interface ICardFactory
{
    public CardComponent CreateCard(CardType type, CardTag tag, Vector3 spawnPosition);
    public CardComponent CreateCard(CardType type, CardTag tag, Vector3 spawnPosition,List<SynthesisRecipe> ls);
}
