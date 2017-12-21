using System.Collections;
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
    public static readonly string bgMusic = "bgMusic.mp3";
    public static readonly string heavyRain = "heavyRain.mp3";
    public static readonly string lightRain = "lightRain.mp3";
    public static readonly string land = "land.wav";
    public static readonly string collectRain = "raindrop.wav";
    public static readonly string aphidHit = "aphidHit.wav";
        

    List<string> disabledSounds;
    AudioSource musicPlayer;
    AudioSource bgMusicPlayer;
    AudioClip landingAC;
    bool bgPlaying = false;

    private LOLAudio()
    {
        disabledSounds = new List<string>();
        landingAC = Resources.Load<AudioClip>("Music/land");
    }


    public void PlayBackgroundAudio(string _name)
    {
        if (disabledSounds.Contains(_name))
            return;

#if UNITY_EDITOR
        GameObject go = new GameObject();
        go.name = "RainSound";
        AudioSource audioSrc = go.AddComponent<AudioSource>();
        string filePath = "Music/" + System.IO.Path.GetFileNameWithoutExtension(_name);
        audioSrc.clip = Resources.Load<AudioClip>(filePath);
        audioSrc.loop = true;
        audioSrc.Play();
        bgMusicPlayer = audioSrc;
        Object.DontDestroyOnLoad(go);
#elif UNITY_WEBGL
        LOLSDK.Instance.PlaySound(_name, true, true);
#endif
        bgPlaying = true;
    }

    public void SetBGLevel(float volume)
    {
        if (!bgPlaying)
            return;
#if UNITY_EDITOR    
            bgMusicPlayer.volume = volume;
#elif UNITY_WEBGL
        LOLSDK.Instance.ConfigureSound(1, volume, 1); //Should be 0 or 1? who knows
#endif
    }

    public void PlayAudio(string _name, bool loop = false)
    {
        if (!GV.Sound_Active || !LOLSDK.Instance.IsInitialized || disabledSounds.Contains(_name))
            return;

#if UNITY_EDITOR
        PlayEditorAudio(_name, loop);        
#elif UNITY_WEBGL
        LOLSDK.Instance.PlaySound(_name, false, loop);
#endif
    }

    public void StopAudio(string _name)
    {
        if (LOLSDK.Instance.IsInitialized)
            LOLSDK.Instance.StopSound(_name);
        if (_name == bgMusic)
            bgPlaying = false;
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
            go.name = "AudioLoop_" + _name;
            Object.DontDestroyOnLoad(go);
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
            AudioClip ac = (_name == "land")? landingAC : Resources.Load<AudioClip>("Music/" + _name);
            musicPlayer.PlayOneShot(ac);
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(_name), new Vector3());
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(_name), new Vector3());
        }

        //_name = System.IO.Path.GetFileNameWithoutExtension(_name);
        //AudioClip ac = Resources.Load<AudioClip>("Music/" + _name);
        //musicPlayer.PlayOneShot(Resources.Load<AudioClip>("Music/" + _name));
    }

    public void AddDisabledSound(string toDisable)
    {
        if (!disabledSounds.Contains(toDisable))
            disabledSounds.Add(toDisable);
    }


    public void RemoveDisabledSound(string toRemove)
    {
        StopAudio(toRemove);
        disabledSounds.Remove(toRemove);
    }

    public void ClearDisabledSounds()
    {
        disabledSounds.Clear();
    }
}