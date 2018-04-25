using JsonData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightHeroView : MonoBehaviour {
    [HideInInspector]
    public Image color_img = null;
    [HideInInspector]
    public Image head_img = null;
    [HideInInspector]
    public Image hp_img = null;
    [HideInInspector]
    public Image vigour_img = null;
    [HideInInspector]
    public StarView star_view = null;
    [HideInInspector]
    public Transform mTransform = null;
    [HideInInspector]
    public Button fightHero_btn = null;
    [HideInInspector]
    public GameObject fire_obj = null;
    [HideInInspector]
    public GameObject vigour_effect = null;
    [HideInInspector]
    public GameObject dead_img = null;

    public FightUnit Unit = null;

    private bool _isActiveSkillEnable = false;
    public bool IsActiveSkillEnable
    {
        get { return _isActiveSkillEnable; }
        set {
            if (_isActiveSkillEnable != value)
            {
                _isActiveSkillEnable = value;
                this.fire_obj.SetActive(_isActiveSkillEnable);
            }
        }
    }
	// Use this for initialization
	public void Init () {
        this.mTransform = this.transform;
        this.color_img = mTransform.Find("color_img").GetComponent<Image>();
        this.head_img = mTransform.Find("head_img").GetComponent<Image>();
        this.fightHero_btn = head_img.gameObject.GetComponent<Button>();
        this.star_view = mTransform.Find("stars_obj").GetComponent<StarView>();
        this.hp_img = mTransform.Find("hp_img").GetComponent<Image>();
        this.vigour_img = mTransform.Find("vigour_img").GetComponent<Image>();
        this.fire_obj = mTransform.Find("fire_obj").gameObject;
        this.vigour_effect = mTransform.Find("VigourEffect_obj").gameObject;
        this.dead_img = mTransform.Find("dead_img").gameObject;
        this.hp_img.fillAmount = 1;
        this.vigour_img.fillAmount = 0;
	}

    public void InitHero(FightUnit unit)
    {
        this.Unit = unit;
        this.star_view.SetStar(unit.Star);
        int headBorder = JsonMgr.GetSingleton().GetHeroRareByID(unit.Rare).FightHeadBorder;
        this.color_img.sprite = ResourceMgr.Instance.LoadSprite(headBorder);
        Hero jHero = JsonMgr.GetSingleton().GetHeroByID(unit.HeroId);
        int headId = jHero.headid;
        if (headId != 0)
            this.head_img.sprite = ResourceMgr.Instance.LoadSprite(headId);
    }

    public void OnFightUnitHpChange(FightUnit unit)
    {
        if (unit != this.Unit)
            return;
        this.hp_img.fillAmount = unit.CurHP / (float)unit.MaxHP;
    }

    public void OnFightUnitVigourChange(FightUnit unit)
    {
        if (unit != this.Unit)
            return;
        float vigourFillAmount = unit.Vigour / (float)unit.MaxVigour;
        this.vigour_img.fillAmount = vigourFillAmount;
        vigour_effect.SetActive(vigourFillAmount >= 1);
    }

    public void OnFightStateChange(FightState state)
    {
    }

    public void OnFightTargetChange(FightUnit unit, FightUnit target)
    {
        if (unit != this.Unit && unit != this.Unit.FightTarget)
            return;
    }

    public void OnFightUnitDie(FightUnit unit)
    {
        if (unit != this.Unit)
            return;
        this.dead_img.SetActive(true);
    }

    private void _reCalActiveSkillEnableState()
    {
        IsActiveSkillEnable = this.Unit.SkillMgrObj.IsActiveSkillEnable(this.Unit);
    }

    private void Update()
    {
        if (this.Unit != null)
            _reCalActiveSkillEnableState();
    }


}
