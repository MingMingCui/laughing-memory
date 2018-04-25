using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDetailView : PlayerInfoDetailViewBase {

    public Sprite StubEmpty = null;

    Dictionary<int, Image> _stubui = new Dictionary<int, Image>();
    public override void Awake()
    {
        base.Awake();
        for (int stubx = 1; stubx <= StubView.StubRange; ++stubx)
        {
            for (int stuby = 1; stuby <= StubView.StubRange; ++stuby)
            {
                int stubpos = stubx * 10 + stuby;
                Image img = this.grids_obj.transform.Find(string.Format("stubpos_{0}", stubpos)).GetComponent<Image>();
                _stubui.Add(stubpos, img);
            }
        }
    }

    public void SetInfo(int headid, string playername, int herocnt, string corpsName, Dictionary<int, int> stubData,
        bool isPvp = false, int arenaRank = 0, int arenaWin = 0)
    {
        //TODO:headid
        this.name_txt.text = playername;
        this.herocnt_txt.text = herocnt.ToString();
        this.corpsname_txt.text = corpsName;
        foreach (var p in _stubui)
            p.Value.sprite = StubEmpty;
        foreach (var p in stubData)
        {
            if (_stubui.ContainsKey(p.Key))
            {
                JsonData.Hero heroData = JsonMgr.GetSingleton().GetHeroByID(p.Value);
                _stubui[p.Key].sprite = ResourceMgr.Instance.LoadSprite(heroData.headid);
            }
            else
            {
                EDebug.LogErrorFormat("PlayerInfoDetailView.SetInfo failed, _stubui doesn't contains stubpos {0}", p.Key);
            }
        }

        this.arena_obj.SetActive(isPvp);

        if (isPvp)
        {
            this.arenarank_txt.text = arenaRank.ToString();
            this.arenawin_txt.text = arenaWin.ToString();
        }
    }
}
