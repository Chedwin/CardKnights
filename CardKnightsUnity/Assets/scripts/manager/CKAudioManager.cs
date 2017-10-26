///
/// File Name:      CKAudioManager.cs
/// 
/// Author:         Edwin Chen
/// Date Created:   Apr 17, 2017
/// Date Modified:  Oct 19, 2017


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CKAudioManager : Singleton<CKAudioManager>
{
    public List<AudioTrack> soundFX = new List<AudioTrack>();
    public List<AudioTrack> musicSoundtrack = new List<AudioTrack>();

    AudioSource efxSource;


    public override void Awake()
    {
        base.Awake();
        efxSource = GetComponent<AudioSource>();
    }

    public AudioTrack GetMusicTrack(string _name)
    {
        for (int i = 0; i < musicSoundtrack.Count; i++)
        {
            if (_name == musicSoundtrack[i].name)
                return musicSoundtrack[i];
        }
        throw new System.Exception("Album track does NOT exist");
    }


} // end class CKAudioManager


[System.Serializable]
public struct AudioTrack
{
    public string name;
    public AudioClip clip;
}