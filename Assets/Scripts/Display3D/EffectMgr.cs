using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : Singleton<EffectMgr> {

    private class EffectObj
    {
        public int resid = 0;
        public bool isUsing = false;
        public Transform effect = null;
        public ParticleSystem[] particles = null;
        public Animator[] animators = null;
        public Animation[] animations = null;
        public readonly float LifeTime = 3.0f;
        public bool loop = false;
        public FightUnit owner = null;
        public float accTime = 0;
        public float FlySpeed = 0;
        public Vector3 Destination = Vector3.zero;

        private bool _pause = false;
        public bool Pause
        {
            get { return _pause; }
            set {
                if (_pause != value)
                {
                    _pause = value;
                    SetSpeed(_pause ? 0 : 1);
                }
            }
        }

        public void SetSpeed(float speed)
        {
            for (int idx = 0; idx < particles.Length; ++idx)
            {
                ParticleSystem ps = particles[idx];
                if (ps == null)
                    continue;
                var main = particles[idx].main;
                main.simulationSpeed = speed;
            }
            for (int idx = 0; idx < animators.Length; ++idx)
            {
                Animator anim = animators[idx];
                if (anim == null)
                    continue;
                animators[idx].speed = speed;
            }
            for (int idx = 0; idx < animations.Length; ++idx)
            {
                Animation animation = animations[idx];
                if (animation != null)
                {
                    foreach (AnimationState state in  animation)
                    {
                        state.speed = speed;
                    }
                }
            }
        }
    }

    private List<EffectObj> _effects = new List<EffectObj>();

    public GameObject CreateFlyEffect(FightUnit owner, int effectId, Quaternion rot, Vector3 des, float time, float playSpeed = 1)
    {
        EffectObj obj = createEffectObj(effectId);
        obj.SetSpeed(playSpeed);
        obj.Destination = des;
        Vector3 original = new Vector3(owner.CurPos.x, 0, owner.CurPos.y);
        obj.effect.localPosition = original;
        obj.effect.localRotation = rot;
        obj.FlySpeed = (des - original).magnitude / (time != 0 ? time : 1);
        obj.effect.localScale = Vector3.one;
        obj.effect.gameObject.SetActive(true);
        obj.accTime = 0;
        obj.isUsing = true;
        obj.loop = false;
        obj.owner = owner;
        return obj.effect.gameObject;
    }

    /// <summary>
    /// 创建一个特效，并且放在指定位置
    /// </summary>
    /// <param name="effectId">特效id</param>
    /// <param name="pos">位置，localpos</param>
    /// <param name="parent">父物体</param>
    public GameObject CreateEffect(FightUnit owner, int effectId, Vector3 pos, Quaternion rot, bool loop, Transform parent = null, float speed = 1)
    {
        EffectObj obj = createEffectObj(effectId);
        obj.SetSpeed(speed);
        obj.effect.parent = parent;
        obj.effect.localPosition = pos;
        obj.effect.localRotation = rot;
        obj.effect.localScale = Vector3.one;
        obj.effect.gameObject.SetActive(true);
        obj.accTime = 0;
        obj.isUsing = true;
        obj.loop = loop;
        obj.owner = owner;
        return obj.effect.gameObject;
    }

    private EffectObj createEffectObj(int effectId)
    {
        EffectObj obj = null;
        for (int idx = 0; idx < _effects.Count; ++idx)
        {
            EffectObj effect = _effects[idx];
            if (effect.resid == effectId && !effect.isUsing)
            {
                obj = effect;
                break;
            }
        }
        if (obj == null)
        {
            obj = new EffectObj();
            _effects.Add(obj);
            obj.resid = effectId;
        }
        if (obj.effect == null)
        {
            obj.effect = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(effectId) as GameObject).transform;
            obj.animations = obj.effect.GetComponentsInChildren<Animation>(true);
            obj.animators = obj.effect.GetComponentsInChildren<Animator>(true);
            obj.particles = obj.effect.GetComponentsInChildren<ParticleSystem>(true);
        }
        return obj;
    }

    public void RemoveEffect(FightUnit owner, int effectId)
    {
        for (int idx = 0; idx < _effects.Count; ++idx)
        {
            EffectObj obj = _effects[idx];
            if (!obj.isUsing)
                continue;
            if (obj.owner == owner && obj.resid == effectId)
                disableEffect(obj);
        }
    }

    public void Update(float delta)
    {
        flyEffect(delta);
        checkEffect(delta);
    }

    private void flyEffect(float delta)
    {
        for (int idx = 0; idx < _effects.Count; ++idx)
        {
            EffectObj obj = _effects[idx];
            if (!obj.isUsing || obj.Pause || obj.Destination == Vector3.zero)
                continue;
            float deltaDis = obj.FlySpeed * delta;
            if ((obj.Destination - obj.effect.position).magnitude <= deltaDis)
            {
                obj.effect.position = obj.Destination;
                return;
            }
            else
            {
                Vector3 dir = (obj.Destination - obj.effect.transform.localPosition).normalized;
                obj.effect.transform.localPosition += dir * deltaDis;
            }
        }
    }

    private void checkEffect(float delta)
    {
        for (int idx = 0; idx < _effects.Count; ++idx)
        {
            EffectObj obj = _effects[idx];
            if (obj.owner != null)
            {
                obj.Pause = FightLogic.Instance.UnitPause && !obj.owner.IsUsingActiveSkill;
            }
            if (!obj.isUsing || obj.loop || obj.Pause)
                continue;
            obj.accTime += delta;
            if (obj.accTime >= obj.LifeTime)
            {
                disableEffect(obj);
            }
        }
    }

    private void disableEffect(EffectObj obj)
    {
        if (obj.effect != null)
        {
            obj.effect.parent = null;
            obj.effect.gameObject.SetActive(false);
            obj.isUsing = false;
        }
    }

    public void Clear()
    {
        for (int idx = 0; idx < _effects.Count; ++idx)
        {
            EffectObj obj = _effects[idx];
            if (obj.effect != null)
            {
                GameObject.Destroy(obj.effect.gameObject);
            }
        }
        _effects.Clear();
    }
}
