using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class AnimatorPlayer : MonoBehaviour,IAnimatorControllerPlayable
{
    private Animator animator;

    public Animator EAnimator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                Init();
            } 
            return animator;
        }
        private set { animator = value; }
    }

    private void Init()
    {
        AnimatorControllerParameter[] acp = EAnimator.parameters;
        for (int i = 0; i < acp.Length; ++i)
        {
            switch (acp[i].type)
            {
                case AnimatorControllerParameterType.Bool:
                case AnimatorControllerParameterType.Float:
                    acp[i].defaultBool = false;
                    break;
                case AnimatorControllerParameterType.Int:
                    acp[i].defaultInt = 1;
                    break;
                case AnimatorControllerParameterType.Trigger:
                    acp[i].defaultFloat = 1.0f;
                    break;
            }
        }
        SetFloat("Asp", 1.0f);
        clips = new Dictionary<string, AnimationClip>();
        playClips = new Dictionary<string, AnimationClip>();
        for (int i = 0; i < Clips.Length; ++i)
        {
            string name = Clips[i].name;
            if (!clips.ContainsKey(name))
                clips.Add(name, Clips[i]);
            else
                playClips.Add(name, Clips[i]);
        }
        
    }

    private Dictionary<string, AnimationClip> clips;
    private Dictionary<string, AnimationClip> playClips;
    private RuntimeAnimatorController Controoller
    {
        get { return EAnimator.runtimeAnimatorController; }
    }
    private AnimationClip[] Clips
    {
        get
        {
            return Controoller.animationClips;
        }
    }

    private AnimationClip GetClip(string name)
    {
        AnimationClip ac;
        clips.TryGetValue(name, out ac);
        return ac;
    }

    private float timer;

    private bool isPlaying;
    public bool IsPlaying { get { return isPlaying; } private set { isPlaying = value; } }

    public void RandomPlay(bool reset)
    {
        if (timer > 0)
            return;
        Animator[] animators = EAnimator.GetComponentsInChildren<Animator>();
        if (playClips.Count <= 0)
            return;
        IsPlaying = true;
        AnimationClip[] ac = new AnimationClip[playClips.Count];
        playClips.Values.CopyTo(ac, 0);

        int random = Random.Range(0, ac.Length);
        string name = ac[random].name;
        timer = ac[random].length * 0.95f;
        EAnimator.speed = 1f;

        for (int i = 0; i < animators.Length; ++i)
        {
            animators[i].CrossFade(name, 0f, -1, float.NegativeInfinity);
        }
        if (reset)
            StartCoroutine(StopAnimation(timer, "Idle"));
    }
    private IEnumerator StopAnimation(float time,string state)
    {
        while (timer > 0)
        {
            if (EAnimator == null)
                yield break;
            timer -= Time.deltaTime;
            if (timer <= 0.033f)
            {
                CrossFade(state, 0f, -1, float.NegativeInfinity);
                EAnimator.speed = 0.5f;
                timer = 0;
                IsPlaying = false;
            }
            yield return null;
        }
    }

    public void CrossFade(string stateName, float transitionDuration, int layer, float normalizedTime)
    {
        EAnimator.CrossFade(stateName, transitionDuration, layer, normalizedTime);
    }

    public void CrossFade(int stateNameHash, float transitionDuration, int layer, float normalizedTime)
    {
        EAnimator.CrossFade(stateNameHash, transitionDuration, layer, normalizedTime);
    }

    public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer, float fixedTime)
    {
        EAnimator.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
    }

    public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer, float fixedTime)
    {
        EAnimator.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
    }

    public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex)
    {
        return EAnimator.GetAnimatorTransitionInfo(0);
    }

    public bool GetBool(string name)
    {
        return EAnimator.GetBool(name);
    }

    public bool GetBool(int id)
    {
        return EAnimator.GetBool(id);
    }

    public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex)
    {
        return EAnimator.GetCurrentAnimatorClipInfo(layerIndex);
    }

    public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
    {
        EAnimator.GetCurrentAnimatorClipInfo(layerIndex, clips);
    }

    public int GetCurrentAnimatorClipInfoCount(int layerIndex)
    {
        return EAnimator.GetCurrentAnimatorClipInfoCount(layerIndex);
    }

    public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
    {
        return EAnimator.GetCurrentAnimatorStateInfo(layerIndex);
    }

    public float GetFloat(string name)
    {
        return EAnimator.GetFloat(name);
    }

    public float GetFloat(int id)
    {
        return EAnimator.GetFloat(id);
    }

    public int GetInteger(string name)
    {
        return EAnimator.GetInteger(name);
    }

    public int GetInteger(int id)
    {
        return EAnimator.GetInteger(id);
    }

    public int GetLayerCount()
    {
        return EAnimator.layerCount;
    }

    public int GetLayerIndex(string layerName)
    {
        return EAnimator.GetLayerIndex(layerName);
    }

    public string GetLayerName(int layerIndex)
    {
        return GetLayerName(layerIndex);
    }

    public float GetLayerWeight(int layerIndex)
    {
       return EAnimator.GetLayerWeight(layerIndex);
    }

    public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex)
    {
        return EAnimator.GetNextAnimatorClipInfo(layerIndex);
    }

    public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
    {
        EAnimator.GetNextAnimatorClipInfo(layerIndex, clips);
    }

    public int GetNextAnimatorClipInfoCount(int layerIndex)
    {
        return EAnimator.GetNextAnimatorClipInfoCount(layerIndex);
    }

    public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex)
    {
        return EAnimator.GetNextAnimatorStateInfo(layerIndex);
    }

    public AnimatorControllerParameter GetParameter(int index)
    {
        return EAnimator.GetParameter(index);
    }

    public int GetParameterCount()
    {
        return EAnimator.parameterCount;
    }

    public bool HasState(int layerIndex, int stateID)
    {
        return EAnimator.HasState(layerIndex, stateID);
    }

    public bool IsInTransition(int layerIndex)
    {
        return EAnimator.IsInTransition(layerIndex);
    }

    public bool IsParameterControlledByCurve(string name)
    {
        return EAnimator.IsParameterControlledByCurve(name);
    }

    public bool IsParameterControlledByCurve(int id)
    {
        return EAnimator.IsParameterControlledByCurve(id);
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        EAnimator.Play(stateName, layer, normalizedTime);
    }

    public void Play(int stateNameHash, int layer, float normalizedTime)
    {
        EAnimator.Play(stateNameHash, layer, normalizedTime);
    }

    public void PlayInFixedTime(string stateName, int layer, float fixedTime)
    {
        EAnimator.PlayInFixedTime(stateName, layer, fixedTime);
    }

    public void PlayInFixedTime(int stateNameHash, int layer, float fixedTime)
    {
        EAnimator.PlayInFixedTime(stateNameHash, layer, fixedTime);
    }

    public void ResetTrigger(string name)
    {
        EAnimator.ResetTrigger(name);
    }

    public void ResetTrigger(int id)
    {
        EAnimator.ResetTrigger(id);
    }

    public void SetBool(string name, bool value)
    {
        EAnimator.SetBool(name, value);
    }

    public void SetBool(int id, bool value)
    {
        EAnimator.SetBool(id, value);
    }

    public void SetFloat(string name, float value)
    {
        EAnimator.SetFloat(name, value);
    }

    public void SetFloat(int id, float value)
    {
        EAnimator.SetFloat(id, value);
    }

    public void SetInteger(string name, int value)
    {
        EAnimator.SetInteger(name, value);
    }

    public void SetInteger(int id, int value)
    {
        EAnimator.SetInteger(id, value);
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
        EAnimator.SetLayerWeight(layerIndex, weight);
    }

    public void SetTrigger(string name)
    {
        EAnimator.SetTrigger(name);
    }

    public void SetTrigger(int id)
    {
        EAnimator.SetTrigger(id);
    }

    public void OnDestroy()
    {
        StopCoroutine("StopAnimation");
    }
}
