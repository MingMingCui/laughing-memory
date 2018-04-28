using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonData;

public class BattleData
{
    public int ChapterId;
    public int StageAwardNum;
    public int Stage1;
    public int[] Stage1Award;
    public int Stage2;
    public int[] Stage2Award;
    public int Stage3;
    public int[] Stage3Award;

}

public class LevelData
{
    public int id; //关卡ID
    public string name; //副本名称
    public bool type;    //类型
    public int level;   //等级限制
    public int power;   //消耗的体力
    public int times; //挑战次数
    public int[] monster;  //怪物
    public string desc;   //关卡简介
    public int[] monster_tip; //怪物提示
    public int[] drop_tip;    //掉落提示
    public int combat_effect;   //战斗力提示
    public int script;    //剧情脚本
}


//public class LevelMonster
//{
//    public int id; //怪物表id
//    public int[] monster; //怪物id
//    public int[] position; //位置
//}
/// <summary>
/// Json解析，增加新json请遵循步骤1-4
/// </summary>
public class JsonMgr
{
    private static JsonMgr _json;
    public static JsonMgr GetSingleton()
    {
        return _json ?? (_json = new JsonMgr());
    }
    //TODO:步骤1，定义json引用
    private string[] _allJsonNames =
    {
           //"Res",
           //"Sprite",
           "Buff",
           "activity_sign",
           "Award",
           "Random",
           "Section",
           "Battle",
           //"Monsters",
           //"GM",
 
           "SkillEffect",
           //"Bullet",
           //"GlobalValue",
           //"Sound",
           //"DropOrder",
           //"GlobalStr",
    };

    #region
    private Dictionary<string, Dictionary<int, JObject>> _allJsonData = new Dictionary<string, Dictionary<int, JObject>>();

    //private Dictionary<int, ItemData> _itemDic = new Dictionary<int, ItemData>();
    private List<List<BattleData>> Chapter = new List<List<BattleData>>();
    private List<List<List<LevelData>>> Level = new List<List<List<LevelData>>>();
    //private List<LevelMonster> LevelMonster = new List<LevelMonster>();
    private List<string> Surname = new List<string>();
    private List<string> Man = new List<string>();
    private List<string> Woman = new List<string>();
    //private List<Vector2Int> _dropOrderList = new List<Vector2Int>();
    private List<Buff> buff = new List<Buff>();
    #endregion

    private JsonMgr()
    {
        init();
        RunLoadJson();
        //ProcessCtrl.Instance.GoCoroutine("RunLoadJson", RunLoadJson());
    }

    private void init()
    {
        loadData();

        //需要预处理的数据
        #region
        //掉落排序表处理
        //Dictionary<int, JObject> _dropOrderBuffer = getWholeData("DropOrder");
        //foreach (var p in _dropOrderBuffer)
        //{
        //    _dropOrderList.Add(new Vector2Int(p.Value["x"].ToObject<int>(), p.Value["y"].ToObject<int>()));
        //}

        //章节表
        Dictionary<int, JObject> _battleBuffer = getWholeData("Battle");
        int idx = 0;
        foreach (var p in _battleBuffer)
        {
            BattleData battleData = new BattleData
            {
                ChapterId = p.Key,
                StageAwardNum = p.Value["StageAwardNum"].ToObject<int>(),
                Stage1 = p.Value["Stage1"].ToObject<int>(),
                Stage1Award = Util.Str2IntArr(p.Value["Stage1Award"].ToString()),
                Stage2 = p.Value["Stage2"].ToObject<int>(),
                Stage2Award = Util.Str2IntArr(p.Value["Stage2Award"].ToString()),
                Stage3 = p.Value["Stage3"].ToObject<int>(),
                Stage3Award = Util.Str2IntArr(p.Value["Stage3Award"].ToString())
            };
            float s = p.Key - (idx + 1);
            if (s % 2 != 0) idx = 0;
            if ((p.Key - (idx + 1)) * 0.01f > Chapter.Count) Chapter.Add(new List<BattleData>());
            int id = int.Parse(((p.Key - (idx + 1)) * 0.01f).ToString());
            Chapter[id - 1].Add(battleData);
            idx++;
        }
        //关卡表
        Dictionary<int, JObject> _levelBuffer = getWholeData("Section");
        for (int idx1 = 0; idx1 < Chapter.Count; idx1++)
        {
            Level.Add(new List<List<LevelData>>());
            for (int idx2 = 0; idx2 < Chapter[idx1].Count; ++idx2)
            {
                Level[idx1].Add(new List<LevelData>());
                int idx3 = 1;
                while (true)
                {
                    int levelId = 0;
                    int i = idx1 + 1;
                    if (idx3 < 10)
                        levelId = int.Parse(String.Concat((i * 100 + (idx2 + 1)).ToString(), "0", idx3));
                    else
                        levelId = int.Parse(String.Concat((i * 100 + (idx2 + 1)).ToString(), idx3));

                    if (!_levelBuffer.Keys.Contains(levelId) && Level[idx1][idx2].Count != 0)
                        break;
                    LevelData levelData = new LevelData
                    {
                        id = levelId, //关卡ID
                        name = _levelBuffer[levelId]["name"].ToString(), //副本名称
                        level = _levelBuffer[levelId]["level"].ToObject<int>(),//等级限制
                        power = _levelBuffer[levelId]["power"].ToObject<int>(), //消耗的体力
                        times = _levelBuffer[levelId]["times"].ToObject<int>(), //挑战次数
                        monster = Util.Str2IntArr(_levelBuffer[levelId]["monster"].ToString()), //怪物
                        desc = _levelBuffer[levelId]["desc"].ToString(),   //关卡简介
                        monster_tip = Util.Str2IntArr(_levelBuffer[levelId]["monster_tip"].ToString()), //怪物提示
                        drop_tip = Util.Str2IntArr(_levelBuffer[levelId]["drop_tip"].ToString()),  //掉落提示
                        combat_effect = _levelBuffer[levelId]["combat_effect"].ToObject<int>(), //战斗力提示
                                                                                                // script = _levelBuffer[levelId]["script"].ToObject<int>()  //剧情脚本
                    };
                    if (_levelBuffer[levelId]["type"].ToObject<int>() == 1) //类型
                        levelData.type = true;
                    else
                        levelData.type = false;
                    //  levelData.name = _levelBuffer[levelId]["StageName"].ToString();
                    // levelData.Intro = _levelBuffer[levelId]["Dese"].ToString();
                    Level[idx1][idx2].Add(levelData);
                    idx3++;
                }
            }
        }
        ////关卡怪物
        //Dictionary<int, JObject> _LevelMonster = getWholeData("Monsters");
        //foreach (var item in _LevelMonster)
        //{
        //    LevelMonster monster = new LevelMonster
        //    {
        //        id = item.Key,//怪物表id
        //        monster = Util.Str2IntArr(item.Value["monster"].ToString()),//怪物id
        //        position = Util.Str2IntArr(item.Value["position"].ToString())//位置
        //    };
        //    LevelMonster.Add(monster);
        //}



        Dictionary<int, JObject> Random = getWholeData("Random");
        foreach (var item in Random)
        {
            if (item.Value["Surname"].ToString() != "")
            {
                Surname.Add(item.Value["Surname"].ToString());
            }
            if (item.Value["Man"].ToString() != "")
            {
                Man.Add(item.Value["Man"].ToString());
            }
            if (item.Value["Woman"].ToString() != "")
            {
                Woman.Add(item.Value["Woman"].ToString());
            }

        }
        #endregion
    }

    #region
    //TODO:步骤2,根据需要添加获取单个json信息方法
    //public JObject GetResArrayByID(int id) { return getSingleData("Res", id, true, "Sound"); }
    //public JObject GetSpriteArrayByID(int id) { return getSingleData("Sprite", id); }
    //public JObject GetGameManagerByID(int id) { return getSingleData("GM", id); }
    //public JObject GetUI(int id) { return getSingleData("UI", id); }
    public JObject GetBuff(int id) { return getSingleData("Buff", id); }
    //public JObject GetItem(int id) { return getSingleData("Item", id); }
    //public JObject GetSkill(int id) { return getSingleData("Skill", id); }
    public JObject Getactivity_sign(int id) { return getSingleData("activity_sign", id); }
    public JObject GetAward(int id) { return getSingleData("Award",id); }
    public JObject GetSkillEffect(int id) { return getSingleData("SkillEffect", id); }
    //public JObject GetBulletByID(int id) { return getSingleData("Bullet", id); }
    public JObject GetBattle(int id) { return getSingleData("Battle", id); }
    //public JObject GetCopyLevelByID(int id) { return getSingleData("Section", id); }

    //public float GetGlobalIntArrayByID(int id)
    //{
    //    JObject global = getSingleData("GlobalValue", id);
    //    return global["value"].ToObject<float>();
    //}
    //public JObject GetSoundArrayByID(int id) {
    //    return getSingleData("Sound", id, false);
    //}
    //public string GetGlobalStringArrayByID(int id) {
    //    JObject global = getSingleData("GlobalStr", id);
    //    return global["desc"].ToString();
    //}
    #endregion


    //#region
    ////TODO:步骤3，根据需要添加特殊处理方法
    //public int GetDropOrderCnt()
    //{
    //    return _dropOrderList.Count;
    //}

    //public Vector2Int GetDropOrder(int idx)
    //{
    //    return _dropOrderList[idx];
    //}

    //#endregion

    private void loadData()
    {
        for (int idx = 0; idx < _allJsonNames.Length; ++idx)
        {
            string fileName = _allJsonNames[idx];
            string jsonStr = loadFile(fileName);
            JArray jsonArr = JsonConvert.DeserializeObject(jsonStr) as JArray;
            Dictionary<int, JObject> buffer = new Dictionary<int, JObject>();
            for (int idx2 = 0; idx2 < jsonArr.Count; ++idx2)
            {
                JObject jItem = jsonArr[idx2] as JObject;
                try
                {
                    int id = jItem["ID"].ToObject<int>();
                    if (buffer.ContainsKey(id))
                    {
                        EDebug.LogWarning(string.Format("{0}.json中包含相同的ID:{1}", fileName, id));
                    }
                    else
                    {
                        buffer[id] = jItem;
                    }
                }
                catch (Exception)
                {
                    EDebug.LogError(string.Format("{0}.json缺少名为ID的字段", fileName));
                }
            }

            _allJsonData[fileName] = buffer;
        }
    }

    private string loadFile(string fileName)
    {
        return ResourceMgr.Instance.LoadData(fileName);
    }

    private JObject getSingleData(string fileName, int id, bool showLog = true, params string[] extraFiles)
    {
        Dictionary<int, JObject> buffer = getWholeData(fileName);
        bool find = (buffer != null && buffer.ContainsKey(id));
        if (!find)
        {
            for (int idx = 0; idx < extraFiles.Length; ++idx)
            {
                buffer = getWholeData(extraFiles[idx]);
                find = (buffer != null && buffer.ContainsKey(id));
                if (find)
                    break;
            }
        }
        if (!find)
        {
            if(showLog)
                EDebug.LogErrorFormat("Can't find {0} json, id:{1}", fileName, id);
            return null;
        }

        return buffer[id];
    }

    private Dictionary<int, JObject> getWholeData(string fileName)
    {
        if (!_allJsonData.ContainsKey(fileName))
        {
            EDebug.LogErrorFormat("Can't find {0} json", fileName);
            return null;
        }
        return _allJsonData[fileName];
    }

    /////// <summary>
    /////// 通过id获取物品道具
    /////// </summary>
    /////// <param name="itemId"></param>
    /////// <returns></returns>
    //public ItemData GetItemProp(int itemid)
    //{
    //    if (!_itemDic.ContainsKey(itemid))
    //    {
    //        EDebug.LogErrorFormat("Can't find itemid:{0} in JsonMgr.GetItemPropId", itemid);
    //        return null;
    //    }
    //    return _itemDic[itemid];
    //}

    ///// <summary>
    ///// 通过类型获取物品道具
    ///// </summary>
    ///// <param name="itemId"></param>
    ///// <param name="curType"></param>
    ///// <returns></returns>
    //public List<ItemData> GetItemPropType(int curType)
    //{
    //    if (_itemDic.Count != 0)
    //    {
    //        EDebug.LogErrorFormat("Can't find itemid:{0} in JsonMgr.GetItemPropType", curType);
    //        return null;
    //    }

    //    List<ItemData> data = new List<ItemData>();
    //    foreach (var item in _itemDic)
    //    {
    //        if (item.Value.ItemType == curType)
    //        {
    //            data.Add(item.Value);
    //        }
    //    }
    //    if (data.Count == 0)
    //    {
    //        EDebug.LogErrorFormat("JsonMgr.GetItemPropType failed, itemId:{0} curType:{1}", curType);
    //        return null;
    //    }
    //    return data;
    //}

    ///// <summary>
    ///// 通过id获取道具类型
    ///// </summary>
    ///// <param name="itemid"></param>
    ///// <returns></returns>
    //public int GetPropType(int itemid)
    //{
    //    if (!_itemDic.ContainsKey(itemid))
    //    {
    //        EDebug.LogErrorFormat("Can't find itemid:{0} in JsonMgr.GetItemPropId", itemid);
    //        return 0;
    //    }
    //    return _itemDic[itemid].ItemType;
    //}

    /// <summary>
    /// 通过ID获取谋一章节信息
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public BattleData GetChapter(int _id)
    {
        BattleData chapterData = null;
        for (int i = 0; i < Chapter.Count; i++)
        {
            if (chapterData != null) break;
            for (int idx = 0; idx < Chapter[i].Count; idx++)
            {

                if (Chapter[i][idx].ChapterId == _id)
                {
                    chapterData = Chapter[i][idx];
                    break;
                }
            }

        }
        if (chapterData == null)
        {
            EDebug.LogErrorFormat("Can't find _id:{0} in JsonMgr.GetChapter", _id);
            return null;
        }
        return chapterData;
    }

    /// <summary>
    /// 通过id获取某一关信息
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public LevelData GetLevel(int _id)
    {
        LevelData leve = null;
        for (int i = 0; i < Level.Count; i++)
        {
            if (leve != null) break;
            for (int idx = 0; idx < Level[i].Count; idx++)
            {
                for (int idx1 = 0; idx1 < Level[i][idx].Count; idx1++)
                {
                    if (Level[i][idx][idx1].id == _id)
                    {
                        leve = Level[i][idx][idx1];
                        break;
                    }
                }
            }
        }
        if (leve == null)
        {
            EDebug.LogErrorFormat("Can't find _id:{0} in JsonMgr.GetLevel", _id);
            return null;
        }
        return leve;
    }

    /// <summary>
    /// 返回LevelDataList
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_chapeer"></param>
    /// <returns></returns>
    public List<List<List<LevelData>>> GetLevelData()
    {
        List<List<List<LevelData>>> levelData = null;
        if (Level.Count == 0)
        {
            EDebug.LogErrorFormat("Can't find _id:{0} in JsonMgr.GetLevel");
            return null;
        }
        levelData = Level;
        return levelData;
    }
    ///// <summary>
    ///// 获取关卡怪物
    ///// </summary>
    ///// <param name="_id"></param>
    ///// <returns></returns>
    //public LevelMonster GetLevelMonstr (int _id)
    //{
    //    LevelMonster monster =null;

    //   for (int idx = 0; idx < LevelMonster.Count; idx++)
    //    {
    //        if (LevelMonster[idx].id == _id)
    //        {
    //            monster = LevelMonster[idx];
    //        }
    //    }
    //    if (monster == null)
    //    {
    //        EDebug.LogErrorFormat("Can't find _id:{0} in JsonMgr.LevelMonstr", _id);
    //        return null;
    //    }
    //    return monster;
    //}



#if !UNITY_EDITOR
    private Dictionary<string, string> allJsonString;
#endif
    private T LoadJson<T>(string jsonName)
    {
        T ret;
//#if UNITY_EDITOR
        string strContent = loadFile(jsonName);
//#else
        //if (allJsonString == null)
        //{
        //    allJsonString = new Dictionary<string, string>();
        //    //加载所有json文件
        //    string filePath = Application.persistentDataPath + "/" + PathUtil.CONFIG_PATH;
        //    if (!File.Exists(filePath))
        //        return default(T);
        //    byte[] bytes = File.ReadAllBytes(filePath);
        //    AssetBundle ab = AssetBundle.LoadFromMemory(bytes);
        //    var allJson = ab.LoadAllAssets<TextAsset>();
        //    for (int i = 0; i < allJson.Length; i++)
        //    {
        //        var asset = allJson[i];
        //        allJsonString.Add(asset.name, asset.text);
        //    }
        //    ab.Unload(true);
        //}
        //string strContent = allJsonString[jsonName];
//#endif
        ret = JsonUtility.FromJson<T>(strContent);
        return ret;
    }
    private void RunLoadJson()
    {
        //yield return new WaitForFixedUpdate();

        heroArray = LoadJson<HeroArray>("Hero");
        monsterArray = LoadJson<MonsterArray>("Monster");
        starArray = LoadJson<HeroStarArray>("HeroStar");
        oupArray = LoadJson<HeroRareArray>("OfficerUp");
        expArray = LoadJson<ExpArray>("Exp");
        officerArray = LoadJson<OfficerArray>("Officer");
        skillArray = LoadJson<SkillArray>("Skill");
        oProArray = LoadJson<OfficerProArray>("OfficerPro");
        equipArray = LoadJson<EquipArray>("Equip");
        heroShowArray = LoadJson<HeroShowArray>("HeroShow");
        tipArray = LoadJson<TipArray>("TipConfig");
        itemArray = LoadJson<ItemConfigArray>("Item");
        uiArray = LoadJson<UIConfigArray>("UI");
        totemArray = LoadJson<TotemArray>("Totem");
        treeArray = LoadJson<TotemTreeArray>("TotemTree");
        tipsArray = LoadJson<TipsArray>("Tips");
        strengthenArray = LoadJson<StrengthenSpendArray>("StrengthenSpend");
        advancedArray = LoadJson<AdvancedSpendArray>("AdvancedSpend");
        resArray = LoadJson<ResArray>("Res");
        spriteArray = LoadJson<ResArray>("Sprite");
        soundArray = LoadJson<ResArray>("Sound");
        intArray = LoadJson<GlobalValueArray>("GlobalValue");
        strArray = LoadJson<GlobalValueArray>("GlobalStr");
        monstersArray = LoadJson<MonstersArray>("Monsters");
        dropArray = LoadJson<DropOrderArray>("DropOrder");
        bulletArray = LoadJson<BulletArray>("Bullet");
        gmArray = LoadJson<GameManagerArray>("GM");
        //levelArray = LoadJson<CopyLevelArray>("Section");
#if !UNITY_EDITOR
        allJsonString = null;
#endif
    }

    /// <summary>
    /// 随机获取角色名
    /// </summary>
    /// <param name="_isMen"></param>
    /// <returns></returns>
    public string RandomName(int _isMen)
    {
        string Name = "";
        Name = Surname[UnityEngine.Random.Range(0, Surname.Count)];
        switch (_isMen)
        {
            case 1:
                Name += Man[UnityEngine.Random.Range(0, Man.Count)];
                break;
            case 2:
                Name += Woman[UnityEngine.Random.Range(0, Woman.Count)];
                break;
            default:
                Debug.Log("角色性别不存在"+":"+ _isMen);
                break;
        }
        return Name;
    }

    private List<string> ShieldWordList = new List<string>(); //屏蔽字库
    /// <summary>
    /// 判断是否包含屏蔽字
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public bool ExamineShieldWord(string _name)
    {
        if (ShieldWordList.Count == 0)
        {
            string ShieldWord = ResourceMgr.Instance.LoadData("ShieldWord");
            ShieldWordList = ShieldWord.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            ShieldWordList = ShieldWordList.Where(s => !string.IsNullOrEmpty(s)).ToList(); //检查是否有空字符串
        }
        bool isInclude = true;
        for (int idx = 0; idx < ShieldWordList.Count; idx++)
        {
            if (_name.IndexOf(ShieldWordList[idx]) != -1)
            {
                isInclude = false;
                return isInclude;
            }
        }
        return isInclude;
    }

    /// <summary>
    /// 通过ID获取月份道具信息
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public Dictionary<int, Item> GetSignInItemData(int _id)
    {
        Dictionary<int, JObject> _itemData = getWholeData("activity_sign");
        Dictionary<int, Item> signinItem = new Dictionary<int, Item>();
        int month = (_id - int.Parse(_id.ToString()[_id.ToString().Length - 2].ToString() + _id.ToString()[_id.ToString().Length - 1].ToString())) / 100;
        int day = 0;
        while (true)
        {
            day++;
            if (_itemData.ContainsKey(month * 100 + day))
            {
                signinItem.Add(month * 100 + day, _itemData[month * 100 + day].ToObject<Item>());
            }
            else
            {
                day = 0;
                break;
            }
        }
        return signinItem;

    }

    //public Dictionary<int, JObject> GetGameManagerArray()
    //{
    //    Dictionary<int, JObject> _itemData = getWholeData("GM");
    //    return _itemData;
    //}
         
    #region 英雄表
    private HeroArray heroArray;
    private Dictionary<int, Hero> _hero;

    //是不是全部显示
    public Hero[] GetHeroArray(bool includeAll)
    {
        if(includeAll)
            return heroArray.Array;
        else
        {
            List<Hero> heroList = new List<Hero>();
            for (int i = 0; i < heroArray.Array.Length; i++)
            {
                Hero hero = heroArray.Array[i];
                if(hero.stage ==1)
                    heroList.Add(hero);
            }
            return heroList.ToArray();
        }
    }


    public Hero GetHeroByID(int id)
    {
        Hero ret;
        if (_hero == null)
        {
            _hero = new Dictionary<int, Hero>();
            Hero[] array = GetHeroArray(true);
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_hero.ContainsKey(key))
                    _hero.Add(key, array[i]);
            }
        }
        _hero.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 怪物表
    private MonsterArray monsterArray;
    private Dictionary<int, Monster> _monster;

    public Monster[] GetMonsterArray()
    {
        return monsterArray.Array;
    }
    public Monster GetMonsterByID(int id)
    {
        Monster ret;
        if (_monster == null)
        {
            _monster = new Dictionary<int, Monster>();
            Monster[] array = GetMonsterArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_monster.ContainsKey(key))
                    _monster.Add(key, array[i]);
            }
        }
        _monster.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 英雄星级表
    private HeroStarArray starArray;
    private Dictionary<int, HeroStar> _heroStar;

    public HeroStar[] GetHeroStarArray()
    {
        return starArray.Array;
    }
    public HeroStar GetHeroStarByID(int id)
    {
        HeroStar ret;
        if (_heroStar == null)
        {
            _heroStar = new Dictionary<int, HeroStar>();
            HeroStar[] array = GetHeroStarArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_heroStar.ContainsKey(key))
                    _heroStar.Add(key, array[i]);
            }
        }
        _heroStar.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 官阶边框头像框
    private HeroRareArray oupArray;
    private Dictionary<int, HeroRare> _offiverup;

    public HeroRare[] GetHeroRareArray()
    {
        return oupArray.Array;
    }
    public HeroRare GetHeroRareByID(int id)
    {
        HeroRare ret;
        if (_offiverup == null)
        {
            _offiverup = new Dictionary<int, HeroRare>();
            HeroRare[] array = GetHeroRareArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_offiverup.ContainsKey(key))
                    _offiverup.Add(key, array[i]);
            }
        }
        _offiverup.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 经验表
    private ExpArray expArray;
    private Dictionary<int, Exp> _exp;

    public Exp[] GetExpArray()
    {
        return expArray.Array;
    }
    public Exp GetExpByID(int id)
    {
        Exp ret;
        if (_exp == null)
        {
            _exp = new Dictionary<int, Exp>();
            Exp[] array = GetExpArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_exp.ContainsKey(key))
                    _exp.Add(key, array[i]);
            }
        }
        _exp.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 官职表
    private OfficerArray officerArray;
    private Dictionary<int, Officer> _officer;

    public Officer[] GetOfficerArray()
    {
        return officerArray.Array;
    }
    public Officer GetOfficerByID(int id)
    {
        Officer ret;
        if (_officer == null)
        {
            _officer = new Dictionary<int, Officer>();
            Officer[] array = GetOfficerArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_officer.ContainsKey(key))
                    _officer.Add(key, array[i]);
            }
        }
        _officer.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 技能表
    private SkillArray skillArray;
    private Dictionary<int, Skill> _skill;

    public Skill[] GetSkillArray()
    {
        return skillArray.Array;
    }
    public Skill GetSkillByID(int id)
    {
        Skill ret;
        if (_skill == null)
        {
            _skill = new Dictionary<int, Skill>();
            Skill[] array = GetSkillArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_skill.ContainsKey(key))
                    _skill.Add(key, array[i]);
            }
        }
        _skill.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 官职属性
    private OfficerProArray oProArray;
    private Dictionary<int, OfficerPro> _oPro;

    public OfficerPro[] GetOfficerProArray()
    {
        return oProArray.Array;
    }
    public OfficerPro GetOfficerProByID(int id)
    {
        OfficerPro ret;
        if (_oPro == null)
        {
            _oPro = new Dictionary<int, OfficerPro>();
            OfficerPro[] array = GetOfficerProArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_oPro.ContainsKey(key))
                    _oPro.Add(key, array[i]);
            }
        }
        _oPro.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 装备表
    private EquipArray equipArray;
    private Dictionary<int, Equip> _equip;

    public Equip[] GetEquipArray()
    {
        return equipArray.Array;
    }
    public Equip GetEquipByID(int id)
    {
        Equip ret;
        if (_equip == null)
        {
            _equip = new Dictionary<int, Equip>();
            Equip[] array = GetEquipArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_equip.ContainsKey(key))
                    _equip.Add(key, array[i]);
            }
        }
        _equip.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 武将展示表
    private HeroShowArray heroShowArray;
    private Dictionary<int, HeroShow> _show;

    public HeroShow[] GetHeroShowArray()
    {
        return heroShowArray.Array;
    }
    public HeroShow GetHeroShowByID(int id)
    {
        HeroShow ret;
        if (_show == null)
        {
            _show = new Dictionary<int, HeroShow>();
            HeroShow[] array = GetHeroShowArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_show.ContainsKey(key))
                    _show.Add(key, array[i]);
            }
        }
        _show.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 提示框
    private TipArray tipArray;
    private Dictionary<int, Tip> _tip;

    public Tip[] GetTipArray()
    {
        return tipArray.Array;
    }
    public Tip GetTipByID(int id)
    {
        Tip ret;
        if (_tip == null)
        {
            _tip = new Dictionary<int, Tip>();
            Tip[] array = GetTipArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_tip.ContainsKey(key))
                    _tip.Add(key, array[i]);
            }
        }
        _tip.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region   道具表
    private ItemConfigArray itemArray;
    private Dictionary<int, ItemConfig> _item;

    public ItemConfig[] GetItemConfigArray()
    {
        return itemArray.Array;
    }
    public ItemConfig GetItemConfigByID(int id)
    {
        ItemConfig ret;
        if (_item == null)
        {
            _item = new Dictionary<int, ItemConfig>();
            ItemConfig[] array = GetItemConfigArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_item.ContainsKey(key))
                    _item.Add(key, array[i]);
            }
        }
        _item.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region   UI表
    private UIConfigArray uiArray;
    private Dictionary<int, UIConfig> _ui;

    public UIConfig[] GetUIConfigArray()
    {
        return uiArray.Array;
    }
    public UIConfig GetUIConfigByID(int id)
    {
        UIConfig ret;
        if (_ui == null)
        {
            _ui = new Dictionary<int, UIConfig>();
            UIConfig[] array = GetUIConfigArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_ui.ContainsKey(key))
                    _ui.Add(key, array[i]);
            }
        }
        _ui.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region   护身符表
    private TotemArray totemArray;
    private Dictionary<int, Totem> _totem;

    public Totem[] GetTotemArray()
    {
        return totemArray.Array;
    }
    public Totem GetTotemByID(int id)
    {
        Totem ret;
        if (_totem == null)
        {
            _totem = new Dictionary<int, Totem>();
            Totem[] array = GetTotemArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_totem.ContainsKey(key))
                    _totem.Add(key, array[i]);
            }
        }
        _totem.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 护身符组
    private TotemTreeArray treeArray;
    private Dictionary<int, TotemTree> _totemTree;
    public TotemTree[] GetTotemTreeArray()
    {
        return treeArray.Array;
    }
    public TotemTree GetTotemTreeByID(int id)
    {
        TotemTree ret;
        if (_totemTree == null)
        {
            _totemTree = new Dictionary<int, TotemTree>();
            TotemTree[] array = GetTotemTreeArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_totemTree.ContainsKey(key))
                    _totemTree.Add(key, array[i]);
            }
        }
        _totemTree.TryGetValue(id, out ret);
        return ret;
    }

    #endregion
    #region 强化花费
    private StrengthenSpendArray strengthenArray;
    private Dictionary<int, StrengthenSpend> _strengthen;
    public StrengthenSpend[] GetStrengthenSpendArray()
    {
        return strengthenArray.Array;
    }
    public StrengthenSpend GetStrengthenSpendByID(int id)
    {
        StrengthenSpend ret;
        if (_strengthen == null)
        {
            _strengthen = new Dictionary<int, StrengthenSpend>();
            StrengthenSpend[] array = GetStrengthenSpendArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_strengthen.ContainsKey(key))
                    _strengthen.Add(key, array[i]);
            }
        }
        _strengthen.TryGetValue(id, out ret);
        return ret;
    }

    #region Tips
    private TipsArray tipsArray;
    private Dictionary<int, Tips> _tips;
    public Tips[] GetTipsArray()
    {
        return tipsArray.Array;
    }
    public Tips GetTipsByID(int id)
    {
        Tips ret;
        if (_tips == null)
        {
            _tips = new Dictionary<int, Tips>();
            Tips[] array = GetTipsArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_tips.ContainsKey(key))
                    _tips.Add(key, array[i]);
            }
        }
        _tips.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #endregion
    #region 进阶花费
    private AdvancedSpendArray advancedArray;
    private Dictionary<int, AdvancedSpend> _advanced;
    public AdvancedSpend[] GetAdvancedSpendArray()
    {
        return advancedArray.Array;
    }
    public AdvancedSpend GetAdvancedSpendByID(int id)
    {
        AdvancedSpend ret;
        if (_advanced == null)
        {
            _advanced = new Dictionary<int, AdvancedSpend>();
            AdvancedSpend[] array = GetAdvancedSpendArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_advanced.ContainsKey(key))
                    _advanced.Add(key, array[i]);
            }
        }
        _advanced.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 资源表
    private ResArray resArray;
    private Dictionary<int, Res> _res;
    public Res[] GetResArray()
    {
        return resArray.Array;
    }
    public Res GetResArrayByID(int id)
    {
        Res ret;
        if (_res == null)
        {
            _res = new Dictionary<int, Res>();
            Res[] array = GetResArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_res.ContainsKey(key))
                    _res.Add(key, array[i]);
            }
        }
        _res.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 精灵表
    private ResArray spriteArray;
    private Dictionary<int, Res> _sprite;
    public Res[] GetSpriteArray()
    {
        return spriteArray.Array;
    }
    public Res GetSpriteArrayByID(int id)
    {
        Res ret;
        if (_sprite == null)
        {
            _sprite = new Dictionary<int, Res>();
            Res[] array = GetSpriteArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_sprite.ContainsKey(key))
                    _sprite.Add(key, array[i]);
            }
        }
        _sprite.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 音效表
    private ResArray soundArray;
    private Dictionary<int, Res> _sound;
    public Res[] GetSoundArray()
    {
        return soundArray.Array;
    }
    public Res GetSoundArrayByID(int id)
    {
        Res ret;
        if (_sound == null)
        {
            _sound = new Dictionary<int, Res>();
            Res[] array = GetSoundArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_sound.ContainsKey(key))
                    _sound.Add(key, array[i]);
            }
        }
        _sound.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 全局参数
    private GlobalValueArray intArray;
    private Dictionary<int, GlobalValue> _int;
    public GlobalValue[] GetGlobalIntArray()
    {
        return intArray.Array;
    }
    public GlobalValue GetGlobalIntArrayByID(int id)
    {
        GlobalValue ret;
        if (_int == null)
        {
            _int = new Dictionary<int, GlobalValue>();
            GlobalValue[] array = GetGlobalIntArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_int.ContainsKey(key))
                    _int.Add(key, array[i]);
            }
        }
        _int.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 全局字符
    private GlobalValueArray strArray;
    private Dictionary<int, GlobalValue> _string;
    public GlobalValue[] GetGlobalStringArray()
    {
        return strArray.Array;
    }
    public GlobalValue GetGlobalStringArrayByID(int id)
    {
        GlobalValue ret;
        if (_string == null)
        {
            _string = new Dictionary<int, GlobalValue>();
            GlobalValue[] array = GetGlobalStringArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_string.ContainsKey(key))
                    _string.Add(key, array[i]);
            }
        }
        _string.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 怪物组
    private MonstersArray monstersArray;
    private Dictionary<int, Monsters> _monsters;
    public Monsters[] GetMonstersArray()
    {
        return monstersArray.Array;
    }
    public Monsters GetMonstersByID(int id)
    {
        Monsters ret;
        if (_monsters == null)
        {
            _monsters = new Dictionary<int, Monsters>();
            Monsters[] array = GetMonstersArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_monsters.ContainsKey(key))
                    _monsters.Add(key, array[i]);
            }
        }
        _monsters.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 掉落排序
    private DropOrderArray dropArray;
    private Dictionary<int, DropOrder> _dropOrder;
    public DropOrder[] GetDropOrderArray()
    {
        return dropArray.Array;
    }
    public DropOrder GetDropOrderByID(int id)
    {
        DropOrder ret;
        if (_dropOrder == null)
        {
            _dropOrder = new Dictionary<int, DropOrder>();
            DropOrder[] array = GetDropOrderArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_dropOrder.ContainsKey(key))
                    _dropOrder.Add(key, array[i]);
            }
        }
        _dropOrder.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 子弹表
    private BulletArray bulletArray;
    private Dictionary<int, Bullet> _bullet;
    public Bullet[] GetBulletArray()
    {
        return bulletArray.Array;
    }
    public Bullet GetBulletByID(int id)
    {
        Bullet ret;
        if (_bullet == null)
        {
            _bullet = new Dictionary<int, Bullet>();
            Bullet[] array = GetBulletArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_bullet.ContainsKey(key))
                    _bullet.Add(key, array[i]);
            }
        }
        _bullet.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region GameManager
    private GameManagerArray gmArray;
    private Dictionary<int, GameManager> _gm;
    public GameManager[] GetGameManagerArray()
    {
        return gmArray.Array;
    }
    public GameManager GetGameManagerByID(int id)
    {
        GameManager ret;
        if (_gm == null)
        {
            _gm = new Dictionary<int, GameManager>();
            GameManager[] array = GetGameManagerArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_gm.ContainsKey(key))
                    _gm.Add(key, array[i]);
            }
        }
        _gm.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
    #region 关卡表
    private CopyLevelArray levelArray;
    private Dictionary<int, CopyLevel> _copyLevel;
    public CopyLevel[] GetCopyLevelArray()
    {
        return levelArray.Array;
    }
    public CopyLevel GetCopyLevelByID(int id)
    {
        CopyLevel ret;
        if (_copyLevel == null)
        {
            _copyLevel = new Dictionary<int, CopyLevel>();
            CopyLevel[] array = GetCopyLevelArray();
            int key = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                key = array[i].ID;
                if (!_copyLevel.ContainsKey(key))
                    _copyLevel.Add(key, array[i]);
            }
        }
        _copyLevel.TryGetValue(id, out ret);
        return ret;
    }
    #endregion
}
