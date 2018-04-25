using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ComposeDivinationView : ComposeDivinationViewBase
{
    public GameObject item;
    private List<DivinationItemView> playItem;
    private List<DivinationItemView> allItem;
    private Image[] playImage;
    private Text[] playText;
    private int playExp;
    private ScaleTween st;

    private TotemData composeItem;
    private int plusLevel;

    private ParticleItem particle;
    private ParticleItem[] childparticle;

    public override void Awake()
    {
        base.Awake();
        playItem = new List<DivinationItemView>();
        allItem = new List<DivinationItemView>();
        playImage = hexagon_trf.GetComponentsInChildren<Image>();
        for (int i = 1; i < playImage.Length; i++)
        {
            Image image = playImage[i];
            int index = i;
            EventListener.Get(image.gameObject).OnClick = e =>
            {
                if (index <= playItem.Count)
                {
                    playItem[index - 1].SetAlpha(0);
                    for (int j = 1; j < playItem[index - 1].data.Level; j++)
                    {
                        playExp -= playItem[index - 1].data.LevelUpExp(j);
                    }
                    playExp -= playItem[index - 1].data.Exp;
                    playExp -= playItem[index - 1].data.TotemConfig.Exp;
                    playItem.RemoveAt(index - 1);
                    SetPlayView();
                    ExpValue();
                }
            };  
        }
        playText = hexagon_trf.GetComponentsInChildren<Text>();
        particle = particlemain_trf.GetComponentInChildren<ParticleItem>();
        particle.Stop();
        childparticle = particle_trf.GetComponentsInChildren<ParticleItem>();
        for (int i = 0; i < childparticle.Length; i++)
        {
            childparticle[i].Stop();
        }
    }

    public void Open(TotemData td)
    {
        if (td == null)
            return;
        playExp = 0;
        plusLevel = 0;
        composeItem = td;
        playItem_img.sprite = ResourceMgr.Instance.LoadSprite(td.ItemData.icon);
        ExpValue();
        name_txt.text = td.ItemData.name;
        SetAttrActive(false);

        st = tip_obj.GetComponent<ScaleTween>();
        st.AddOnFinished(delegate () { tip_obj.SetActive(false); });
        EventListener.Get(tip_btn.gameObject).OnClick = e =>
        {
            tip_obj.SetActive(true);
            st.ResetToBeginning();
            st.PlayForward();
        };
        EventListener.Get(close_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Close(UIID.CompostDivination);
        };
        SetPlayView();
        InitItem();
    }  
    private void SetAttrView()
    {
        int upexp = composeItem.LevelUpExp() - composeItem.Exp;
        plusLevel = composeItem.Level;
        float temp = playExp;
        while (temp >= upexp)
        {
            plusLevel++;
            temp -= upexp;
            upexp = composeItem.LevelUpExp(plusLevel);
        }

        Pro[] proa = composeItem.Attribute;
        StringBuilder sb = new StringBuilder();
        StringBuilder sbnext = new StringBuilder();
        sb.Append(composeItem.Level + "级");
        sbnext.Append(plusLevel + "级");

        for (int i = 0; i < proa.Length; ++i)
        {
            sb.Append("\n");
            sbnext.Append("\n");
            sb.Append(AttrUtil.GetAttribute(proa[i].attr));
            sbnext.Append(AttrUtil.GetAttribute(proa[i].attr));
            sb.Append("  +");
            sbnext.Append("  +");
            if (proa[i].num != 0)
            {
                sb.Append(AttrUtil.ShowText(proa[i].attr,proa[i].num ));
                sbnext.Append(AttrUtil.ShowText(proa[i].attr,proa[i].num));
            }
            else if (proa[i].per != 0)
            {
                sb.Append(AttrUtil.ShowText(proa[i].attr,0,proa[i].per));
                sbnext.Append(AttrUtil.ShowText(proa[i].attr,0,proa[i].per));
            }
            sb.Append("\n");
            sbnext.Append("\n");
        }

        attr_txt.text = sb.ToString();
        nextattr_txt.text = sbnext.ToString();
        SetAttrActive(playExp != 0);
    }
    private void SetAttrActive(bool active)
    {
        int count = attr_txt.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            attr_txt.transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    private void InitItem()
    {
        TotemData[] totems = TotemMgr.GetSingleton().Totems;
        for (int i = 0; i < totems.Length; ++i)
        {
            TotemData totem = totems[i];
            if (totem.md5 == composeItem.md5 || totem.HeroID != 0)
                continue;
            GameObject t = Instantiate(item, divination_sr.content);
            DivinationItemView div = t.GetComponent<DivinationItemView>();
            div.SetView(totem);
            allItem.Add(div);
            EventListener.Get(t).OnClick = e =>
            {
                if (!playItem.Contains(div))
                {
                    if (playItem.Count >= 6)
                    {
                        CanvasView.Instance.AddNotice("已到最大数量");
                        return;
                    }
                    playItem.Add(div);
                    div.SetAlpha(1);
                    for (int j = 1; j < div.data.Level; j++)
                    {
                        playExp += div.data.LevelUpExp(j);
                    }
                    playExp += totem.Exp;
                    playExp += totem.TotemConfig.Exp;
                }
                else
                {
                    playItem.Remove(div);
                    div.SetAlpha(0);
                    for (int j = 1; j < div.data.Level; j++)
                    {
                        playExp -= div.data.LevelUpExp(j);
                    }
                    playExp -= totem.Exp;
                    playExp -= totem.TotemConfig.Exp;
                }
                SetPlayView();
                ExpValue();
            };
            EventListener.Get(t).BegineDragEvent = e =>
            {
                divination_sr.OnBeginDrag(e);
            };
            EventListener.Get(t).DragEvent = e =>
            {
                divination_sr.OnDrag(e);
            };
            EventListener.Get(t).EndDragEvent = e =>
            {
                divination_sr.OnEndDrag(e);
            };
        }

        EventListener.Get(auto_btn.gameObject).OnClick = e =>
        {
            int count = playItem.Count;
            int index = allItem.Count;
            index--;
            while (playItem.Count < 6 && index >= 0)
            {
                if (!playItem.Contains(allItem[index]))
                {
                    playItem.Add(allItem[index]);
                    allItem[index].SetAlpha(1);
                }
                index--;
            }
            playExp = 0;
            for (int i = 0; i < playItem.Count; i++)
            {
                for (int j = 1; j < playItem[i].data.Level; j++)
                {
                    playExp += playItem[i].data.LevelUpExp(j); ;
                }
                playExp += playItem[i].data.Exp;
                playExp += playItem[i].data.TotemConfig.Exp;
            }
            SetPlayView();
            ExpValue();
        };

        Vector2 size = divination_sr.content.sizeDelta;
        size.y = Mathf.CeilToInt(totems.Length / 3f) * 182 + 24;
        divination_sr.content.sizeDelta = size;
        EventListener.Get(sure_btn.gameObject).OnClick = e =>
        {
            if (composeItem == null)
                return;
            Compose();
            SetPlayView();
            ExpValue();
            Close();
            Open(composeItem);
        };
    }

    private void Compose()
    {
        particle.Play();
        for (int i = 0; i < playItem.Count; ++i)
        {
            TotemMgr.GetSingleton().RemoveTotem(playItem[i].data);
            childparticle[i].Play();
        }
        playItem.Clear();
        int upexp = composeItem.LevelUpExp() - composeItem.Exp;
        while (playExp >= upexp)
        {
            composeItem.Level++;
            composeItem.Exp = 0;
            playExp -= upexp;
            upexp = composeItem.LevelUpExp() - composeItem.Exp;
        }
        composeItem.Exp += Mathf.FloorToInt(playExp);
        playExp = 0;
        ZEventSystem.Dispatch(EventConst.TOTEMDATACHANGE);
    }
     /// <summary>
     /// 左侧被选中
     /// </summary>
    private void SetPlayView()
    {
        if (composeItem == null)
        {
            Color c = playImage[0].color;
            c.a = 0;
            playImage[0].color = c;
            playText[0].text = "";
        }
        else
        {
            Color c = playImage[0].color;
            c.a = 1;
            playImage[0].color = c;
            int rare = composeItem.ItemData.rare;
            string color = ColorMgr.Colors[rare - 1];
            playText[0].text = string.Format("<color=#{0}>Lv.{1}\n {2}</color>", color, composeItem.Level, composeItem.TotemConfig.Name);
        }          

        int i = 0;
        for (; i < playItem.Count; ++i)
        {
            Color c = playImage[i + 1].color;
            c.a = 1;
            playImage[i + 1].color = c;
            playImage[i + 1].sprite = playItem[i].item_img.sprite;
            playText[i + 1].text = playItem[i].name_txt.text;
        }
        for (i = i + 1; i < playImage.Length ; ++i)
        {
            Color c = playImage[i].color;
            c.a = 0;
            playImage[i].color = c;
            playText[i].text = "";
        }
    }

    public void Close()
    {
        for (int i = 0,length = divination_sr.content.childCount; i < length; ++i)
        {
            DestroyImmediate(divination_sr.content.GetChild(0).gameObject);
        }

        playItem.Clear();
        allItem.Clear();
    }
     /// <summary>
     /// 经验条
     /// </summary>
    private void ExpValue()
    {
        if (composeItem == null)
            return;
        SetAttrView();

        float upExp = composeItem.LevelUpExp();
        engulfexp_img.fillAmount= (composeItem.Exp + playExp) / upExp;
        expslider_img.fillAmount = composeItem.Exp / upExp;
        exp_txt.text = composeItem.Exp + playExp + "/" + upExp;
    }
}
