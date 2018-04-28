using UnityEngine;

public class RotationTween : ITweener
{
	public Vector3 from;
	public Vector3 to;
	public bool quaternionLerp = false;
    public Style mStyle = Style.Once;
    public AnimationCurve mAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    public float mRosDelay = 0f;
    public float mRosDuration = 1f;
	Transform mTrans;
    void Awake()
    {
        base.style = mStyle;
        base.animationCurve = mAnimationCurve;
        base.delay = mRosDelay;
        base.duration = mRosDuration;
    }
	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	public Quaternion value { get { return cachedTransform.localRotation; } set { cachedTransform.localRotation = value; } }


	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor) :
			Quaternion.Euler(new Vector3(
			Mathf.Lerp(from.x, to.x, factor),
			Mathf.Lerp(from.y, to.y, factor),
			Mathf.Lerp(from.z, to.z, factor)));
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value.eulerAngles; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value.eulerAngles; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = Quaternion.Euler(from); }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = Quaternion.Euler(to); }
}
