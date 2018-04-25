using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarView : MonoBehaviour {

    public RectTransform Star = null;

    private List<RectTransform> _allStars = new List<RectTransform>();

    public void SetStar(int n)
    {
        if (_allStars.Count < n)
        {
            int needStar = n - _allStars.Count;
            for (int idx = 0; idx < needStar; ++idx)
            {
                GameObject star = GameObject.Instantiate(Star.gameObject, this.transform);
                _allStars.Add(star.GetComponent<RectTransform>());
            }
        }
        for (int idx = 0; idx < _allStars.Count; ++idx)
        {
            RectTransform star = _allStars[idx];
            if (idx >= n)
            {
                star.gameObject.SetActive(false);
            }
            else
            {
                star.gameObject.SetActive(true);
                star.anchoredPosition = new Vector2((idx - _allStars.Count / 2.0f + 0.5f) * star.sizeDelta.x, 0);
            }
        }
    }
}
