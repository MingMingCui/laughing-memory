using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class PopTextMgr : MonoBehaviour
{
    [Tooltip("UI控件")]
    public List<GameObject> PopTexts = null;
    [Tooltip("时长，所有动画都会应用这个时间，不能为0")]
    public float Duration = 1;
    [Tooltip("位移最大值，X轴动画和Y轴动画都会应用这个最大值")]
    public float MaxDis = 1;
    [Tooltip("是否使用X轴动画")]
    public bool IsAnimX;
    [Tooltip("X轴动画曲线")]
    public AnimationCurve AnimX;
    [Tooltip("是否使用Y轴动画")]
    public bool IsAnimY;
    [Tooltip("Y轴动画曲线")]
    public AnimationCurve AnimY;
    [Tooltip("是否使用缩放动画")]
    public bool IsAnimScale;
    [Tooltip("缩放动画曲线")]
    public AnimationCurve AnimScale;
    [Tooltip("是否使用透明度动画")]
    public bool IsAnimAlpha;
    [Tooltip("透明度动画曲线")]
    public AnimationCurve AnimAlpha;


    private class TextObject
    {
        public RectTransform TexGo;
        public Text TextComp;
        public CanvasGroup CanvasGroupComp;
        public bool Using;
        public float UsingTime;
    }

    private List<List<TextObject>> _textPool = new List<List<TextObject>>();
    private List<Vector3> oldSize = new List<Vector3>();
    private List<Vector2> oldPos = new List<Vector2>();
	// Use this for initialization
	void Start ()
	{
	    if (PopTexts == null)
	        return;

        for (int idx = 0; idx < PopTexts.Count; ++idx)
        {
            RectTransform popRect = PopTexts[idx].GetComponent<RectTransform>();
            Text popText = PopTexts[idx].GetComponentInChildren<Text>();
            CanvasGroup groupComp = PopTexts[idx].GetComponentInChildren<CanvasGroup>();
            if (popText == null)
            {
                EDebug.LogErrorFormat("PopMgr.Start Get Text Component in {0} failed", PopTexts[idx]);
                continue;
            }
            if (groupComp == null)
            {
                EDebug.LogErrorFormat("PopMgr.Start Get CanvasGroup Component in {0} failed", PopTexts[idx]);
                continue;
            }
            oldSize.Add(popRect.localScale);
            oldPos.Add(popRect.anchoredPosition);
            _textPool.Add(new List<TextObject>());
        }
	    if (Duration == 0)
	        Duration = 1;

	}
	
	// Update is called once per frame
	void Update ()
	{
#if UNITY_EDITOR
	    if (Duration == 0)
	        Duration = 1;
#endif
        for (int idx = 0; idx < _textPool.Count; ++idx)
	    {
            for (int idx2 = 0; idx2 < _textPool[idx].Count; ++idx2)
            {
                TextObject to = _textPool[idx][idx2];
                if (!to.Using)
                    continue;
                float last = Time.time - to.UsingTime;
                if (last >= Duration)
                {
                    to.Using = false;
                    to.TexGo.anchoredPosition = oldPos[idx];
                    to.TexGo.localScale = oldSize[idx];
                    to.CanvasGroupComp.alpha = 0;
                }
                else
                {
                    float animPos = last / Duration;
                    float x = 0;
                    float y = 0;
                    float scale = 1;
                    float alpha = 1;
                    if (IsAnimX)
                        x = AnimX.Evaluate(animPos) * MaxDis;
                    if (IsAnimY)
                        y = AnimY.Evaluate(animPos) * MaxDis;
                    if (IsAnimScale)
                        scale = AnimScale.Evaluate(animPos);
                    if (IsAnimAlpha)
                        alpha = AnimAlpha.Evaluate(animPos);
                    to.TexGo.anchoredPosition = oldPos[idx] + new Vector2(x, y);
                    to.TexGo.transform.localScale = Vector3.one * scale;
                    to.CanvasGroupComp.alpha = alpha;
                }
            }
	    }
	}

    public void AddInfo(string info, int id)
    {
        if (info == "")
            return;
        TextObject to = CreateTextObject(id);
        to.TextComp.text = info;
    }

    public void Clear()
    {
        for (int idx = 0; idx < _textPool.Count; ++idx)
        {
            for (int idx2 = 0; idx2 < _textPool[idx].Count; ++idx2)
            {
                Destroy(_textPool[idx][idx2].TexGo.gameObject);
            }
        }
        _textPool.Clear();
    }


    private TextObject CreateTextObject(int id)
    {
        TextObject ret = null;
        List<TextObject> _buffer = _textPool[id];
        for (int idx = 0; idx < _buffer.Count; ++idx)
        {
            if (!_buffer[idx].Using)
            {
                ret = _buffer[idx];
                break;
            }
        }

        if (ret == null)
        {
            ret = new TextObject();
            ret.TexGo = GameObject.Instantiate(PopTexts[id]).GetComponent<RectTransform>();
            ret.TextComp = ret.TexGo.GetComponentInChildren<Text>();
            ret.CanvasGroupComp = ret.TexGo.GetComponentInChildren<CanvasGroup>();
            ret.TexGo.transform.SetParent(this.transform, false);
            _buffer.Add(ret);
        }
        ret.TexGo.anchoredPosition = Vector2.zero;
        ret.Using = true;
        ret.UsingTime = Time.time;
        ret.CanvasGroupComp.alpha = 1;
        return ret;
    }
}
