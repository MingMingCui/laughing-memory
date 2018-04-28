using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public enum MailTyper
{
     zero,
     one,
     two,
     three
}

public class PostBoxView : PostBoxViewBase
{
    public GameObject Mail;
    public GameObject MailItem;
    public GameObject Three;
    [HideInInspector]
    public List<MailView> MailList = new List<MailView>();  //邮件
    [HideInInspector]
    public List<MailItemview> MailItemList = new List<MailItemview>(); //附件物品
    [HideInInspector]
    public List<ThreeView> ThreeList = new List<ThreeView>();  //附件钱币
    private readonly int Ico_id = 20111;
    private readonly int ThreeTall = 65;
    private MailTyper MailType = MailTyper.one;
    RectTransform Addres;  //寄件人姓名
    RectTransform MailWard;  // 内容
    RectTransform threetallmrec;  //邮件附件钱币父类
    RectTransform mailitemrec;    //邮件附件物品父类
    RectTransform Adjunct;       //邮件附件区域
    RectTransform Content = null;        //拖动区域
  //  RectTransform NoticeMatter;  //无附件邮件内容
  //  RectTransform NoticeAddresser;  //无附件邮件发件人
  //  RectTransform Notice;            //无附件邮件拖动区域
  //  RectTransform NoticeName;       //邮件内容显示区
    float height = 0f;
    GridLayoutGroup glg = null;

    public void InitMail(int threenum, Text content)
    {
        if (Addres == null)
        {
            Addres = Addresser_txt.GetComponent<RectTransform>();
            MailWard = MailWardContent_txt.GetComponent<RectTransform>();
            threetallmrec = Currency_trf.GetComponent<RectTransform>();
            mailitemrec = MailItem_trf.GetComponent<RectTransform>();
            Adjunct = Adjunct_obj.GetComponent<RectTransform>();
            Content = Content_obj.GetComponent<RectTransform>();
            //NoticeMatter = NoticeMatter_txt.GetComponent<RectTransform>();
            //NoticeAddresser = NoticeAddresser_txt.GetComponent<RectTransform>();
            //Notice = Notice_obj.GetComponent<RectTransform>();
            //NoticeName = NoticeName_obj.GetComponent<RectTransform>();
        }
        string ids = content.text;
        content.text = string.Empty;
        height = content.preferredHeight; //一行文字的高度
        content.text = ids;
        float line = content.preferredHeight / height;
        
            MailWard.offsetMin = new Vector2(0, -line * height);
            Addres.anchoredPosition = new Vector2(0, MailWard.offsetMin.y*1.1f);
            Adjunct.anchoredPosition = new Vector2(0, Addres.anchoredPosition.y + -height + -Addres.rect.size.y);
            threetallmrec.anchoredPosition = new Vector2(0, -height);
            mailitemrec.anchoredPosition = new Vector2(0, -(threenum * ThreeTall) + -height);
            Content.offsetMin = new Vector2(0, Addres.anchoredPosition.y);
            Content.offsetMax = Vector2.zero;
    }

    /// <summary>
    /// 初始化邮件
    /// </summary>
    /// <param name="_mailData"></param>
    public void Init(List<MailDatas> _mailData)
    {
        //if (_mailData.Count > 3)
        //{
        //    show_trf.GetComponent<RectTransform>().offsetMin = new Vector2(0, -(_mailData.Count - 3) * 150);
        //    show_trf.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        //}
        //else
        //{
        //    show_trf.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        //    show_trf.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        //}
      
        if (_mailData.Count > 0)
        {
            for (int idx = 0; idx < _mailData.Count; idx++)
            {
                if (_mailData.Count > MailList.Count)
                {
                    MailView mailView = InitItemInfo(Mail, show_trf).GetComponent<MailView>();
                    mailView.Init();
                    MailList.Add(mailView);
                    // Found(_mailData[idx]);
                }
                //else
                //{
                //    for (int idx1 = 0; idx1 < MailList.Count; idx1++)
                //    {

                //        if (idx1 >= _mailData.Count)
                //        {
                //            MailList[idx1].gameObject.SetActive(false);
                //            MailList[idx1].Close();
                //        }
                //        else
                //        {
                //            MailList[idx1].gameObject.SetActive(true);
                //        }
                //    }
                //    return;
                //}
            }
            totala_tog.isOn = true;
             GetMail(MailTyper.zero);
        }
    }

    public void Found(List<MailDatas> _mail)
    {

    //for (int idx = 0; idx < MailList.Count; idx++)
    // {
    //    if (MailList[idx].MailId != 0)
    //      {
    //         MailList[idx].Endow(_mail[idx]);
    //         bool idure = false;
    //         if (_mail[idx].isRead == 0)
    //             idure = false;
    //         else
    //             idure = true;
    //          MailList[idx].RedDot.SetActive(!idure);
    //          MailList[idx].Shade.SetActive(idure);

    //          MailList[idx].gameObject.SetActive(true);
    //          MailList[idx].isUse = 0;
    //          ZEventSystem.Dispatch(EventConst.Incident, true);
    //        }
    //    }

        //MailView mailView = InitItemInfo(Mail, show_trf).GetComponent<MailView>();
        //mailView.Init();
        //mailView.Endow(_mail);
        //MailList.Add(mailView);
      //  ZEventSystem.Dispatch(EventConst.Incident, true);
    }



    public void Found(int _mailid, List<Item> _item)
    {
        int num = 0;
        for (int idx = 0; idx < _item.Count; idx++)
        {
            if (_item[idx].itemId < 1003)
            {
                if (ThreeList.Count + MailItemList.Count >= _item.Count)
                {
                    for (int idx1 = 0; idx1 < ThreeList.Count; idx1++)
                    {
                        if (false == ThreeList[idx1].idUse)
                        {
                            ThreeList[idx1].gameObject.SetActive(true);
                            ThreeList[idx1].Endow(_mailid, _item[idx]);
                            num++;
                            break;
                        }
                    }
                }
                else
                {
                    ThreeView mailView = InitItemInfo(Three, Currency_trf).GetComponent<ThreeView>();
                    mailView.Init();
                    mailView.Endow(_mailid, _item[idx]);
                    ThreeList.Add(mailView);
                    num++;
                }

            }
            else
            {
                if (MailItemList.Count + ThreeList.Count < _item.Count)
                {
                    MailItemview mailView = InitItemInfo(MailItem, MailItem_trf).GetComponent<MailItemview>();
                    mailView.Init();
                    mailView.Endow(_mailid, _item[idx].itemId, _item[idx].itemNum);
                    MailItemList.Add(mailView);
                }
                else
                {
                    for (int idx1 = 0; idx1 < MailItemList.Count; idx1++)
                    {
                        if (false == MailItemList[idx1].gameObject.activeSelf)
                            MailItemList[idx1].gameObject.SetActive(true);
                        MailItemList[idx1].Endow(_mailid, _item[idx1].itemId, _item[idx1].itemNum);

                        if (_item.Count - idx1 <= ThreeList.Count)
                        {
                            MailItemList[idx1].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        InitMail(num, MailWardContent_txt);
    }

    public GameObject InitItemInfo(Object _Mailitem, Transform _Parent)
    {
        GameObject _item = GameObject.Instantiate((GameObject)_Mailitem);
        _item.transform.SetParent(_Parent);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }

    public void OpenMail(MailView _mail)
    {
        if (MailList.Count == 0) return;
        Close(true,null);
        for (int idx = 0; idx < PostBoxMgr.Instance.MailData.Count; idx++)
        {
            if (PostBoxMgr.Instance.MailData[idx].mail_id == _mail.MailId)
            {
                MailWardName_txt.text = PostBoxMgr.Instance.MailData[idx].mailname;
                MailWardContent_txt.text = PostBoxMgr.Instance.MailData[idx].content;
                Addresser_txt.text = PostBoxMgr.Instance.MailData[idx].addresser;
                if (PostBoxMgr.Instance.MailData[idx].mailitem.Count > 0)
                {
                    Found(PostBoxMgr.Instance.MailData[idx].mail_id, PostBoxMgr.Instance.MailData[idx].mailitem);
                    Adjunct_obj.SetActive(true);
                    Get_btn.gameObject.SetActive(true);
                    Draw_btn.gameObject.SetActive(false);
                    deletea_btn.gameObject.SetActive(false);
                }
                else
                {
                    deletea_btn.gameObject.SetActive(true);
                    Draw_btn.gameObject.SetActive(true);
                    Get_btn.gameObject.SetActive(false);
                    _mail.Shade.SetActive(true);
                    _mail.RedDot.SetActive(false);
                    Adjunct_obj.SetActive(false);
                    InitMail(0, MailWardContent_txt);
                    PostBoxMgr.Instance.MailData[idx].isRead = -1;
                }
                
                MailaWard_btn.gameObject.SetActive(true);
                ZEventSystem.Dispatch(EventConst.OnMailItemIncident, true, PostBoxMgr.Instance.MailData[idx]);
            }
        }
    }
    /// <summary>
    /// 删除邮件
    /// </summary>
    /// <param name="icoid"></param>
    public void Destroy(bool isdelete, MailDatas _mail)
    {
        for (int idx = 0; idx < MailList.Count; idx++)
        {
            if (MailList[idx].MailId == _mail.mail_id)
            {
                if (isdelete == false)
                {
                    MailList[idx].Ico.sprite = ResourceMgr.Instance.LoadSprite(Ico_id);
                  //  _mail.isRead = -1;
                   // MailaWard_obj.SetActive(false);
                }
                else
                {
                    MailList[idx].Close();
                    PostBoxMgr.Instance.Remove(_mail.mail_id);
                   // MailList[idx].Mail.onClick.RemoveAllListeners();
                    MailList[idx].gameObject.SetActive(false);
                }
                MailaWard_btn.gameObject.SetActive(false);
                ZEventSystem.Dispatch(EventConst.OnMailItemIncident, false, _mail);
                GetMail(MailType);
            }
        }
    }

    public void Close(bool iscos, object _mail)
    {
        MailDatas mail = null;
        if (_mail != null)
            mail = (MailDatas)_mail;
        if (iscos)
        {
            for (int idx = 0; idx < MailItemList.Count; idx++)
            {
                MailItemList[idx].idUse = false;
                MailItemList[idx].gameObject.SetActive(false);
            }
            for (int idx1 = 0; idx1 < ThreeList.Count; idx1++)
            {
                ThreeList[idx1].idUse = false;
                ThreeList[idx1].gameObject.SetActive(false);
            }
        }
        else
        {
            if (mail.mailitem.Count >0)
            {
                MailaWard_btn.gameObject.SetActive(false);
                ZEventSystem.Dispatch(EventConst.OnMailItemIncident, false, mail);
            }
        }
    }

    /// <summary>
    /// 整理邮件
    /// </summary>
    /// <param name="type"></param>
    public void GetMail(MailTyper type)
    {
        if (glg == null)
        {
            glg = show_trf.GetComponent<GridLayoutGroup>();
        }
        MailType = type;
        int num = 0;
        int count = 0;
        MailSort();
        for (int idx = 0; idx < MailList.Count; idx++)
        {
            if (type== MailTyper.zero)
            {
               // if (idx == 0) 
                if (idx >= PostBoxMgr.Instance.MailData.Count)
                {
                    MailList[idx].gameObject.SetActive(false);
                }
                else
                {
                    MailList[idx].gameObject.SetActive(true);
                    MailList[idx].Endow(PostBoxMgr.Instance.MailData[idx]);
                }
            }
            else
            {
                if (idx >= PostBoxMgr.Instance.MailData.Count)
                {
                    MailList[idx].gameObject.SetActive(false);
                }
                else
                {
                    if (PostBoxMgr.Instance.MailData[idx].type == (int)type)
                    {
                        MailList[idx].gameObject.SetActive(true);
                        MailList[idx].Endow(PostBoxMgr.Instance.MailData[idx]);
                        count++;
                    }
                    else
                        MailList[idx].gameObject.SetActive(false);
                }

            }
            if (idx < PostBoxMgr.Instance.MailData.Count)
            {
           
            if (PostBoxMgr.Instance.MailData[idx].isRead == -1)
            {
                MailList[idx].RedDot.SetActive(false);
                MailList[idx].Shade.SetActive(true);
                MailList[idx].Ico.sprite = ResourceMgr.Instance.LoadSprite(Ico_id);
            }
            else
            {
                MailList[idx].RedDot.SetActive(true);
                MailList[idx].Shade.SetActive(false);
                }
            }

        }
        Vector2 size = Control_sr.GetComponent<RectTransform>().sizeDelta;
        num = (int)(size.y / glg.cellSize.y);
        if (type == 0)
        {
            count = PostBoxMgr.Instance.MailData.Count;
            if (count > num)
                count = count - num;
            else
                count = 0;
        }
        else
            count = count - num;
        float y = 0; ;
        if (count > 0)
            y = Mathf.CeilToInt(count * glg.cellSize.y);

        show_trf.GetComponent<RectTransform>().offsetMin = new Vector2(0, -y);
        show_trf.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        ZEventSystem.Dispatch(EventConst.Incident, true);
    }


    /// <summary>
    /// 排序
    /// </summary>
    public void MailSort()
    {
        PostBoxMgr.Instance.MailData.Sort((MailDatas item1, MailDatas item2) =>
        {
           
            //邮件是否已读
            if (item1.isRead > item2.isRead)
            {
                return -1;   //左值小于右值,返回-1，为升序，如果返回1，就是降序
            }
            else if (item1.isRead < item2.isRead)
            {
                return 1;
            }
            else 
            {
                //时间进行排序
                if (item1.date < item2.date)
                {
                    return 1;
                }
                else if (item1.date > item2.date)
                {
                    return -1;
                }
                else
                {
                    if (PostBoxMgr.Instance.GetMailType(item1.mail_id) > PostBoxMgr.Instance.GetMailType(item2.mail_id))
                        return 1;
                    else if (PostBoxMgr.Instance.GetMailType(item1.mail_id) < PostBoxMgr.Instance.GetMailType(item2.mail_id))
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        });
    }
}

