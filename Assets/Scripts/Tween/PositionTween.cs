using UnityEngine;

public class PositionTween : ITweener
{
    public Vector3 from;
    public Vector3 to;
    public Style mStyle = Style.Once;
    public AnimationCurve mAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    public float mPosDelay = 0.1f;
    public float mPosDuration = 1f;

    Transform mTrans;
    void Awake()
    {
        base.style = mStyle;
        base.animationCurve = mAnimationCurve;
        base.delay = mPosDelay;
        base.duration = mPosDuration;
    }
    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

    public Vector3 value
    {
        get
        {
            return cachedTransform.localPosition;
        }
        set
        {
            cachedTransform.localPosition = value;
        }
    }
    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }
}
