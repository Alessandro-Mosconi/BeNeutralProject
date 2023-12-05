using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
        [Space(20)]
        [Header("Audio Sources")]
        [SerializeField] private AudioSource backgroundMusicAudioSource;

        [SerializeField] private AudioSource playerAudioSource;
        
        [SerializeField] private AudioSource enemieAudioSource;
        
        
        [Space(30)]
        [Header("Player sounds")]
        [SerializeField] private AudioClip fireAudioClipPlayer;
        [SerializeField] private AudioClip walkingAudioClipPlayer;
        [SerializeField] private AudioClip dieAudioClipPlayer;
        
        [Space(30)]
        [Header("Enemies sounds")]
        [SerializeField] private AudioClip fireAudioClipEnemie;
        [SerializeField] private AudioClip walkingAudioClipEnemie;
        [SerializeField] private AudioClip dieAudioClipEnemie;
        
        
        [Space(5)]
        [Header("Background Music")]
        [SerializeField] private AudioClip quietBeatAudioClip;
        [SerializeField] private float quietBeatInterval = 1.0f;
        [SerializeField] private AudioClip dramaticBeatAudioClip;
        [SerializeField] private float dramaticBeatInterval = 0.5f;

        [Space(20)]
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer mixer;
        
        private AudioClip beatAudioClip;
        private float beatInterval = 0f;
        private Coroutine backgroundMusicCoroutine;
        private bool dramaticBackgroundMusic = false;
        
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
        
        
        public void StartBackgroundMusic()
        {
            if (backgroundMusicCoroutine != null)
                return;
            backgroundMusicCoroutine = StartCoroutine("PlayBackgroundMusicRoutine");


        }

        public void StopBackgroundMusic()
        {
            if (backgroundMusicCoroutine != null)
                StopCoroutine(backgroundMusicCoroutine);
        }
        
        public void ShiftToDramaticBackgroundMusic()
        {
            if (backgroundMusicCoroutine != null)
                dramaticBackgroundMusic = true;
        }

        public void ResetBackgroundMusic()
        {
            beatAudioClip = quietBeatAudioClip;
            beatInterval = quietBeatInterval;
            dramaticBackgroundMusic = false;
        }
        
        private IEnumerator PlayBackgroundMusicRoutine()
        {
            ResetBackgroundMusic();
            
            while (true)
            {
                backgroundMusicAudioSource.PlayOneShot(beatAudioClip);
                yield return new WaitForSeconds(beatInterval);

                if (dramaticBackgroundMusic)
                {
                    beatAudioClip = dramaticBeatAudioClip;
                    beatInterval = dramaticBeatInterval;
                    
                } 
                
            }
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
