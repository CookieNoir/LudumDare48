using UnityEngine;
using UnityEngine.UI;

public class IslandFound : InteractionWindow
{
    public Text islandNameText;
    public string islandResourceNameDictionaryKey;
    public string islandTradingNameDictionaryKey;
    public string islandEventNameDictionaryKey;

    protected override void SetValues()
    {
        IslandTypes island = GameController.instance.activeIsland.islandType;
        string resultName = null;
        switch (island)
        {
            case IslandTypes.Resource:
                {
                    resultName = islandResourceNameDictionaryKey;
                    break;
                }
            case IslandTypes.Trading:
                {
                    resultName = islandTradingNameDictionaryKey;
                    break;
                }
            case IslandTypes.Event:
                {
                    resultName = islandEventNameDictionaryKey;
                    break;
                }
        }
        islandNameText.text = resultName + ' ' + GameController.instance.activeIsland.name + '.';
    }
}