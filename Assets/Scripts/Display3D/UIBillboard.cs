using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIBillboard : MonoBehaviour {
    //RenderTexture宽度
    public int TexWidth = 256;
    //RenderTexture高度
    public int TexHeight = 256;

    //默认显示层级
    public string DefaultCullMask = "UI";
    //是否是正交相机
    public bool IsOrtho = true;
    //正交相机大小
    public float OrthoSize { get { return Cam.orthographicSize; }set { Cam.orthographicSize = value; } }
    //FOV大小
    public float Fov { get { return Cam.fieldOfView; } set { Cam.fieldOfView = value; } }

    //使用次数，保证每次创建摄像机的位置都不一样
    private static int _uid = 0;
    private const float CAM_SPACE = 1000;
    //渲染用摄像机
    private GameObject camGo = null;
    public Camera Cam { get; private set; }
    //渲染纹理
    private RenderTexture _rt = null;

    public int Layer
    {
        get {
            int layer = Cam.cullingMask;
            int layerCnt = 0;
            while (layer > 0)
            {
                ++layerCnt;
                layer = layer >> 1;
            }
            return layerCnt - 1;
        }
        set {
            Cam.cullingMask = 1 << value;
        }
    }

    void Awake()
    {
        ++_uid;
        camGo = new GameObject("_billboardCam");
        camGo.transform.position = new Vector3(10000 + _uid * CAM_SPACE, 10000, 10000);
        Cam = camGo.AddComponent<Camera>();
        _rt = new RenderTexture(TexWidth, TexHeight, 24);
        Cam.targetTexture = _rt;
        Cam.orthographicSize = OrthoSize;
        Cam.fieldOfView = Fov;
        Cam.orthographic = IsOrtho;
        Layer = LayerMask.NameToLayer(DefaultCullMask);
        RawImage destImage = this.GetComponent<RawImage>();
        destImage.material = new Material(Shader.Find("Particles/Alpha Blended"));
        destImage.texture = _rt;
    }

    public Vector3 GetCamPos()
    {
        return camGo.transform.position;
    }
}
