using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour
{

    public Shader shader;
    void Start()
    {
        if (!_checkForSupport())
        {
            Debug.LogError("PostEffect Not Support！");
            this.enabled = false;
        }
    }

    private bool _checkForSupport()
    {
        return SystemInfo.supportsImageEffects;
    }

    private Material _material;
    public Material material
    {
        get
        {
            if (_material == null)
            {
                if (shader == null)
                    return null;
                _material = new Material(shader);
                _material.hideFlags = HideFlags.DontSave;
            }
            return _material;
        }
    }
}