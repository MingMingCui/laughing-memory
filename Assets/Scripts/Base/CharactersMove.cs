using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersMove : MonoBehaviour {

    [Tooltip("时长，所有动画都会应用这个时间，不能为0")]
    public float Duration = 1;
    [Tooltip("位移最大值，X轴动画和Y轴动画都会应用这个最大值")]
    public float MaxDis = 1;

    [Tooltip("Y轴动画曲线")]
    public AnimationCurve AnimY;

    public RectTransform rect;
    public Vector3 curPosition;
    float TimeCount= 0;//计时器
    float y = 0;
    void Start()
    {
        rect = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Duration == 0)
            Duration = 1;
        TimeCount+=Time.deltaTime;
      
        if (rect.anchoredPosition.y < -800)
        {
            rect.anchoredPosition = curPosition;
            TimeCount = 0;
            y = 0;
        }else
        {
            float animPos = TimeCount / Duration;
            y = AnimY.Evaluate(animPos) * MaxDis;
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y-y, 0);
        }

    }
}
