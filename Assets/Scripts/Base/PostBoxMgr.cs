using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailDatas
{
    public int mail_id;                 //邮件id
    public int ico_id;                  //图标id
    public int type;                    //邮件类型
    public string mailname;             //发件标题
    public string addresser;            //发件人
    public System.DateTime date;                 //发件时间
    public string content;              //邮件内容 
    public int isRead;                 //是否已读
    public List<Item> mailitem = new List<Item>();         //邮件附件
}


public class PostBoxMgr : Singleton<PostBoxMgr>
{
    public List<MailDatas> MailData = new List<MailDatas>();
    public bool isUpd = false;

    /// <summary>
    /// 通过MailID获取Mail类型
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public int  GetMailType(int _id)
    {
        int type = 0;
        for (int idx = 0; idx < MailData.Count; idx++)
        {
            if (MailData[idx].mail_id == _id)
            {
                type = MailData[idx].type;
                break;
            }
            else if (idx == MailData.Count-1)
            {
                Debug.Log("不包含次邮件ID"+_id);
                return 0;
            }
        }
        return type;
    }

     

    /// <summary>
    /// 删除邮件数据
    /// </summary>
    /// <param name="_mailid"></param>
    public void Remove(int _mailid)
    {
        for (int idx = 0; idx < MailData.Count; idx++)
        {
            if (MailData[idx].mail_id == _mailid)
            {
                MailData.RemoveAt(idx);
            }
        }

    }

}
