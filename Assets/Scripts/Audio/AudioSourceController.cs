using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AudioToggle.onAudioToggle += ToggleAudio;
    }

    public void ToggleAudio(bool value)
    {
        audioSource.volume = value ? 1f : 0f;
    }

    private void OnDestroy()
    {
        AudioToggle.onAudioToggle -= ToggleAudio;
    }
}