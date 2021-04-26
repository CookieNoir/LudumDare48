using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HarvestSign : MonoBehaviour
{
    public Image maskImage;
    public Image itemIconImage;
    public Text text;
    public string addedToInventoryDictionaryKey;
    public string notEnoughDurabilityDictionaryKey;
    public string toolTierIsLowDictionaryKey;
    public float waitingTime = 3f;
    private IEnumerator fade;

    private void Awake()
    {
        fade = Fade();
    }

    public void SetValues(int type, string itemName, Sprite itemIcon, int quantity)
    {
        StopCoroutine(fade);
        maskImage.fillAmount = 0f;
        switch (type)
        {
            case 0:
                {
                    text.text = itemName + " (" + quantity.ToString() + ") " + addedToInventoryDictionaryKey;
                    break;
                }
            case 1:
                {
                    text.text = notEnoughDurabilityDictionaryKey;
                    break;
                }
            case 2:
                {
                    text.text = toolTierIsLowDictionaryKey;
                    break;
                }
        }
        itemIconImage.sprite = itemIcon;
        gameObject.SetActive(true);
        fade = Fade();
        StartCoroutine(fade);
    }

    private IEnumerator Fade()
    {
        float f = 0f;
        while (f < 1f)
        {
            yield return null;
            f += Time.deltaTime;
            maskImage.fillAmount = Helpers.SmoothStep(f);
        }
        maskImage.fillAmount = 1f;
        yield return new WaitForSeconds(waitingTime);
        f = 1f;
        while (f > 0f)
        {
            yield return null;
            f -= Time.deltaTime;
            maskImage.fillAmount = Helpers.SmoothStep(f);
        }
        maskImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }
}