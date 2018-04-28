using UnityEngine;
using UnityEngine.UI;

public class RecruitingProbabilityViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject allhero_obj;
    [HideInInspector]
    public GameObject hero_obj;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       allhero_obj = mTransform.Find("Image (1)/allhero_obj").gameObject;
       hero_obj = mTransform.Find("Image (1)/allhero_obj/hero_obj").gameObject;
    }
}