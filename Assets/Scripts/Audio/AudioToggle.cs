using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class AudioToggle : MonoBehaviour
{
    public static bool audioEnabled = true;
    public static event Action<bool> onAudioToggle;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = audioEnabled;
    }

    private void Start()
    {
        onAudioToggle?.Invoke(audioEnabled);
    }

    public void ToggleAudio()
    {
        audioEnabled = toggle.isOn;
        onAudioToggle?.Invoke(audioEnabled);
    }
}