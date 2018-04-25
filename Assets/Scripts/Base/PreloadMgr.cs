using JsonData;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadMgr : Singleton<PreloadMgr> {

    public List<int> GetPreloadList(string sceneName)
    {
        List<int> ret = new List<int>();
        HashSet<int> ui = new HashSet<int>();
        HashSet<int> sound = new HashSet<int>();
        HashSet<int> gos = new HashSet<int>();
        switch (sceneName)
        {
            case "Login":
                //ret.Add(1011);
                //ret.Add(100001);
                ui.Add(1002);
                sound.Add(100001);
                break;
            case "Main":
                //ret.Add(1012);
                //ret.Add(1023);
                //ret.Add(1049);
                //ret.Add(100002);
                ret.Add(CamMgr.FightCamEffect);
                ui.Add(1003);
                ui.Add(1005);
                ui.Add(1006);
                sound.Add(100002);
                var stubData = Role.Instance.GetStubData(StubType.PVE);
                for (int idx = 0; idx < stubData.Count; ++idx)
                {
                    Hero heroJson = JsonMgr.GetSingleton().GetHeroByID(stubData[idx].y);
                    gos.Add(heroJson.resid);
                }
                break;
            case "Game":
                {
                    gos.Add(1048);      //宝箱
                    ui.Add(1004);
                    sound.Add(100003);
                    //场景
                    gos.Add(10100);
                    gos.Add(10101);
                    gos.Add(10102);
                    gos.Add(FightUnitView.MOVE_EFFECT);

                    _analyzeFightersRes(FightLogic.Instance.Fighters, ref sound, ref gos);
                    for (int idx = 0; idx < FightLogic.Instance.EnemyFighters.Count; ++idx)
                        _analyzeFightersRes(FightLogic.Instance.EnemyFighters[idx], ref sound, ref gos);
                }
                break;
            default:
                break;
        }
        foreach (int uiid in ui)
        {
            UIConfig jUI = JsonMgr.GetSingleton().GetUIConfigByID(uiid);
            ret.Add(jUI.Resid);
        }
        //ret.AddRange(ui);
        ret.AddRange(sound);
        ret.AddRange(gos);
        ret.Sort();
        //Debug.Log("Preload~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        //for(int idx = 0; idx < ret.Count; ++idx)
        //{
        //    Debug.LogFormat("Load {0}", ret[idx]);
        //}
        //Debug.Log("Preload End~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        return ret.Count != 0 ? ret : null;
    }

    private void _analyzeFightersRes(List<FightUnit> fighterList, ref HashSet<int> sound, ref HashSet<int> gos)
    {
        for (int idx = 0; idx < fighterList.Count; ++idx)
            _analyzeFighterRes(fighterList[idx].HeroId, fighterList[idx].IsMonster, ref sound, ref gos);
    }

    private void _analyzeFighterRes(int fightUnitId, bool isMonster, ref HashSet<int> sound, ref HashSet<int> gos)
    {
        int heroResId = 0;
        int heroHorseId = 0;
        List<int> skills = new List<int>();

        if (isMonster)
        {
            Monster monster = JsonMgr.GetSingleton().GetMonsterByID(fightUnitId);
            heroResId = monster.resid;
            heroHorseId = monster.horseid;
            skills.AddRange(monster.skills);
        }
        else
        {
            Hero hero = JsonMgr.GetSingleton().GetHeroByID(fightUnitId);
            heroResId = hero.resid;
            heroHorseId = hero.horseid;
            skills.AddRange(hero.skills);

        }
        //if (jHero == null)
        //{
        //    EDebug.LogErrorFormat("PreloadMgr._analyzeFighterRes failed, no such hero:{0}", fightUnitId);
        //    return;
        //}

        JsonMgr.GetSingleton().GetHeroByID(fightUnitId);

        gos.Add(heroResId);
        if (heroHorseId != 0)
            gos.Add(heroHorseId);
        //技能特效
        for (int idx = 0; idx < skills.Count; ++idx)
        {
            int skillId = skills[idx];
            _analyzeSkillRes(skillId,  ref sound, ref gos);
        }
        //音效
        for (int idx = 1; idx <= 5; ++idx)
        {
            int soundId = heroResId * 10 + idx;
            var soundJson = JsonMgr.GetSingleton().GetSoundArrayByID(soundId);
            if(soundJson != null)
                sound.Add(soundId);
        }
    }

    private void _analyzeSkillRes(int skillId, ref HashSet<int> sound, ref HashSet<int> gos)
    {
        Skill jSkill = JsonMgr.GetSingleton().GetSkillByID(skillId);
        if (jSkill == null)
        {
            EDebug.LogErrorFormat("PreloadMgr._analyzeSkillRes failed, no such skill:{0}", skillId);
            return;
        }
        //技能特效
        int startEffect = jSkill.starteffect;
        int fulleffect = jSkill.fulleffect;
        int attackeffect = jSkill.attackeffect;
        if (startEffect != 0)
            gos.Add(startEffect);
        if (fulleffect != 0)
            gos.Add(fulleffect);
        if (attackeffect != 0)
            gos.Add(attackeffect);
        //分析技能效果
        int[] skillEffects = jSkill.effects;
        for (int idx = 0; idx < skillEffects.Length; ++idx)
        {
            int effectId = skillEffects[idx];
            _analyzeEffect(effectId, ref sound, ref gos);
        }
    }

    private void _analyzeEffect(int effectId, ref HashSet<int> sound, ref HashSet<int> gos)
    {
        JObject jEffect = JsonMgr.GetSingleton().GetSkillEffect(effectId);
        if (jEffect == null)
        {
            EDebug.LogErrorFormat("PreloadMgr._analyzeEffect failed, no such effect:{0}", effectId);
            return;
        }
        int hitEffect = jEffect["hiteffect"].ToObject<int>();
        if (hitEffect != 0)
            gos.Add(hitEffect);
        int effect = jEffect["effect"].ToObject<int>();
        int paramType = jEffect["paramtype"].ToObject<int>();
        if (1 == paramType)
            return;
        Vector3 EffectParams = AttrUtil.CalExpression(paramType, jEffect["effectparam"].ToObject<JArray>());
        switch (effect)
        {
            case (int)EffectType.BULLET:
                _analyzeBullet((int)EffectParams.y, ref sound, ref gos);
                break;
            case (int)EffectType.BUFF:
                _analyzeBuffRes((int)EffectParams.y, ref sound, ref gos);
                break;
            case (int)EffectType.Summon:
                _analyzeFighterRes((int)EffectParams.x, true, ref sound, ref gos);
                break;
        }
    }

    private void _analyzeBullet(int bulletId, ref HashSet<int> sound, ref HashSet<int> gos)
    {
        var jBullet = JsonMgr.GetSingleton().GetBulletByID(bulletId);
        if (jBullet == null)
        {
            EDebug.LogErrorFormat("PreloadMgr._analyzeBullet failed, no such bullet:{0}", bulletId);
            return;
        }
        int resId = jBullet.resid;
        if (resId != 0)
            gos.Add(resId);
        int[] effects = jBullet.effects;
        for (int idx = 0; idx < effects.Length; ++idx)
            _analyzeEffect(effects[idx], ref sound, ref gos);
    }

    private void _analyzeBuffRes(int buffId, ref HashSet<int> sound, ref HashSet<int> gos)
    {
        JObject jBuff = JsonMgr.GetSingleton().GetBuff(buffId);
        if (jBuff == null)
        {
            EDebug.LogErrorFormat("PreloadMgr._analyzeBuff failed, no such buff:{0}", buffId);
            return;
        }
        int resId = jBuff["resid"].ToObject<int>();
        if (resId != 0)
            gos.Add(resId);
        int effect = jBuff["effect"].ToObject<int>();
        int calType = jBuff["caltype"].ToObject<int>();
        if (1 == calType)
            return;
        Vector3 buffParams = AttrUtil.CalExpression(calType, jBuff["buffparam"].ToObject<JArray>());
        switch (effect)
        {
            case (int)BuffEffect.UsePromotSkill:
            case (int)BuffEffect.UseSkill:
                _analyzeSkillRes((int)buffParams.y, ref sound, ref gos);
                break;
            case (int)BuffEffect.AddBuff:
                _analyzeBuffRes((int)buffParams.y, ref sound, ref gos);
                break;
        }
    }
}
