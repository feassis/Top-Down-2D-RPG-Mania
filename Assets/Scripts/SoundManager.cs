using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<BGMAudioSource> bgms;

    [Serializable]
    private class BGMAudioSource
    {
        public BGMName BGM;
        public AudioSource AudioSource;

        public bool CheckIfIsPlaying()
        {
            return AudioSource.isPlaying;
        }

        public void Play()
        {
            if (CheckIfIsPlaying())
            {
                return;
            }

            AudioSource.Play();
        }

        
    }

    public void PlayBGM(BGMName bgmName)
    {
        var bgm = bgms.Find(b => b.BGM == bgmName);

        bgm.Play();
    }
}

public enum BGMName
{
    None = 0
}
