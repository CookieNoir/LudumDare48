using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public GameObject hintGameObject;
    public Text hintText;
    public RectTransform hintBackground;
    public float widthAddition = 20f;
    public string pressDictionaryKey;
    public string[] hintsDictionaryKeys;

    public void SetHint(HintTypes type)
    {
        if (type != HintTypes.None)
        {
            hintGameObject.SetActive(true);
            hintText.text = pressDictionaryKey + ' ' + GameController.instance.interactionKey.ToString() + ' ' + hintsDictionaryKeys[(int)type];
            hintBackground.sizeDelta = new Vector2(hintText.preferredWidth + widthAddition, hintBackground.sizeDelta.y);
        }
    }

    public void HideHint()
    {
        hintGameObject.SetActive(false);
    }
}