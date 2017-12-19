﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;

public class LOLAudio
{
#region Singleton
    private static LOLAudio instance;

    public static LOLAudio Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LOLAudio();
            }
            return instance;
        }
    }
    #endregion
    
    AudioSource musicPlayer;
    AudioSource bgMusicPlayer;

    private LOLAudio()
    {
    }


    public void PlayBackgroundAudio(string _name)
    {
#if UNITY_EDITOR
        GameObject go = new GameObject();
        AudioSource audioSrc = go.AddComponent<AudioSource>();
        audioSrc.clip = Resources.Load<AudioClip>("Music/" + _name);
        audioSrc.loop = true;
        audioSrc.Play();
        bgMusicPlayer = audioSrc;
#elif UNITY_WEBGL
        LOLSDK.Instance.PlaySound("Resources/" + _name, true, true);
#endif
    }

    public void SetBGLevel(float volume)
    {
#if UNITY_EDITOR
        bgMusicPlayer.volume = volume;
#elif UNITY_WEBGL
        LOLSDK.Instance.ConfigureSound(1, volume, 1); //Should be 0 or 1? who knows
#endif
    }

    public void PlayAudio(string _name, bool loop = false)
    {
        if (!GV.Sound_Active || !LOLSDK.Instance.IsInitialized)
            return;

#if UNITY_EDITOR
        PlayEditorAudio(_name, loop);        
#elif UNITY_WEBGL
        LOLSDK.Instance.PlaySound("Resources/" + _name, false, loop);
#endif
    }

    public void StopAudio(string _name)
    {
        if (LOLSDK.Instance.IsInitialized)
            LOLSDK.Instance.StopSound(_name);
    }

    private void PlayEditorAudio(string _name, bool loop)
    {
        if (loop)
        {
            GameObject go = new GameObject();
            AudioSource audioSrc = go.AddComponent<AudioSource>();
            audioSrc.clip = Resources.Load<AudioClip>(_name);
            audioSrc.loop = true;
            audioSrc.Play();
        }
        else
        {
            if (!musicPlayer)
            {
                GameObject go = new GameObject();
                go.name = "musicPlayer";
                musicPlayer = go.AddComponent<AudioSource>();
            }
            _name = System.IO.Path.GetFileNameWithoutExtension(_name);
            AudioClip ac = Resources.Load<AudioClip>("Music/" + _name);
            musicPlayer.PlayOneShot(ac);
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(_name), new Vector3());
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(_name), new Vector3());
        }

        //_name = System.IO.Path.GetFileNameWithoutExtension(_name);
        //AudioClip ac = Resources.Load<AudioClip>("Music/" + _name);
        //musicPlayer.PlayOneShot(Resources.Load<AudioClip>("Music/" + _name));
    }

}