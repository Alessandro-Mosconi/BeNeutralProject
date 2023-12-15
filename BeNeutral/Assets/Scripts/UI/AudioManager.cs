using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace UI
{

    public class AudioManager : Singleton<AudioManager>
    {


        [Space(20)] [Header("Audio Sources")] 
        [SerializeField] private AudioSource backgroundMusicAudioSource;
        [SerializeField] private AudioSource playerAudioSource;
        [SerializeField] private AudioSource enemieAudioSource;
        [SerializeField] private AudioSource menuInteractionSource;


        [Space(30)] [Header("Player sounds")] 
        [SerializeField] private AudioClip fireAudioClipPlayer;
        [SerializeField] private AudioClip walkingAudioClipPlayer;
        [SerializeField] private AudioClip dieAudioClipPlayer;
        [SerializeField] private AudioClip forceFieldAudioClipPlayer;

        [Space(30)] [Header("Enemies sounds")] [SerializeField]
        private AudioClip fireAudioClipEnemie;

        [SerializeField] private AudioClip walkingAudioClipEnemie;
        [SerializeField] private AudioClip dieAudioClipEnemie;


        [Space(5)]
        [Header("Background Music")]
        [SerializeField] private AudioClip backgroundMenuAudioClip; //0
        [SerializeField] private AudioClip backgroundGameAudioClip; //1
        [SerializeField] private AudioClip backgroundLoseAudioClip; //2
        [SerializeField] private AudioClip backgroundNextLevelAudioClip; //3
        [Space(3)]
        [Header("Background music controls")]
        [SerializeField] private float backgroundVolume;
        [SerializeField] private float fadeInTime;
        
        [Space(20)]
        [Header("Menu Interactions")]
        [SerializeField] private AudioClip buttonClick;
        
        
        [Space(20)]
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer mixer;
        
        private Coroutine backgroundMusicCoroutine;
        private Coroutine fadeInCoroutine;
        private AudioClip currentBackgroundMusic;
        
        
        // - player sounds
        public void PlayFirePlayer()
        {
            playerAudioSource.PlayOneShot(fireAudioClipPlayer);
        }
        public void PlayWalkingPlayer()
        {
            playerAudioSource.PlayOneShot(walkingAudioClipPlayer);
        }
        public void PlayDiePlayer()
        {
            playerAudioSource.PlayOneShot(dieAudioClipPlayer);
        }
        public void PlayForceFieldPlayer() 
        {
            playerAudioSource.PlayOneShot(forceFieldAudioClipPlayer);
        }
        
        
        // - enemies sounds
        public void PlayFireEnemie()
        {
            enemieAudioSource.PlayOneShot(fireAudioClipEnemie);
        }
        public void PlayWalkingEnemie()
        {
            enemieAudioSource.PlayOneShot(walkingAudioClipEnemie);
        }
        public void PlayDieEnemie()
        {
            enemieAudioSource.PlayOneShot(dieAudioClipEnemie);
        }

        // - menu interactions
        
        public void PlayClickButton()
        {
            enemieAudioSource.PlayOneShot(buttonClick);
        }
        
        // - background sounds
        public void ChooseBackgroundMusic(int musicChoice)
        {
            switch (musicChoice)
            {
                case 0:
                    currentBackgroundMusic = backgroundMenuAudioClip;
                    break;
                case 1:
                    currentBackgroundMusic = backgroundGameAudioClip;
                    break;
                case 2:
                    currentBackgroundMusic = backgroundLoseAudioClip;
                    break;
                case 3:
                    currentBackgroundMusic = backgroundNextLevelAudioClip;
                    break;
                default:
                    currentBackgroundMusic = backgroundMenuAudioClip;
                    break;
            }
            StartBackgroundMusic();
        }

        private void StartBackgroundMusic()
        {
            StopBackgroundMusic();
            if (backgroundMusicCoroutine != null)
                return;
            if(fadeInCoroutine != null)
                StopCoroutine(fadeInCoroutine);
            FadeInMusic(backgroundMusicAudioSource, fadeInTime);
            backgroundMusicAudioSource.PlayOneShot(currentBackgroundMusic);
            backgroundMusicCoroutine = StartCoroutine(PlayBackgroundMusicRoutine());
        }
        
        private IEnumerator PlayBackgroundMusicRoutine()
        {
            while (backgroundMusicAudioSource.isPlaying)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            backgroundMusicCoroutine = null;
            StartBackgroundMusic();
        }

        public void StopBackgroundMusic()
        {
            backgroundMusicAudioSource.Stop();
            if (backgroundMusicCoroutine == null) return;
            StopCoroutine(backgroundMusicCoroutine);
            backgroundMusicCoroutine = null;
        }

        // - effects 
        private void FadeInMusic(AudioSource audioS, float time)
        {
            audioS.volume = 0;
            fadeInCoroutine = StartCoroutine(FadeInRoutine(audioS, time));
        }

        private IEnumerator FadeInRoutine(AudioSource audioS, float time)
        {
            float currentVolume = 0;
            const float step = 0.01f;
            float waitingTime = time / (backgroundVolume / step);
            while (backgroundVolume > currentVolume)
            {
                currentVolume += step;
                audioS.volume = currentVolume;
                yield return new WaitForSeconds(waitingTime);
            }
            audioS.volume = backgroundVolume;
        }
       
        
        // these functions are needed if the game has a menu that can modify the master volume
        public void SetMixerMasterVolume(float volume)
        {
            float mixerVolume = AudioManager.SliderToDB(volume);
            mixer.SetFloat ("MasterVolume", mixerVolume);
        }
    
        public static float SliderToDB(float volume, float maxDB=-10, float minDB=-80)
        {
            float dbRange = maxDB - minDB;
            return minDB + volume * dbRange;
        }
    }
}
