using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Audio_Manager;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private List<AudioData> _sounds;

    [SerializeField] private AudioHolder audioHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _sounds = new List<AudioData>(audioHolder.AudioDatas);
        foreach (var s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
            s.source.playOnAwake = false;
        }
    }

    private void Start()
    {
        UnmuteAllSounds();
        UnmuteMusic();
        PlayStartSounds();
    }

    public void PlayStartSounds()
    {
        Play(Sound.Theme);
    }
    public void Play(Sound name)
    {
        var s = _sounds.Find(sound => sound.name == name);
        if (s == null)
            return;
        
        s.source.Play();
        Debug.Log($"Playing {name}");
    }
    public void Stop(Sound name)
    {
        var s = _sounds.Find(sound => sound.name == name);
        if (s == null)
            return;

        s.source.Stop();
    }
    public bool IsPlaying(Sound name)
    {
        var s = _sounds.Find(sound => sound.name == name);
        return s != null && s.source.isPlaying;
    }
    public void MuteAllSounds()
    {
        var s = _sounds.Where(sound => sound.audioType == AudioType.Sound).ToList();
        if (s.Count == 0)
        {
            return;
        }

        foreach (var audioData in s)
        {
            audioData.source.mute = true;
        }
    }

    public void UnmuteAllSounds()
    {
        var s = _sounds.Where(sound => sound.audioType == AudioType.Sound).ToList();
        if (s.Count == 0)
        {
            return;
        }

        foreach (var audioData in s)
        {
            audioData.source.mute = false;
        }
    }

    public void MuteMusic()
    {
        var s = _sounds.Find(sound => sound.audioType == AudioType.Music);
        if (s == null)
            return;
        s.source.mute = true;
    }

    public void UnmuteMusic()
    {
        var s = _sounds.Find(sound => sound.audioType == AudioType.Music);
        if (s == null)
            return;
        s.source.mute = false;
    }

    public void MuteSpecificTrack(Sound name)
    {
        var s = _sounds.Find(sound => sound.name == name);
        if (s == null)
            return;
        s.source.mute = true;
    }
    public void UnmuteSpecificTrack(Sound name)
    {
        var s = _sounds.Find(sound => sound.name == name);
        if (s == null)
            return;
        s.source.mute = false;
    }
    public void MuteAllMusics()
    {
        var s = _sounds.Where(sound => sound.audioType == AudioType.Music).ToList();
        foreach (var audioData in s)
        {
            audioData.source.mute = true;
        }
    }
    public void UnmuteAllMusics()
    {
        var s = _sounds.Where(sound => sound.audioType == AudioType.Music).ToList();
        foreach (var audioData in s)
        {
            audioData.source.mute = false;
        }
    }
}