using UnityEngine;
using UnityEngine.UI;

public class RepairWindow : MonoBehaviour
{
    public Image itemIcon;
    public Text restoresText;
    public string restoresDictionaryKey;
    public string ofDurabilityDictionaryKey;
    public string ofHungerDictionaryKey;

    public void RefreshData(Sprite sprite, bool restoresDurability, float value)
    {
        itemIcon.sprite = sprite;
        restoresText.text = restoresDictionaryKey + ' ' + value.ToString() + ' ';
        if (restoresDurability)
        {
            restoresText.text += ofDurabilityDictionaryKey;
        }
        else
        {
            restoresText.text += ofHungerDictionaryKey;
        }
    }
}