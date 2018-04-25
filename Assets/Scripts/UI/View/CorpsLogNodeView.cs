using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpsLogNodeView : MonoBehaviour {

    public GameObject bg_obj = null;

    public Text date_txt = null;

    public Text log_txt = null;

    public void SetInfo(bool dateOrLog, string content)
    {
        if (dateOrLog)
            this.date_txt.text = content;
        else
            this.log_txt.text = content;

        this.bg_obj.SetActive(dateOrLog);
        this.date_txt.gameObject.SetActive(dateOrLog);
        this.log_txt.gameObject.SetActive(!dateOrLog);
    }
}
