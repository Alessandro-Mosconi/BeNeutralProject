using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VolumeManager : Singleton<VolumeManager>
    {
        [Header("Volumes")] 
        [SerializeField] private float gameMusicVolume = 0.5f;
        [SerializeField] private float menuMusicVolume = 0.5f;
        [SerializeField] private float gameSoundsVolume = 0.5f;
        
        [SerializeField] private Slider gameSoundsSlider;
        [SerializeField] private Slider gameMusicSlider;
        [SerializeField] private Slider menuMusicSlider;

        public float GetGameMusicVolume()
        {
            return gameMusicVolume;
        }
    
        public float GetMenuMusicVolume()
        {
            return menuMusicVolume;
        }
        public float GetGameSoundsVolume()
        {
            return gameSoundsVolume;
        }
    
        public void SetGameMusicVolume()
        {
            gameMusicVolume = gameMusicSlider.value;
        }
    
        public void SetMenuMusicVolume()
        {
            menuMusicVolume = menuMusicSlider.value;
            AudioManager.Instance.ChangeMenuMusicVolume(menuMusicVolume);
        }
    
        public void SetGameSoundsVolume()
        {
            gameSoundsVolume = gameSoundsSlider.value;
            AudioManager.Instance.ChangeSoundsVolume(gameSoundsVolume);
        }
    
    }
}
