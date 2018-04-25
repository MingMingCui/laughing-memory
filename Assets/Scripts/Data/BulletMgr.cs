using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public enum BulletType
{
    Normal = 0,
    Bounce = 1,
    Pass = 2,
}

public class BulletData
{
    public int TplId = 0;
    public float CSpeed = 0;
    public float Speed = 0;
    public int Type = 0;
    public int BounceTimes = 0;
    public int[] Effects = null;
    public Skill CSkill = null;
    private bool _isUsing = false;
    public bool IsUsing
    {
        get { return _isUsing; }
        set
        {
            if (_isUsing != value)
            {
                _isUsing = value;
                if (!IsUsing)
                {
                    if (BulletObj != null)
                        BulletView.Instance.DestroyBullet(BulletObj);
                }
            }
        }
    }
    public FightUnit User = null;
    public FightUnit Target = null;
    public Vector2 CurPos = Vector2.zero;
    public float Height = 1;
    public Vector2 TargetPos = Vector2.zero;
    public BulletObj BulletObj = null;
    public int SkillLevel = 0;

    public void Update(float delta)
    {
        if (this.Target != null && !this.Target.IsDead)
            this.TargetPos = this.Target.CurPos;
        float deltaDis = this.Speed * delta;
        Vector2 dir = this.TargetPos - this.CurPos;
        if (dir.magnitude <= deltaDis)
        {
            this.CurPos = this.TargetPos;
            //射到目标位置
            if (this.Target != null && !this.Target.IsDead)
            {
                _makeEffect();
            }

            switch (this.Type)
            {
                case (int)BulletType.Normal:
                    {
                        this.IsUsing = false;
                    }
                    break;
                case (int)BulletType.Bounce:
                    {
                        if (this.Target != null && !this.Target.IsDead)
                        {
                            //弹射
                            if (BounceTimes > 0)
                            {
                                FightUnit t = FightLogic.Instance.SelectRandom(this.CurPos, this.User.IsEnemy, 7);
                                if (t != null)
                                {
                                    this.Target = t;
                                    this.TargetPos = t.CurPos;
                                    this.BounceTimes--;
                                    if (this.CSpeed < 0)
                                        this.Speed = (t.CurPos - this.User.CurPos).magnitude / -this.CSpeed;
                                }
                                else
                                {
                                    this.IsUsing = false;
                                }
                            }
                            else
                            {
                                this.IsUsing = false;
                            }
                        }
                        else
                        {
                            this.IsUsing = false;
                        }
                    }
                    break;
                case (int)BulletType.Pass:
                    {
                        //寻找下个目标
                    }
                    break;
            }
        }
        else
        {
            this.CurPos += dir.normalized * deltaDis;
        }
        if (this.IsUsing)
        {
            this.BulletObj.bullet.transform.position = new Vector3(CurPos.x, this.Height, CurPos.y);
            this.BulletObj.bullet.transform.LookAt(new Vector3(TargetPos.x, this.Height, TargetPos.y));
        }
    }

    private void _makeEffect()
    {
        if (this.User == null || this.Target == null || this.Target.IsDead)
        {
            EDebug.LogErrorFormat("BulletObj._makeEffect failed, User {0} Target {1}", this.User, this.Target);
            return;
        }


        for (int idx = 0; idx < this.Effects.Length; ++idx)
        {
            JObject ceffect = JsonMgr.GetSingleton().GetSkillEffect(this.Effects[idx]);
            int effectType = ceffect["effect"].ToObject<int>();
            if (effectType == (int)EffectType.BULLET)
            {
                EDebug.LogErrorFormat("BulletId {0} effect {1} create bullet again", this.TplId, this.Effects[idx]);
                continue;
            }
            this.User.MakeEffect(ceffect, CSkill.ID, SkillLevel, Target);
        }
    }
}

public class BulletMgr {

    private List<BulletData> _allBulletObj = new List<BulletData>();

    public bool CrateBullet(int bulletId, int skillLevel, FightUnit user, FightUnit target, Skill cskill)
    {
        var cbullet = JsonMgr.GetSingleton().GetBulletByID(bulletId);
        if (cbullet == null)
            return false;
        BulletData bullet = null;
        for (int idx = 0; idx < _allBulletObj.Count; ++idx)
        {
            BulletData tmp = _allBulletObj[idx];
            if (!tmp.IsUsing)
            {
                bullet = tmp;
                break;
            }
        }
        if (bullet == null)
        {
            bullet = new BulletData();
            _allBulletObj.Add(bullet);
        }
        bullet.TplId = bulletId;
        float cspeed = cbullet.speed;
        bullet.CSpeed = cspeed;
        if (cspeed >= 0)
            bullet.Speed = cspeed;
        else
            bullet.Speed = (target.CurPos - user.CurPos).magnitude / -cspeed;
        bullet.Type = cbullet.type;
        bullet.BounceTimes = cbullet.bounce;
        bullet.Effects = cbullet.effects;
        bullet.CSkill = cskill;
        bullet.IsUsing = true;
        bullet.User = user;
        bullet.Target = target;
        bullet.CurPos = user.CurPos;
        bullet.Height = cbullet.bulletheight;
        bullet.TargetPos = target.CurPos;
        bullet.BulletObj = BulletView.Instance.CreateBullet(cbullet.resid);
        bullet.SkillLevel = skillLevel;
        return true;
    }

    public void UpdateBullet(float delta)
    {
        for (int idx = 0; idx < _allBulletObj.Count; ++idx)
        {
            BulletData bullet = _allBulletObj[idx];
            if (!bullet.IsUsing)
                continue;
            if (FightLogic.Instance.UnitPause && !bullet.User.IsUsingActiveSkill)
                continue;
            bullet.Update(delta);
        }
    }

    public void Clear()
    {
        _allBulletObj.Clear();
        BulletView.Instance.Clear();
    }
}
