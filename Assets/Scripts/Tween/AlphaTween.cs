using UnityEngine;
using UnityEngine.UI;

public class AlphaTween : ITweener
{
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;
    public Style mStyle = Style.Once;
    public float mAlphaDelay = 0f;
    public float mAlphaDuration = 1f;
	bool mCached = false;
	Material mMat;
	Image  image;
    void Awake()
    {
        base.style = mStyle;
        base.delay = mAlphaDelay;
        base.duration = mAlphaDuration;
    }

	void Cache ()
	{
		mCached = true;
        image = GetComponent<Image>();

		if (image == null)
		{
			Renderer ren = GetComponent<Renderer>();
			if (ren != null) mMat = ren.material;
		}
	}

	public float value
	{
		get
		{
			if (!mCached) Cache();
			if (image != null) return image.color.a;
			return mMat != null ? mMat.color.a : 1f;
		}
		set
		{
			if (!mCached) Cache();

            if (image != null)
			{
				Color c = image.color;
				c.a = value;
                image.color = c;
			}
			else if (mMat != null)
			{
				Color c = mMat.color;
				c.a = value;
				mMat.color = c;
			}
		}
	}

	protected override void OnUpdate (float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
