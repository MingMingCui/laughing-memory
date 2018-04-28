using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/ToggleControl", 155)]
public class ToogleExpand : MonoBehaviour
{
    [Header("Toggle组显示一个还是隐藏一个")]
    public bool which;

    private Toggle toggle;
    private CanvasRenderer cr;

	void Awake ()
    {
        toggle = GetComponent<Toggle>();
        cr = GetComponent<CanvasRenderer>();
        toggle.onValueChanged.AddListener(ChangeValue);
    }	
    void ChangeValue(bool isOn)
    {
        cr.SetAlpha(isOn ? which ? 1 : 0 : which ? 0 : 1);
    }
}
