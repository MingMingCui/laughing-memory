using UnityEngine;
using UnityEngine.UI;

public class DataAnalyzeView : MonoBehaviour {

    public HeroHeadView heroHeadView = null;
    [HideInInspector]
    public Image harm_pro_img = null;
    [HideInInspector]
    public Text data_text = null;
    [HideInInspector]
    public GameObject HeadPos_obj = null;


    public void Init () {
        Transform mTransform = this.transform;
        harm_pro_img = mTransform.Find("harm_pro_img").GetComponent<Image>();
        data_text = mTransform.Find("data_text").GetComponent<Text>();
        HeadPos_obj = mTransform.Find("HeadPos_obj").gameObject;

    }

    public void SetInfo(int headId, int rare, int star, int level, int harm, int max)
    {
        if (heroHeadView != null)
        {
            GameObject node = GameObject.Instantiate(heroHeadView.gameObject, HeadPos_obj.transform, false);
            node.transform.localPosition = Vector3.zero;
            HeroHeadView headView = node.GetComponent<HeroHeadView>();
            headView.Init();
            headView.SetHeroInfo(headId, rare, star, level);
        }
        data_text.text = harm.ToString("N0");
        harm_pro_img.fillAmount = max > 0 ? ((float)harm / max) : 1;
    }
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
