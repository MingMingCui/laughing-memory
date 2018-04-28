using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopTextDir
{
    Up              = 0,
    LEFT            = 1,
    RIGHT           = 2,
}

public class FightUnitInfoView : MonoBehaviour {

    [HideInInspector]
    public RectTransform Info_obj = null;
    [HideInInspector]
    public CanvasGroup Hp_group = null;
    [HideInInspector]
    public Image Hp_self_pro = null;
    [HideInInspector]
    public Image Hp_enemy_pro = null;
    [HideInInspector]
    public Image Follow_pro = null;
    [HideInInspector]
    public PopTextMgr PopText = null;
    [HideInInspector]
    public PopTextMgr PopTextLeft = null;
    [HideInInspector]
    public PopTextMgr PopTextRight = null;
    [HideInInspector]
    public FightUnit Unit = null;
    [HideInInspector]
    public Camera UICamera = null;

    public static readonly float FOLLOW_SPEED = 0.3f;
    public static readonly float FADE_SPEED = 2f;
    public static readonly float FADE_TIME = 1.0f;
    public static readonly Vector3 OUT_SCREEN = new Vector3(99999, 99999, 0);
    public static readonly float POP_GAP = 0.1f;
    private float startFade = 0;

    private struct PopObj
    {
        public int style;
        public string info;
        public PopTextDir Dir;
    }

    private Queue<PopObj> _popObjs = new Queue<PopObj>();
    private float _lastPopTime = 0;

    // Use this for initialization
    void Start () {
        Info_obj = this.transform.GetComponent<RectTransform>();
        Hp_group = this.transform.Find("hp_group").GetComponent<CanvasGroup>();
        Hp_self_pro = this.transform.Find("hp_group/hp_self_pro").GetComponent<Image>();
        Hp_enemy_pro = this.transform.Find("hp_group/hp_enemy_pro").GetComponent<Image>();
        Follow_pro = this.transform.Find("hp_group/follow_pro").GetComponent<Image>();
        PopText = this.transform.Find("PopText").GetComponent<PopTextMgr>();
        PopTextLeft = this.transform.Find("PopTextLeft").GetComponent<PopTextMgr>();
        PopTextRight = this.transform.Find("PopTextRight").GetComponent<PopTextMgr>();
        Hp_group.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {

        Image CurHpPro = this.Unit.IsEnemy ? this.Hp_enemy_pro : this.Hp_self_pro;
        if (Unit != null)
        {
            Vector3 followPoint = OUT_SCREEN;
            //if (!Unit.IsDead)
            {
                Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(new Vector3(Unit.CurPos.x, Unit.HorseId != 0 ? 3.7f : 2.6f, Unit.CurPos.y));
                if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(Info_obj, targetScreenPosition, CanvasView.Instance.uicamera, out followPoint))
                {
                    Debug.LogErrorFormat("计算位置失败：{0}", Unit.UID);
                }
            }
            Info_obj.position = followPoint;

            //更新Hp
            float newFillAmount = this.Unit.CurHP / (float)this.Unit.MaxHP;
            CurHpPro.fillAmount = newFillAmount;

            //HP_FADE
            if (Hp_group.alpha > 0)
            {
                if (Time.time > startFade)
                {
                    Hp_group.alpha = Mathf.Max(0, Hp_group.alpha - FADE_SPEED * Time.deltaTime);
                }
            }

        }

        float fill_dis = CurHpPro.fillAmount - Follow_pro.fillAmount;
        if (fill_dis >= 0)
        {
            Follow_pro.fillAmount = CurHpPro.fillAmount;
        }
        else
        {
            float fill_delta = FOLLOW_SPEED * Time.deltaTime;
            if (fill_delta >= -fill_dis)
                Follow_pro.fillAmount = CurHpPro.fillAmount;
            else
                Follow_pro.fillAmount -= fill_delta;
        }

        //处理冒字
        if (Time.time - _lastPopTime >= POP_GAP && _popObjs.Count > 0)
        {
            PopObj obj = _popObjs.Dequeue();
            PopTextMgr mgr = obj.Dir == PopTextDir.Up ? this.PopText : (obj.Dir == PopTextDir.LEFT ? this.PopTextLeft : this.PopTextRight);
            mgr.AddInfo(obj.info, obj.style);
            _lastPopTime = Time.time;
        }

    }

    public void SetFightUnit(FightUnit u)
    {
        this.Unit = u;
    }


    /// <summary>
    /// 显示冒字
    /// 冒字规则:
    /// 0、己方伤害(包含未命中)
    /// 1、敌方伤害(包含未命中、物免魔免)
    /// 2、治疗生命
    /// 3、增加能量、金钱
    /// 4、其他buff
    /// 汉字:A、暴击 B、格挡 C、闪避 D、未命中 E、胆裂 F、混乱 G、击晕
    /// H、决斗 I、伪报 J、加速 K、减速 L、回复血量 M、回复士气 /*N、击杀奖励*/
    /// O、降低攻击 P、降低攻速 Q、降低护甲 R、谋略免疫 S、物理免疫 T、增加攻击
    /// U、增加攻速 V、增加护甲 W、增加铜钱
    /// </summary>
    /// <param name="data"></param>
    public void SetPopInfo(Vector2Int data)
    {
        string info = string.Format("{0}{1}", _getCharFromState(data.x), (data.y > 0) ? "+" + data.y.ToString() : ((data.y < 0) ? data.y.ToString() : ""));
        int style = _getStyleFromState(data, this.Unit.IsEnemy);
        //this.PopText.AddInfo(info, style);
        PopObj obj = new PopObj();
        obj.info = info;
        obj.style = style;
        obj.Dir = _getPopTextDir(data, this.Unit.IsEnemy);
        _popObjs.Enqueue(obj);
        Hp_group.alpha = 1;
        startFade = Time.time + FADE_TIME;
    }

    public string _getCharFromState(int data)
    {
        switch (data)
        {
            case (int)EffectState.None:
                return "";
            case (int)EffectState.Crit:
            case (int)EffectState.HealCrit:
                return "A";
            case (int)EffectState.Dodge:
                return "C";
            case (int)EffectState.Block:
                return "B";
            case (int)EffectState.HealVigour:
                return "M";
            default:
                return "";
        }
    }

    public int _getStyleFromState(Vector2Int data, bool isEnemy)
    {
        //TODO:其他增加数值的情况
        switch (data.x)
        {
            case (int)EffectState.None:
            case (int)EffectState.Crit:
            case (int)EffectState.Block:
            case (int)EffectState.Dodge:
            case (int)EffectState.Miss:
                return isEnemy ? 1 : 0;
            case (int)EffectState.Heal:
            case (int)EffectState.HealCrit:
                return 2;
            case (int)EffectState.HealVigour:
            case (int)EffectState.AddGold:
                return 3;
            default:
                return 4;
        }
    }

    public PopTextDir _getPopTextDir(Vector2Int data, bool isEnemy)
    {
        if (data.x == (int)EffectState.None || data.x == (int)EffectState.Heal || data.x == (int)EffectState.HealVigour)
            return PopTextDir.Up;
        else
            return isEnemy ? PopTextDir.RIGHT : PopTextDir.LEFT;
    }
    public void Clear()
    {
        this.Unit = null;
    }
}
