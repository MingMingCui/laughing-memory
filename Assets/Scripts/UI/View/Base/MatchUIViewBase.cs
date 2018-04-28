using UnityEngine;
using UnityEngine.UI;

public class MatchUIViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image ActiveSkill_img;
    [HideInInspector]
    public GameObject Top_obj;
    [HideInInspector]
    public Button pause_btn;
    [HideInInspector]
    public Text time_txt;
    [HideInInspector]
    public Text round_txt;
    [HideInInspector]
    public Text money_txt;
    [HideInInspector]
    public Text treasure_txt;
    [HideInInspector]
    public RectTransform treasureicon_rect;
    [HideInInspector]
    public GameObject Bottom_obj;
    [HideInInspector]
    public Button auto_fight_btn;
    [HideInInspector]
    public Image auto_fight_img;
    [HideInInspector]
    public GameObject hero_obj;
    [HideInInspector]
    public Button speed_btn;
    [HideInInspector]
    public Image speed2_img;
    [HideInInspector]
    public GameObject Info_obj;
    [HideInInspector]
    public RectTransform Treasure_rect;
    [HideInInspector]
    public GameObject Pause_obj;
    [HideInInspector]
    public Button exit_btn;
    [HideInInspector]
    public Button music_btn;
    [HideInInspector]
    public Image musicforbid_img;
    [HideInInspector]
    public Button sound_btn;
    [HideInInspector]
    public Image soundforbid_img;
    [HideInInspector]
    public Button continue_btn;
    [HideInInspector]
    public GameObject Fail_obj;
    [HideInInspector]
    public Button UpgradeHero_btn;
    [HideInInspector]
    public Button UpgradeEquip_btn;
    [HideInInspector]
    public Button UpgradeWorship_btn;
    [HideInInspector]
    public Button UpgradeStub_btn;
    [HideInInspector]
    public Button FailTryAgain_btn;
    [HideInInspector]
    public Button FailExit_btn;
    [HideInInspector]
    public Button FailDataAnalyze_btn;
    [HideInInspector]
    public GameObject PVESuccess_obj;
    [HideInInspector]
    public Image PVEStar1_img;
    [HideInInspector]
    public Image PVEStar2_img;
    [HideInInspector]
    public Image PVEStar3_img;
    [HideInInspector]
    public Text pvelevel_txt;
    [HideInInspector]
    public Text pveexp_txt;
    [HideInInspector]
    public Text pvegold_txt;
    [HideInInspector]
    public Button PVEDataAnalyze_btn;
    [HideInInspector]
    public GameObject pveheros_obj;
    [HideInInspector]
    public GameObject PVEAwards_obj;
    [HideInInspector]
    public GameObject spoils_obj;
    [HideInInspector]
    public Button PVERetry_btn;
    [HideInInspector]
    public Button PVENext_btn;
    [HideInInspector]
    public Button PVEBack_btn;
    [HideInInspector]
    public GameObject PVPSuccess_obj;
    [HideInInspector]
    public Text pvplevel_txt;
    [HideInInspector]
    public Text pvpexp_txt;
    [HideInInspector]
    public Text pvpgold_txt;
    [HideInInspector]
    public Button PVPDataAnalyze_btn;
    [HideInInspector]
    public GameObject pvpheros_obj;
    [HideInInspector]
    public Button PVPRetry_btn;
    [HideInInspector]
    public Image FightMask_img;
    [HideInInspector]
    public GameObject DataAnalyze_obj;
    [HideInInspector]
    public Button DataAnalyzeClose_btn;
    [HideInInspector]
    public GameObject Datas_obj;
    [HideInInspector]
    public GameObject Buff_obj;
    [HideInInspector]
    public GameObject herobufflist_obj;
    [HideInInspector]
    public GameObject enemybufflist_obj;
    [HideInInspector]
    public Text MaxHealth_txt;
    [HideInInspector]
    public Text StealHealth_txt;
    [HideInInspector]
    public Text CurHealth_txt;
    [HideInInspector]
    public Text VigourStimulate_txt;
    [HideInInspector]
    public Text HealthRegen_txt;
    [HideInInspector]
    public Text VigourRegen_txt;
    [HideInInspector]
    public Text AttackSpeed_txt;
    [HideInInspector]
    public Text PhysicalAttacks_txt;
    [HideInInspector]
    public Text StrategicAttack_txt;
    [HideInInspector]
    public Text Damage_txt;
    [HideInInspector]
    public Text Treatment_txt;
    [HideInInspector]
    public Text PhysicalRes_txt;
    [HideInInspector]
    public Text StrategicRes_txt;
    [HideInInspector]
    public Text AlienationRes_txt;
    [HideInInspector]
    public Text DizzinessRes_txt;
    [HideInInspector]
    public Text MisrepresentingRes_txt;
    [HideInInspector]
    public Text PhysicalPenetration_txt;
    [HideInInspector]
    public Text StrategicPenetration_txt;
    [HideInInspector]
    public Text AlienationRate_txt;
    [HideInInspector]
    public Text DizzinessRate_txt;
    [HideInInspector]
    public Text DodgeRate_txt;
    [HideInInspector]
    public Text MisrepresentingRate_txt;
    [HideInInspector]
    public Text HitRate_txt;
    [HideInInspector]
    public Text BlockRate_txt;
    [HideInInspector]
    public Text RoutRate_txt;
    [HideInInspector]
    public Text FirmRate_txt;
    [HideInInspector]
    public Text CritRate_txt;
    [HideInInspector]
    public Text CritInc_txt;
    [HideInInspector]
    public GameObject objlist_obj;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       ActiveSkill_img = mTransform.Find("ActiveSkill_img").GetComponent<Image>();
       Top_obj = mTransform.Find("Top_obj").gameObject;
       pause_btn = mTransform.Find("Top_obj/pause_btn").GetComponent<Button>();
       time_txt = mTransform.Find("Top_obj/time_bg/time_txt").GetComponent<Text>();
       round_txt = mTransform.Find("Top_obj/round_bg/round_txt").GetComponent<Text>();
       money_txt = mTransform.Find("Top_obj/money_bg/money_txt").GetComponent<Text>();
       treasure_txt = mTransform.Find("Top_obj/treasure_bg/treasure_txt").GetComponent<Text>();
       treasureicon_rect = mTransform.Find("Top_obj/treasure_bg/treasureicon_rect").GetComponent<RectTransform>();
       Bottom_obj = mTransform.Find("Bottom_obj").gameObject;
       auto_fight_btn = mTransform.Find("Bottom_obj/auto_fight_btn").GetComponent<Button>();
       auto_fight_img = mTransform.Find("Bottom_obj/auto_fight_btn/auto_fight_img").GetComponent<Image>();
       hero_obj = mTransform.Find("Bottom_obj/hero_obj").gameObject;
       speed_btn = mTransform.Find("Bottom_obj/speed_btn").GetComponent<Button>();
       speed2_img = mTransform.Find("Bottom_obj/speed_btn/speed2_img").GetComponent<Image>();
       Info_obj = mTransform.Find("Info_obj").gameObject;
       Treasure_rect = mTransform.Find("Treasure_rect").GetComponent<RectTransform>();
       Pause_obj = mTransform.Find("Pause_obj").gameObject;
       exit_btn = mTransform.Find("Pause_obj/exit_btn").GetComponent<Button>();
       music_btn = mTransform.Find("Pause_obj/music_btn").GetComponent<Button>();
       musicforbid_img = mTransform.Find("Pause_obj/music_btn/musicforbid_img").GetComponent<Image>();
       sound_btn = mTransform.Find("Pause_obj/sound_btn").GetComponent<Button>();
       soundforbid_img = mTransform.Find("Pause_obj/sound_btn/soundforbid_img").GetComponent<Image>();
       continue_btn = mTransform.Find("Pause_obj/continue_btn").GetComponent<Button>();
       Fail_obj = mTransform.Find("Fail_obj").gameObject;
       UpgradeHero_btn = mTransform.Find("Fail_obj/UpGradeBg/UpgradeHero_btn").GetComponent<Button>();
       UpgradeEquip_btn = mTransform.Find("Fail_obj/UpGradeBg/UpgradeEquip_btn").GetComponent<Button>();
       UpgradeWorship_btn = mTransform.Find("Fail_obj/UpGradeBg/UpgradeWorship_btn").GetComponent<Button>();
       UpgradeStub_btn = mTransform.Find("Fail_obj/UpGradeBg/UpgradeStub_btn").GetComponent<Button>();
       FailTryAgain_btn = mTransform.Find("Fail_obj/FailTryAgain_btn").GetComponent<Button>();
       FailExit_btn = mTransform.Find("Fail_obj/FailExit_btn").GetComponent<Button>();
       FailDataAnalyze_btn = mTransform.Find("Fail_obj/FailDataAnalyze_btn").GetComponent<Button>();
       PVESuccess_obj = mTransform.Find("PVESuccess_obj").gameObject;
       PVEStar1_img = mTransform.Find("PVESuccess_obj/PVEStar1_img").GetComponent<Image>();
       PVEStar2_img = mTransform.Find("PVESuccess_obj/PVEStar2_img").GetComponent<Image>();
       PVEStar3_img = mTransform.Find("PVESuccess_obj/PVEStar3_img").GetComponent<Image>();
       pvelevel_txt = mTransform.Find("PVESuccess_obj/TopAwardBg/pvelevel_txt").GetComponent<Text>();
       pveexp_txt = mTransform.Find("PVESuccess_obj/TopAwardBg/pveexp_txt").GetComponent<Text>();
       pvegold_txt = mTransform.Find("PVESuccess_obj/TopAwardBg/pvegold_txt").GetComponent<Text>();
       PVEDataAnalyze_btn = mTransform.Find("PVESuccess_obj/TopAwardBg/PVEDataAnalyze_btn").GetComponent<Button>();
       pveheros_obj = mTransform.Find("PVESuccess_obj/pveheros_obj").gameObject;
       PVEAwards_obj = mTransform.Find("PVESuccess_obj/PVEAwards_obj").gameObject;
       spoils_obj = mTransform.Find("PVESuccess_obj/PVEAwards_obj/spoils_obj").gameObject;
       PVERetry_btn = mTransform.Find("PVESuccess_obj/PVERetry_btn").GetComponent<Button>();
       PVENext_btn = mTransform.Find("PVESuccess_obj/PVENext_btn").GetComponent<Button>();
       PVEBack_btn = mTransform.Find("PVESuccess_obj/PVEBack_btn").GetComponent<Button>();
       PVPSuccess_obj = mTransform.Find("PVPSuccess_obj").gameObject;
       pvplevel_txt = mTransform.Find("PVPSuccess_obj/TopAwardBg/pvplevel_txt").GetComponent<Text>();
       pvpexp_txt = mTransform.Find("PVPSuccess_obj/TopAwardBg/pvpexp_txt").GetComponent<Text>();
       pvpgold_txt = mTransform.Find("PVPSuccess_obj/TopAwardBg/pvpgold_txt").GetComponent<Text>();
       PVPDataAnalyze_btn = mTransform.Find("PVPSuccess_obj/TopAwardBg/PVPDataAnalyze_btn").GetComponent<Button>();
       pvpheros_obj = mTransform.Find("PVPSuccess_obj/pvpheros_obj").gameObject;
       PVPRetry_btn = mTransform.Find("PVPSuccess_obj/PVPRetry_btn").GetComponent<Button>();
       FightMask_img = mTransform.Find("FightMask_img").GetComponent<Image>();
       DataAnalyze_obj = mTransform.Find("DataAnalyze_obj").gameObject;
       DataAnalyzeClose_btn = mTransform.Find("DataAnalyze_obj/DataAnalyzeClose_btn").GetComponent<Button>();
       Datas_obj = mTransform.Find("DataAnalyze_obj/Datas/Datas_obj").gameObject;
       Buff_obj = mTransform.Find("Buff_obj").gameObject;
       herobufflist_obj = mTransform.Find("Buff_obj/BuffBg/HeroBuff/herobufflist_obj").gameObject;
       enemybufflist_obj = mTransform.Find("Buff_obj/BuffBg/EnemyBuff/enemybufflist_obj").gameObject;
       MaxHealth_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text/MaxHealth_txt").GetComponent<Text>();
       StealHealth_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (1)/StealHealth_txt").GetComponent<Text>();
       CurHealth_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (2)/CurHealth_txt").GetComponent<Text>();
       VigourStimulate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (3)/VigourStimulate_txt").GetComponent<Text>();
       HealthRegen_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (4)/HealthRegen_txt").GetComponent<Text>();
       VigourRegen_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (5)/VigourRegen_txt").GetComponent<Text>();
       AttackSpeed_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (6)/AttackSpeed_txt").GetComponent<Text>();
       PhysicalAttacks_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (7)/PhysicalAttacks_txt").GetComponent<Text>();
       StrategicAttack_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (8)/StrategicAttack_txt").GetComponent<Text>();
       Damage_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (9)/Damage_txt").GetComponent<Text>();
       Treatment_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (10)/Treatment_txt").GetComponent<Text>();
       PhysicalRes_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (11)/PhysicalRes_txt").GetComponent<Text>();
       StrategicRes_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (12)/StrategicRes_txt").GetComponent<Text>();
       AlienationRes_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (13)/AlienationRes_txt").GetComponent<Text>();
       DizzinessRes_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (14)/DizzinessRes_txt").GetComponent<Text>();
       MisrepresentingRes_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (15)/MisrepresentingRes_txt").GetComponent<Text>();
       PhysicalPenetration_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (16)/PhysicalPenetration_txt").GetComponent<Text>();
       StrategicPenetration_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (17)/StrategicPenetration_txt").GetComponent<Text>();
       AlienationRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (18)/AlienationRate_txt").GetComponent<Text>();
       DizzinessRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (19)/DizzinessRate_txt").GetComponent<Text>();
       DodgeRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (20)/DodgeRate_txt").GetComponent<Text>();
       MisrepresentingRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (21)/MisrepresentingRate_txt").GetComponent<Text>();
       HitRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (22)/HitRate_txt").GetComponent<Text>();
       BlockRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (23)/BlockRate_txt").GetComponent<Text>();
       RoutRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (24)/RoutRate_txt").GetComponent<Text>();
       FirmRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (25)/FirmRate_txt").GetComponent<Text>();
       CritRate_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (26)/CritRate_txt").GetComponent<Text>();
       CritInc_txt = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/Text (27)/CritInc_txt").GetComponent<Text>();
       objlist_obj = mTransform.Find("Buff_obj/BuffBg/ShowBuffData/obj/objlist_obj").gameObject;
    }
}