using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasView : MonoBehaviour {

    private static CanvasView _instance = null;

    public static Transform CanvasRoot;
    public static GraphicRaycaster graphic;
    public Canvas canvas = null;

    public Camera uicamera = null;

    public RectTransform canvasRect = null;

    public GameObject loading = null;

    public Image loading_cur = null;

    public Text loading_txt = null;

    public GameObject notice = null;

    public PopTextMgr notice_pop = null;

    public readonly float MB = 1024 * 1024;

    public bool EnableDebugUILine = true;

    public GameObject ConnectObj = null;

    private readonly float designWidth = 1920f;
    private readonly float designHeight = 1080f;

    public static CanvasView Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
        CanvasRoot = transform;
        graphic = CanvasRoot.GetComponent<GraphicRaycaster>();
        canvas = CanvasRoot.GetComponent<Canvas>();
        uicamera = CanvasRoot.Find("Camera").GetComponent<Camera>();
        canvasRect = CanvasRoot.GetComponent<RectTransform>();
        loading = CanvasRoot.Find("Loading").gameObject;
        loading_cur = loading.transform.Find("loading_cur").GetComponent<Image>();
        loading_txt = loading.transform.Find("loading_txt").GetComponent<Text>();

        notice = this.transform.Find("Notice").gameObject;
        notice_pop = notice.GetComponent<PopTextMgr>();
        //JsonMgr.GetSingleton();

#if UNITY_EDITOR
        if(EnableDebugUILine)
            this.gameObject.AddComponent<DebugUILine>();
#endif

        //调整分辨率
        float designAspect = designWidth / designHeight;
        float aspect = Screen.width / Screen.height;
        this.GetComponent<CanvasScaler>().matchWidthOrHeight = (aspect > designAspect ? 1 : 0);
    }

    public void AddNotice(string notice)
    {
        notice_pop.AddInfo(notice, 0);
    }

    public void OpenUncompress()
    {
        SetUncompress(0);
        loading.SetActive(true);
    }

    public void OpenLoading()
    {
        SetLoading(0);
        loading.SetActive(true);
    }

    public void OpenDownloading()
    {
        SetDownloading(0, 0, 0);
        loading.SetActive(true);
    }

    public void CloseLoading()
    {
        loading.SetActive(false);
    }

    public void SetUncompress(float progress)
    {
        loading_cur.fillAmount = progress;
        loading_txt.text = string.Format("正在解压资源{0}%，不消耗流量", (int)(progress * 100));
    }

    public void SetLoading(float progress)
    {
        loading_cur.fillAmount = progress;
        loading_txt.text = string.Format("资源加载中{0}%", (int)(progress * 100));
    }

    public void SetDownloading(int curByte, uint totalByte, float speed)
    {
        float progress = (float)curByte / totalByte;
        loading_cur.fillAmount = progress;
        loading_txt.text = string.Format("下载中：{0:F}mb/{1:F}mb，当前速度：{2:F}mb/s", curByte / MB, totalByte / MB, speed / MB);
    }

    public void OpenConnect(bool open)
    {
        this.ConnectObj.SetActive(open);
    }

    public void OnClickReConnection()
    {
        OpenConnect(false);
        Client.Instance.Connect();
    }

}
