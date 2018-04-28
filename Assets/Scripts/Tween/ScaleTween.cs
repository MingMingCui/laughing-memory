using UnityEngine;

public class ScaleTween : ITweener
{
	public Vector3 from = Vector3.one;
	public Vector3 to = Vector3.one;
    public Style mStyle = Style.Once;
    public AnimationCurve mAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    public float mScaDelay = 0f;
    public float mScaDuration = 1f;
	Transform mTrans;
    void Awake()
    {
        base.style = mStyle;
        base.animationCurve = mAnimationCurve;
        base.delay = mScaDelay;
        base.duration = mScaDuration;
    }
	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	public Vector3 value { get { return cachedTransform.localScale; } set { cachedTransform.localScale = value; } }

	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = from * (1f - factor) + to * factor;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}
