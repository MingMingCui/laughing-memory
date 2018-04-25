using UnityEngine;
using UnityEngine.UI;

public class DivinationTree : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text spend_txt;
    [HideInInspector]
    public ScaleTween st;
    [HideInInspector]
    public Button get_btn;
    private CanvasRenderer[] cr;
    public virtual void Awake()
    {
        this.mTransform = this.transform;
        st = mTransform.GetComponent<ScaleTween>();
        get_btn = mTransform.Find("get_btn").GetComponent<Button>();
        spend_txt = mTransform.Find("spend_txt").GetComponent<Text>();
        cr = GetComponentsInChildren<CanvasRenderer>();
    }
    public int progress;

    public int RandomTree()
    {
        int ret = Random.Range(0, 5);
        return ret;
    }
    public void SetView(int index)
    {
        spend_txt.text = JsonMgr.GetSingleton().GetTotemTreeByID(index + 1).spend.ToString();
    }

    private void OnEnable()
    {
        for (int i = 1; i < cr.Length; ++i)
        {
            cr[i].SetAlpha(1);
        }
    }
    private void OnDisable()
    {
        for (int i = 1; i < cr.Length; ++i)
        {
            cr[i].SetAlpha(0);
        }
    }
}
