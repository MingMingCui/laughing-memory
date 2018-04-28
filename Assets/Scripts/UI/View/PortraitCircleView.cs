using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitCircleView : MonoBehaviour
{

    [HideInInspector]
    public Button Select;
    [HideInInspector]
    public Image Portrait;
    [HideInInspector]
    public GameObject Hint;
    [HideInInspector]
    public GameObject Clear;
    [HideInInspector]
    public int ID;
    [HideInInspector]
    public int isRelieve;
    [HideInInspector]
    public GameObject SelectImag;
    public void Init()
    {
        Select = transform.GetComponent<Button>();
        Portrait = transform.Find("Portrait").GetComponent<Image>();
        Hint = transform.Find("Hint").gameObject;
        Clear = transform.Find("Clear").gameObject;
        SelectImag = transform.Find("Select").gameObject;
    }

    public void Endow(int id,bool isclear)
    {
        ID = id;
        Portrait.sprite = ResourceMgr.Instance.LoadSprite(id);
    
        if (isclear)
        {
            if (id == Role.Instance.HeadId)
            {
                Hint.SetActive(true);
                Clear.SetActive(false);
                SelectImag.SetActive(true);
            }
            else
            {
                Hint.SetActive(false);
                SelectImag.SetActive(false);
            }
            isRelieve = 1;
        }
        else
        {
            isRelieve = 2;
           Clear.SetActive(true);
        }
          
       

    }
}
