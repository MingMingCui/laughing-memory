using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDissolve : PostEffectBase {

    //溶解原图
    public Texture SourceTexture = null;
    //噪声图
    public Texture NoiseMap = null;
    //溶解颜色
    public Color DissolveColor = Color.black;
    //溶解边缘
    public float DissolveBorder = 0.1f;
    //溶解程度
    [Range(0, 1)]
    public float Dissolve = 0;
    //初始化材质
    private bool isInitMat = false;

    private void InitMat()
    {
        if (material != null && !isInitMat)
        {
            material.SetTexture("_SourceMap", SourceTexture);
            material.SetTexture("_NoiseMap", NoiseMap);
            material.SetColor("_DissolveColor", DissolveColor);
            material.SetFloat("_DissolveBorder", DissolveBorder);
            isInitMat = true;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            InitMat();
            material.SetFloat("_Dissolve", Dissolve);
            Graphics.Blit(source, destination, material);
        }
        else
            Graphics.Blit(source, destination);
    }
}
