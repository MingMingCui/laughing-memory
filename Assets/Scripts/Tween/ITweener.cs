using UnityEngine;
using System.Collections.Generic;

public abstract class ITweener : MonoBehaviour
{

    static public ITweener current;

	public enum Method
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut,
		BounceIn,
		BounceOut,
	}

	public enum Style
	{
		Once,
		Loop,
		PingPong,
	}
    [HideInInspector]
    protected Method method = Method.Linear;
    [HideInInspector]
    protected Style style = Style.Once;
    [HideInInspector]
    protected AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));	
	[HideInInspector]
    protected bool ignoreTimeScale = false;
    [HideInInspector]
    protected float delay = 0.1f;
    [HideInInspector]
    protected float duration = 1f;
	[HideInInspector]
    protected bool steeperCurves = false;
    [HideInInspector]
    protected int tweenGroup = 0;
	[HideInInspector]
    public List<FinishedEvent> onFinished = new List<FinishedEvent>();
    public delegate void FinishedEvent();

	bool mStarted = false;
	float mStartTime = 0f;
	float mDuration = 0f;
	float mAmountPerDelta = 1000f;
	float mFactor = 0f;
    public float Duration
    { 
        get { return duration; } 
    }
    public float Delay
    {
        get { return delay; }
    }
	public float amountPerDelta
	{
		get
		{
			if (duration == 0f) return 1000f;

			if (mDuration != duration)
			{
				mDuration = duration;
				mAmountPerDelta = Mathf.Abs(1f / duration) * Mathf.Sign(mAmountPerDelta);
			}
			return mAmountPerDelta;
		}
	}

	public float tweenFactor { get { return mFactor; } set { mFactor = Mathf.Clamp01(value); } }

	void Reset ()
	{
		if (!mStarted)
		{
			SetStartToCurrentValue();
			SetEndToCurrentValue();
		}
	}
	protected virtual void Start () { Update(); }

	void Update ()
	{
        float delta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        float time = ignoreTimeScale ? Time.unscaledDeltaTime : Time.time;

		if (!mStarted)
		{
			delta = 0;
			mStarted = true;
			mStartTime = time + delay;
		}

		if (time < mStartTime) return;

		mFactor += (duration == 0f) ? 1f : amountPerDelta * delta;

		if (style == Style.Loop)
		{
			if (mFactor > 1f)
			{
				mFactor -= Mathf.Floor(mFactor);
			}
		}
		else if (style == Style.PingPong)
		{
			if (mFactor > 1f)
			{
				mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
				mAmountPerDelta = -mAmountPerDelta;
			}
			else if (mFactor < 0f)
			{
				mFactor = -mFactor;
				mFactor -= Mathf.Floor(mFactor);
				mAmountPerDelta = -mAmountPerDelta;
			}
		}

		if ((style == Style.Once) && (duration == 0f || mFactor > 1f || mFactor < 0f))
		{
			mFactor = Mathf.Clamp01(mFactor);
			Sample(mFactor, true);
			enabled = false;

			if (current != this)
			{
                ITweener before = current;
				current = this;

				if (onFinished != null)
				{
                    for (int i = 0; i < onFinished.Count; ++i)
					{
                        FinishedEvent ed = onFinished[i];
                        if (ed != null)
                            ed();
					}
				}
				current = before;
			}
		}
		else Sample(mFactor, false);
	}
    public void SetOnFinished(List<FinishedEvent> onFinished) 
    { 
        this.onFinished = onFinished;
    }
    public void AddOnFinished(FinishedEvent call) 
    { 
        if (onFinished == null) 
            onFinished = new List<FinishedEvent>();
        this.onFinished.Add(call); 
    }
    public void RemoveOnFinished(FinishedEvent call) 
    { 
        if (onFinished != null) 
            this.onFinished.Remove(call); 
    }
    public void ClearOnFinished() 
    { 
        if (onFinished != null)
            onFinished.Clear(); 
        onFinished = null; 
    }
	void OnDisable () { mStarted = false; }

	public void Sample (float factor, bool isFinished)
	{
		float val = Mathf.Clamp01(factor);

		if (method == Method.EaseIn)
		{
			val = 1f - Mathf.Sin(0.5f * Mathf.PI * (1f - val));
			if (steeperCurves) val *= val;
		}
		else if (method == Method.EaseOut)
		{
			val = Mathf.Sin(0.5f * Mathf.PI * val);

			if (steeperCurves)
			{
				val = 1f - val;
				val = 1f - val * val;
			}
		}
		else if (method == Method.EaseInOut)
		{
			const float pi2 = Mathf.PI * 2f;
			val = val - Mathf.Sin(val * pi2) / pi2;

			if (steeperCurves)
			{
				val = val * 2f - 1f;
				float sign = Mathf.Sign(val);
				val = 1f - Mathf.Abs(val);
				val = 1f - val * val;
				val = sign * val * 0.5f + 0.5f;
			}
		}
		else if (method == Method.BounceIn)
		{
			val = BounceLogic(val);
		}
		else if (method == Method.BounceOut)
		{
			val = 1f - BounceLogic(1f - val);
		}

		OnUpdate((animationCurve != null) ? animationCurve.Evaluate(val) : val, isFinished);
	}

	float BounceLogic (float val)
	{
		if (val < 0.363636f) // 0.363636 = (1/ 2.75)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f) // 0.727272 = (2 / 2.75)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f; // 0.545454f = (1.5 / 2.75) 
		}
		else if (val < 0.909090f) // 0.909090 = (2.5 / 2.75) 
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f; // 0.818181 = (2.25 / 2.75) 
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f; // 0.9545454 = (2.625 / 2.75) 
		}
		return val;
	}

	public void PlayForward () { Play(true); }

	public void PlayReverse () { Play(false); }

	private void Play (bool forward)
	{
		mAmountPerDelta = Mathf.Abs(amountPerDelta);
		if (!forward) mAmountPerDelta = -mAmountPerDelta;
		enabled = true;
		Update();
	}

	public void ResetToBeginning ()
	{
		mStarted = false;
		mFactor = (amountPerDelta < 0f) ? 1f : 0f;
		Sample(mFactor, false);
	}

	public void Toggle ()
	{
		if (mFactor > 0f)
		{
			mAmountPerDelta = -amountPerDelta;
		}
		else
		{
			mAmountPerDelta = Mathf.Abs(amountPerDelta);
		}
		enabled = true;
	}

	abstract protected void OnUpdate (float factor, bool isFinished);

    static public T Begin<T>(GameObject go, float duration) where T : ITweener
	{
		T comp = go.GetComponent<T>();

		if (comp != null && comp.tweenGroup != 0)
		{
			comp = null;
			T[] comps = go.GetComponents<T>();
			for (int i = 0, imax = comps.Length; i < imax; ++i)
			{
				comp = comps[i];
				if (comp != null && comp.tweenGroup == 0) break;
				comp = null;
			}
		}

		if (comp == null)
		{
			comp = go.AddComponent<T>();

			if (comp == null)
			{
				Debug.LogError("Unable to add " + typeof(T) + " to :" + go);
				return null;
			}
		}
		comp.mStarted = false;
		comp.mFactor = 0f;
		comp.duration = duration;
		comp.mDuration = duration;
		comp.mAmountPerDelta = duration > 0f ? Mathf.Abs(1f / duration) : 1000f;
		comp.style = Style.Once;
		comp.animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		comp.enabled = true;
		return comp;
	}


	public virtual void SetStartToCurrentValue () { }

	public virtual void SetEndToCurrentValue () { }
}
