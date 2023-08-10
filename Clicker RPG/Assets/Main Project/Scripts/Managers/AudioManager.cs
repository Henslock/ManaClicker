using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REPLACE THIS WITH FMOD DOWN THE LINE

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static AudioManager instance;
    //Audio Sources
    public AudioSource UIAudioSource;

    public AudioClip specialClick;
    public AudioClip genericClick;
    public AudioClip promptClick;
    public AudioClip toggleClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayUISound(AudioClip clip)
    {
        if (clip == null) return;
        UIAudioSource.pitch = 1.0f;
        UIAudioSource.PlayOneShot(clip);
    }

    public void PlayUISound(AudioClip clip, float _pitch)
    {
        if (clip == null) return;
        UIAudioSource.pitch = _pitch;
        UIAudioSource.PlayOneShot(clip);

    }    
    
    public void PlayUISound(AudioClip clip, float _volume, float _pitch)
    {
        if (clip == null) return;
        UIAudioSource.pitch = _pitch;
        UIAudioSource.PlayOneShot(clip, _volume);

    }

    public void PlayUISound(AudioClip clip, AudioSource source, float _volume, float _pitch)
    {
        if (clip == null) return;
        source.pitch = _pitch;
        source.PlayOneShot(clip, _volume);

    }

    public void PlayUISoundRandomPitch(AudioClip clip, float _volume, float _pitchmin, float _pitchmax)
    {
        if (clip == null) return;
        var pitchResult = Random.Range(_pitchmin, _pitchmax);
        UIAudioSource.pitch = pitchResult;
        UIAudioSource.PlayOneShot(clip, _volume);
    }

    //No volume specification
    public void PlayUISoundRandomPitch(AudioClip clip, float _pitchmin, float _pitchmax)
    {
        if (clip == null) return;
        var pitchResult = Random.Range(_pitchmin, _pitchmax);
        UIAudioSource.pitch = pitchResult;
        UIAudioSource.PlayOneShot(clip, 1.0f);
    }

    public void PlayUISoundRandomPitch(AudioClip clip, AudioSource source, float _volume, float _pitchmin, float _pitchmax)
    {
        if (clip == null) return;
        var pitchResult = Random.Range(_pitchmin, _pitchmax);
        source.pitch = pitchResult;
        source.PlayOneShot(clip, _volume);
    }

    //No volume specification
    public void PlayUISoundRandomPitch(AudioClip clip, AudioSource source, float _pitchmin, float _pitchmax)
    {
        if (clip == null) return;
        var pitchResult = Random.Range(_pitchmin, _pitchmax);
        source.pitch = pitchResult;
        source.PlayOneShot(clip, 1.0f);
    }

}
