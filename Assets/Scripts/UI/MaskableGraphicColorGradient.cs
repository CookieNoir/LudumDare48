using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MaskableGraphicColorGradient : MonoBehaviour
{
    public MaskableGraphic maskableGraphic;

    public Color color1;
    public Color color2;

    [Range(0f, 1f)] public float value = 0f;
    [Min(0.05f)] public float time = 1f;

    private float frequency;
    private IEnumerator changeColor;

    private void Awake()
    {
        maskableGraphic.color = Color.Lerp(color1, color2, value);
        frequency = 1f / time;
        changeColor = ChangeColor();
    }

    public void SetValue(float newValue)
    {
        value = Mathf.Clamp01(newValue);
        StopCoroutine(changeColor);
        changeColor = ChangeColor();
        StartCoroutine(changeColor);
    }

    private IEnumerator ChangeColor()
    {
        Color startColor = maskableGraphic.color;
        Color endColor = Color.Lerp(color1, color2, value);
        float f = 0f;
        while (f < 1f)
        {
            yield return null;
            f += Time.deltaTime * frequency;
            maskableGraphic.color = Color.Lerp(startColor, endColor, Helpers.SmoothStep(f));
        }
        maskableGraphic.color = endColor;
    }
}