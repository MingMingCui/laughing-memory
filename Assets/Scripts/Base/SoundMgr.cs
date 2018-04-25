using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObj
{
    public SoundObj()
    {
        GameObject sound_obj = new GameObject("sound_obj");
        Audio = sound_obj.AddComponent<AudioSource>();
        Audio.playOnAwake = false;
        Audio.spatialBlend = 0;
    }

    public void Play(AudioClip clip, bool musicOrSound, bool loop)
    {
        Audio.clip = clip;
        IsUsing = true;
        MusicOrSound = musicOrSound;
        Loop = loop;
        Audio.loop = loop;
        Length = clip.length;
        this.AccTime = 0;
        this.IsPause = false;
        SetMute(musicOrSound ? SoundMgr.Instance.MuteMusic : SoundMgr.Instance.MuteSound);
        if(musicOrSound)
            Audio.volume = 0.7f;
        Audio.Play();
        IsUsing = true;
    }

    public void Stop()
    {
        this.Audio.Stop();
        this.IsUsing = false;
    }

    public void SetMute(bool mute)
    {
        this.Audio.mute = mute;
    }

    public void PauseAudio(bool pause)
    {
        if (IsPause != pause)
        {
            IsPause = pause;
            if (IsPause)
                Audio.Pause();
            else
                Audio.UnPause();
        }
    }

    public AudioSource Audio  = null;
    public bool IsUsing = false;
    public bool MusicOrSound = false;
    public bool Loop = false;
    public float Length = 0;
    public float AccTime = 0;
    public bool IsPause = false;


    public void Update(float delta)
    {
        if (!IsPause && !Loop)
        {
            this.AccTime += delta;
            if (AccTime >= Length)
            {
                Audio.Stop();
                IsUsing = false;
            }
        }
    }
}

public class SoundMgr : Singleton<SoundMgr>, IUpdate {

    public SoundMgr()
    {
        ProcessCtrl.Instance.AddUpdate(this);
    }

    private bool _muteMusic = false;
    public bool MuteMusic
    {
        get { return _muteMusic; }
        set
        {
            if (_muteMusic != value)
            {
                _muteMusic = value;
                ZEventSystem.Dispatch(EventConst.OnMusicMute, value);
                for (int idx = 0; idx < _soundList.Count; ++idx)
                {
                    SoundObj obj = _soundList[idx];
                    if (obj.MusicOrSound)
                    {
                        obj.SetMute(_muteMusic);
                    }
                }
            }
        }
    }

    private bool _muteSound = false;
    public bool MuteSound
    {
        get { return _muteSound; }
        set
        {
            if (_muteSound != value)
            {
                _muteSound = value;
                ZEventSystem.Dispatch(EventConst.OnSoundMute, value);
                for (int idx = 0; idx < _soundList.Count; ++idx)
                {
                    SoundObj obj = _soundList[idx];
                    if (!obj.MusicOrSound)
                    {
                        obj.SetMute(_muteSound);
                    }
                }
            }
        }
    }

    private List<SoundObj> _soundList = new List<SoundObj>();

    public void PlayMusic(int id)
    {
        _createSoundObj(id, true, true);
    }

    public void PlaySound(int id, bool loop = false)
    {
        _createSoundObj(id, false, loop);
    }

    public void Update()
    {
        for (int idx = 0; idx < _soundList.Count; ++idx)
        {
            SoundObj obj = _soundList[idx];
            if (obj.IsUsing)
                obj.Update(Time.deltaTime);
        }
    }

    public void Clear()
    {
        _soundList.Clear();
    }

    private void _createSoundObj(int id, bool MusicOrSound, bool loop = false)
    {
        AudioClip clip = ResourceMgr.Instance.LoadSound(id);
        if (clip == null)
        {
            //Debug.LogErrorFormat("SoundMgr _createSoundObj, invalid soundid {0}", id);
            return;
        }
        _getSoundObj().Play(clip, MusicOrSound, loop);
    }

    private SoundObj _getSoundObj()
    {
        SoundObj obj = null;
        for (int idx = 0; idx < _soundList.Count; ++idx)
        {
            if (!_soundList[idx].IsUsing)
            {
                obj = _soundList[idx];
                break;
            }
        }
        if (obj == null)
        {
            obj = new SoundObj();
            _soundList.Add(obj);
        }
        return obj;
    }
}
