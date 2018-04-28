using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISprite : MonoBehaviour
{
    [HideInInspector]
    public string movieName;
    public List<Sprite> Sprites;
    public float fSep = 0.05f;

    public bool playOnce;

    private bool play  = true;

    public float showerWidth
    {
        get
        {
            if (shower == null)
            {
                return 0;
            }
            return shower.rectTransform.rect.width;
        }
    }
    public float showerHeight
    {
        get
        {
            if (shower == null)
            {
                return 0;
            }
            return shower.rectTransform.rect.height;
        }
    }

    void Awake()
    {
        shower = GetComponent<Image>();

        if (string.IsNullOrEmpty(movieName))
        {
            movieName = "movieName";
        }
        if (playOnce)
        {
            play = false;
            RegistMovieEvent(Sprites.Count - 1, delegate { shower.gameObject.SetActive(false); play = false; });
        }
    }
    void Start()
    {
        Play(curFrame);
    }

    public void Play()
    {
        shower.gameObject.SetActive(true);
        curFrame = 0;
        play = true;
    }

    public void Play(int iFrame)
    {
        if (iFrame >= FrameCount)
        {
            iFrame = 0;
        }
        shower.sprite = Sprites[iFrame];
        curFrame = iFrame;
        shower.SetNativeSize();

        if (dMovieEvents.ContainsKey(iFrame))
        {
            dMovieEvents[iFrame]();
        }
    }

    private Image shower;

    int curFrame = 0;
    public int FrameCount
    {
        get
        {
            return Sprites.Count;
        }
    }


    float fDelta = 0;
    void Update()
    {
        if (!play)
            return;
        fDelta += Time.deltaTime;
        if (fDelta > fSep)
        {
            fDelta = 0;
            curFrame++;
            Play(curFrame);
        }
    }

    public delegate void delegateMovieEvent();
    private Dictionary<int, delegateMovieEvent> dMovieEvents = new Dictionary<int, delegateMovieEvent>();
    public void RegistMovieEvent(int frame, delegateMovieEvent delEvent)
    {
        if (!dMovieEvents.ContainsKey(frame))
        {
            dMovieEvents.Add(frame, delEvent);
        }
        else
        {
            dMovieEvents[frame] += delEvent;
        }
    }
    public void UnregistMovieEvent(int frame, delegateMovieEvent delEvent)
    {
        if (!dMovieEvents.ContainsKey(frame))
            return;
        if(dMovieEvents[frame] != null)
            dMovieEvents[frame] -= delEvent;
    }

}