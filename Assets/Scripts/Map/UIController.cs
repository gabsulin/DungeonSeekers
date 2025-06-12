using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        AudioManager.Instance.PlaySFX("UIButton");
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        AudioManager.Instance.PlaySFX("UIButton");
    }
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }
}
