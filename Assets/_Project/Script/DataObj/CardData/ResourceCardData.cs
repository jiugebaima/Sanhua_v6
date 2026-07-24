public class ResourceCardData : BaseCardData
{
    public ResourceCardData(CardTag tag)
    {
        cardType = CardType.resource;
        cardTag = tag;
        getName();
        synthesisRecipeTable = new SynthesisRecipeTable();
    }

    private void getName()
    {
        switch (cardTag)
        {
            case CardTag.Agent:
                name = "agent";
                break;

            case CardTag.Match:
                name = "match";
                break;

            case CardTag.Mission:
                name = "mission";
                break;

            case CardTag.Resource_1:
                name = "resource_1";
                break;

            case CardTag.Resource_2:
                name = "resource_2";
                break;

            case CardTag.Resource_3:
                name = "resource_3";
                break;

            default:
                name = "unknown";
                break;
        }

        name += "card";
    }
}
