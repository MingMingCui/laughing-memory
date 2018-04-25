using UnityEngine;


public class TransformTween : ITweener
{
	public Transform from;
	public Transform to;
	public bool parentWhenFinished = false;
    public Style mStyle = Style.Once;
    public float mTranDelay = 0f;
    public float mTranDuration = 1f;
	Transform mTrans;
	Vector3 mPos;
	Quaternion mRot;
	Vector3 mScale;
    void Awake()
    {
        base.style = mStyle;
        base.delay = mTranDelay;
        base.duration = mTranDuration;
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
		if (to != null)
		{
			if (mTrans == null)
			{
				mTrans = transform;
				mPos = mTrans.position;
				mRot = mTrans.rotation;
				mScale = mTrans.localScale;
			}

			if (from != null)
			{
				mTrans.position = from.position * (1f - factor) + to.position * factor;
				mTrans.localScale = from.localScale * (1f - factor) + to.localScale * factor;
				mTrans.rotation = Quaternion.Slerp(from.rotation, to.rotation, factor);
			}
			else
			{
				mTrans.position = mPos * (1f - factor) + to.position * factor;
				mTrans.localScale = mScale * (1f - factor) + to.localScale * factor;
				mTrans.rotation = Quaternion.Slerp(mRot, to.rotation, factor);
			}

			if (parentWhenFinished && isFinished) mTrans.parent = to;
		}
	}
}
