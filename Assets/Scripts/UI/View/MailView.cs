using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailView : MonoBehaviour
{
    [HideInInspector]
    public Button Mail;
    [HideInInspector]
    public int isUse; //是否添加
    [HideInInspector]
    public GameObject RedDot;
    [HideInInspector]
    public Text Addresser;
  //  [HideInInspector]
  //  public int Ico_id;
    [HideInInspector]
    public Image Ico;
    [HideInInspector]
    public GameObject Shade;
    [HideInInspector]
    public int MailId; //邮件iD
    [HideInInspector]
    public Text title_txt;
    [HideInInspector]
    public int count; //附件长度
    [HideInInspector]
    public System.DateTime time;
    [HideInInspector]
    public int MailType; //邮件类型
    private Text Date;




  public void Init()
    {
        if (Mail != null) return;
        Mail = transform.GetComponent<Button>();
        Ico = transform.Find("ico_img").GetComponent<Image>();
        Date = transform.Find("date_txt").GetComponent<Text>();
        Addresser = transform.Find("addresser/addresser_txt").GetComponent<Text>();
        Shade = transform.Find("shade").gameObject;
        RedDot = transform.Find("reddot_obj").gameObject;
        title_txt = transform.Find("title_txt").GetComponent<Text>();
    }
	public void Endow(MailDatas mail)
    {
        MailType = mail.type;

        time = mail.date;
        count = mail.mailitem.Count;
        MailId = mail.mail_id;
        Ico.sprite = ResourceMgr.Instance.LoadSprite(mail.ico_id);
        Addresser.text = mail.addresser;
        Date.text = mail.date.Year.ToString() + "." + mail.date.Month.ToString() + "." + mail.date.Day.ToString();
        title_txt.text = mail.mailname;
     }
	public void Close()
    {
        MailType = -1;
        count = 0;
        MailId = 0;
    }
}
