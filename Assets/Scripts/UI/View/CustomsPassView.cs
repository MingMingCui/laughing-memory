using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CustomsPassView: CustomsPassViewBase
{
    [HideInInspector]
    
    public EnemyView EnemyIco;
    public ItemUIView Drop_Tip;
    [HideInInspector]
    public List<EnemyView> EnemyIcoList = new List<EnemyView>();
    [HideInInspector]
    public List<ItemUIView> Drop_TipList = new List<ItemUIView>();
    [HideInInspector]
    public int Power;
    public void Init()
    {
        BattleMgr.Instance.isOff = false;
        int stra = BattleMgr.Instance.GetStar(BattleMgr.Instance.LevelID);
        LevelData data = JsonMgr.GetSingleton().GetLevel(BattleMgr.Instance.LevelID);
        Power = data.power;
        intro_txt.text = data.desc; //简介
        power_txt.color = Role.Instance.Power < data.power ? Color.red : Color.white;
        power_txt.text = data.power.ToString();//体力消耗
        combat_txt.text = data.combat_effect.ToString();
        name_txt.text = data.name;
        //  InitItemInfo()
        EnemyView enemyico = null;
        for (int idx = 0; idx < data.monster_tip.Length; idx++)
        {
            if (idx == EnemyIcoList.Count)
            {
                enemyico = InitItemInfo(EnemyIco.gameObject, enemyShow_obj).GetComponent<EnemyView>();
                enemyico.Init();
                EnemyIcoList.Add(enemyico);
            }
            else
                enemyico = EnemyIcoList[idx];

            if (idx == data.monster_tip.Length - 1)
            {
                enemyico.gameObject.GetComponent<LayoutElement>().preferredHeight = 190;
                enemyico.gameObject.GetComponent<LayoutElement>().preferredWidth = 190;
                enemyico.Endow(data.monster_tip[idx] , true);
            }
            else
                enemyico.Endow(data.monster_tip[idx], false);
            enemyico.isShow = true;
        }

        ItemUIView drop_tip = null;
        for (int idx = 0; idx < data.drop_tip.Length; idx++)
        {
            if (idx == Drop_TipList.Count)
            {
                drop_tip = InitItemInfo(Drop_Tip.gameObject, dropOutShow_obj).GetComponent<ItemUIView>();
                drop_tip.Init();
                Drop_TipList.Add(drop_tip);
            }
            else
                drop_tip = Drop_TipList[idx];
            drop_tip.isShow = true;
            drop_tip.SetInfo(data.drop_tip[idx], 0);
        }
        switch (stra) //根据本关卡获得星数对关卡显示界面星星赋值
        {
            case 0:
                star_1_img.color = new Color(star_1_img.color.r, star_1_img.color.g, star_1_img.color.b, 0);
                star_2_img.color = new Color(star_2_img.color.r, star_2_img.color.g, star_2_img.color.b, 0);
                star_3_img.color = new Color(star_3_img.color.r, star_3_img.color.g, star_3_img.color.b, 0);
                break;
            case 1:
                star_1_img.color = new Color(star_1_img.color.r, star_1_img.color.g, star_1_img.color.b, 1);
                star_2_img.color = new Color(star_2_img.color.r, star_2_img.color.g, star_2_img.color.b, 0);
                star_3_img.color = new Color(star_3_img.color.r, star_3_img.color.g, star_3_img.color.b, 0);
                break;
            case 2:
                star_1_img.color = new Color(star_1_img.color.r, star_1_img.color.g, star_1_img.color.b, 1);
                star_2_img.color = new Color(star_2_img.color.r, star_2_img.color.g, star_2_img.color.b, 1);
                star_3_img.color = new Color(star_3_img.color.r, star_3_img.color.g, star_3_img.color.b, 0);
                break;
            case 3:
                star_1_img.color = new Color(star_1_img.color.r, star_1_img.color.g, star_1_img.color.b, 1);
                star_2_img.color = new Color(star_2_img.color.r, star_2_img.color.g, star_2_img.color.b, 1);
                star_3_img.color = new Color(star_3_img.color.r, star_3_img.color.g, star_3_img.color.b, 1);
                break;
        }
        if (data.times == 1) // 精英及史诗副本通关次数如果小于1
        {
            time_obj.SetActive(false);
            // CustomsPass_obj.SetActive(true);
        }
        else 
        {
            for (int idx2 = 0; idx2 < BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].GenStage.Count; idx2++)
            {
                if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].GenStage[idx2].LevelID == BattleMgr.Instance.LevelID)
                {
                    if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].GenStage[idx2].ResidueTime == 0) //剩余次数
                    {
                        nowTime_txt.text = "0";
                    }
                    else
                    {
                        if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].GenStage[idx2].isZeki)
                        {
                            time_obj.SetActive(true);
                            Total_txt.text = "/" + data.times.ToString();
                            nowTime_txt.text = BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].GenStage[idx2].ResidueTime.ToString();
                        }
                    }
                    // CustomsPass_obj.SetActive(true);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 实例化物品信息
    /// </summary>
    public GameObject InitItemInfo(Object item, GameObject parent )
    {
        GameObject _item = GameObject.Instantiate((GameObject)item);
        _item.transform.SetParent(parent.transform);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }


}
