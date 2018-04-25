using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenActivityView : SevenActivityViewBase
{


    public ItemUIView itemUIView;
    [HideInInspector]
    public List<ItemUIView> ListAward = new List<ItemUIView>();
    [HideInInspector]
    public List<int> GetNum = new List<int>();
    [HideInInspector]
    public int AwardID;
    private Transform ct = null;
    private readonly int ID = 11270;
    public void Init(int _num)
    {
        for (int idx = 0; idx < _num; idx++)
        {
            if(_num > ListAward.Count)
            {
                ItemUIView itemUiView = InitItemInfo().GetComponent<ItemUIView>();
                itemUiView.Init();
                ListAward.Add(itemUiView);
            }
            else
                return;
        }
    }

    public GameObject InitItemInfo()
    {
        GameObject _item = GameObject.Instantiate(itemUIView.gameObject);
        _item.transform.SetParent(GoodsBar_obj.transform);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }

    public void SkyNum(int _SkyNum)
    {

        AwardID = _SkyNum;

        if (ct == null)
        {
            ct = GameObject.Find("HeroParent").transform;
        }
            if (ct.childCount > 0)
            {
                for (int i = 0, length = ct.childCount; i < length; ++i)
                {
                    Destroy(ct.GetChild(0).gameObject);
                }
            }
            GameObject heroGo = ResourceMgr.Instance.LoadResource(ID) as GameObject;
            if (heroGo == null)
                return;
            heroGo = Instantiate(heroGo, ct, false);
            heroGo.transform.localPosition = new Vector3(0,-1,0);
            heroGo.transform.localScale = new Vector3(2f,2f,2f);
            heroGo.SetLayer("Hero");
            
       
      JObject award = JsonMgr.GetSingleton().GetAward(_SkyNum);
      int[] awardId = Util.Str2IntArr(award["AwardID"].ToString());
      int[] num = Util.Str2IntArr(award["AwardNum"].ToString());
      if (ListAward.Count < awardId.Length)
            Init(awardId.Length);
        else
        {
            for (int idx1 = 0; idx1 < ListAward.Count; idx1++)
            {
                if (idx1 >= awardId.Length)
                    ListAward[idx1].gameObject.SetActive(false);
                else if(false == ListAward[idx1].gameObject.activeSelf)
                    ListAward[idx1].gameObject.SetActive(true);
            }
        }

      for (int idx = 0; idx < awardId.Length; idx++)
       {
        ListAward[idx].SetInfo(awardId[idx], num[idx]); 
        }
        if (awardId.Length >= 4)
        {
            GoodsBar_obj.GetComponent<RectTransform>().offsetMax = new Vector2((awardId.Length - 4) * 170, 0);
            GoodsBar_obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        }
        if (GetNum.Contains(AwardID))
        {
            Get_obj.SetActive(true);
            Affirm_btn.interactable = false;
            Affirm_btn.enabled = false;
            return;
        }
        else
        {
            Get_obj.SetActive(false);
            Affirm_btn.interactable = true;
            Affirm_btn.enabled = true;
        }
    }
    public void GetAward()
    {

        if (GetNum.Contains(AwardID))
            return;
        else
        {
            Get_obj.SetActive(true);
            Affirm_btn.interactable = false;
            Affirm_btn.enabled = false;
            GetNum.Add(AwardID);
        }
            
        Debug.Log("给服务器发信息");
    }
    
}
