using UnityEngine;
using UnityEngine.UI;

public class ColorTween : ITweener
{
	public Color from = Color.white;
	public Color to = Color.white;
    public Style mStyle = Style.Once;
    public float mColDelay = 0f;
    public float mColDuration = 1f;
	bool mCached = false;
	Material mMat;
	Light mLight;
	Image image;
    void Awake()
    {
        base.style = mStyle;
        base.delay = mColDelay;
        base.duration = mColDuration;
    }
	void Cache ()
	{
		mCached = true;

        image = GetComponent<Image>();
		if (image != null) return;

		Renderer ren = GetComponent<Renderer>();

		if (ren != null)
		{
			mMat = ren.material;
			return;
		}
		mLight = GetComponent<Light>();
	}

	public Color value
	{
		get
		{
			if (!mCached) Cache();
			if (mMat != null) return mMat.color;
			if (image != null) return image.color;
			if (mLight != null) return mLight.color;
			return Color.black;
		}
		set
		{
			if (!mCached) Cache();
			else if (mMat != null) mMat.color = value;
			else if (image != null) image.color = value;
			else if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
			}
		}
	}

	protected override void OnUpdate (float factor, bool isFinished) { value = Color.Lerp(from, to, factor); }

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}
