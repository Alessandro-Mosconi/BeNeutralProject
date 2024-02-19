using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using Random = System.Random;

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
        [SerializeField] private AudioClip fallAudioClipPlayer;
        [SerializeField] private AudioClip jumpAudioClipPlayer;

        [Space(30)] [Header("Enemies sounds")] [SerializeField]
        private AudioClip fireAudioClipEnemie;

        [SerializeField] private AudioClip walkingAudioClipEnemie;
        [SerializeField] private AudioClip dieAudioClipEnemie;


        [Space(5)]
        [Header("Background Music")]
        [SerializeField] private AudioClip backgroundMenuAudioClip; //0
        [SerializeField] private AudioClip backgroundGameAudioClip; //1
        [SerializeField] private AudioClip backgroundNextLevelAudioClip; //2
        
        [SerializeField] private AudioClip gameOverAudioClip; 
        
        [Space(3)]
        [Header("Background music controls")]
        [SerializeField] private float backgroundVolume;
        [SerializeField] private float fadeInTime;
        
        [Space(20)]
        [Header("Menu Interactions")]
        [SerializeField] private AudioClip buttonClick;
        [SerializeField] private AudioClip keyboardClick1;
        [SerializeField] private AudioClip keyboardClick2;
        [SerializeField] private AudioClip keyboardClick3;
        
        
        [Space(20)]
        [Header("Audio Mixer")]
        [SerializeField] private VolumeManager mixer;
        
        private Coroutine backgroundMusicCoroutine;
        private Coroutine fadeInCoroutine;
        private AudioClip currentBackgroundMusic;
        private bool isWalking = false;
        
        // change volumes
        public void ChangeSoundsVolume(float newVolume)
        {
            playerAudioSource.volume = newVolume;
            enemieAudioSource.volume = newVolume;
        }
        
        public void ChangeBackgroundMusicVolume(float newVolume)
        {
            backgroundMusicAudioSource.volume = newVolume;
            backgroundVolume = newVolume;
        }
        
        public void ChangeMenuVolume(float newVolume)
        {
            menuInteractionSource.volume = newVolume;
        }
        
        // - player sounds

        public void PlayFirePlayer()
        {
            playerAudioSource.PlayOneShot(fireAudioClipPlayer);
        }
        public void PlayJumpPlayer()
        {
            playerAudioSource.PlayOneShot(jumpAudioClipPlayer);
        }
    
        public void PlayWalkingPlayer()
        {
            if (!isWalking)
            {
                isWalking = true;
                if(!playerAudioSource.isPlaying)
                    playerAudioSource.PlayOneShot(walkingAudioClipPlayer);
            }
                
        }
        
        public void StopWalkingPlayerSound()
        {
            isWalking = false;
        }

        public void PlayDiePlayer()
        {
            playerAudioSource.PlayOneShot(dieAudioClipPlayer);
        }
        public void PlayForceFieldPlayer() 
        {
            playerAudioSource.PlayOneShot(forceFieldAudioClipPlayer);
        }
        public void PlayFallPlayer() 
        {
            playerAudioSource.PlayOneShot(fallAudioClipPlayer);
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
            menuInteractionSource.PlayOneShot(buttonClick);
        }
        public void PlayClickKeyboard()
        {
            int n = RandomNumberGenerator.GetInt32(1, 2);
            if (n == 1)
            {
                menuInteractionSource.PlayOneShot(keyboardClick1);
            }else if (n == 2)
            {
                menuInteractionSource.PlayOneShot(keyboardClick2);
            }
        }
        
        // - background sounds
        
        public void PlayGameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        private IEnumerator GameOverRoutine()
        {
            yield return new WaitForSeconds(1f);
            playerAudioSource.PlayOneShot(gameOverAudioClip);
        }

        public void ChooseBackgroundMusic(int musicChoice)
        {
            switch (musicChoice)
            {
                case 0:
                    currentBackgroundMusic = backgroundMenuAudioClip;
                    backgroundVolume = mixer.GetMenuMusicVolume();
                    break;
                case 1:
                    currentBackgroundMusic = backgroundGameAudioClip;
                    backgroundVolume = mixer.GetGameMusicVolume();
                    break;
                case 2:
                    currentBackgroundMusic = backgroundNextLevelAudioClip;
                    backgroundVolume = mixer.GetGameMusicVolume();
                    break;
                default:
                    currentBackgroundMusic = backgroundMenuAudioClip;
                    backgroundVolume = mixer.GetMenuMusicVolume();
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
    }
}
