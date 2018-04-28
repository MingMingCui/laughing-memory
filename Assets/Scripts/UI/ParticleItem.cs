
using UnityEngine;

public class ParticleItem : MonoBehaviour
{
    ParticleSystem[] partcleCache;

    ParticleSystem[] PartcleCache
    {
        get
        {
           if(partcleCache == null)
                partcleCache = GetComponentsInChildren<ParticleSystem>();
            return partcleCache;
        }
    }
    public void Play()
    {
        for (int i = 0; i < PartcleCache.Length; i++)
        {
            PartcleCache[i].Play(true);
        }
    }
    public void Stop()
    {
        for (int i = 0; i < PartcleCache.Length; i++)
        {
            PartcleCache[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

}
