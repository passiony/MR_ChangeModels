using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Model
{
    public GameObject gameObject;
    public MeshRenderer mesh;
    public Material[] materials;
    public int matIndex;
    public int metallicIndex;
    public int transparentIndex;
}

public class ModelController : MonoBehaviour
{
    public static ModelController Instance;

    public Model[] models;
    public int ModelIndex;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeMat(int index)
    {
        models[ModelIndex].matIndex = index;
        models[ModelIndex].mesh.material = models[ModelIndex].materials[models[ModelIndex].matIndex];
    }


    private float[] glosses = { 0.1f, 0.4f, 0.6f, 0.9f };
    private static readonly int GlossMapScale = Shader.PropertyToID("_GlossMapScale");

    public void ChangeMatMetallic(int index)
    {
        models[ModelIndex].metallicIndex = index;
        var gloss = glosses[index];
        models[ModelIndex].mesh.material.SetFloat(GlossMapScale, gloss);
    }


    public void ChangeMatTransparent(int index)
    {
        models[ModelIndex].transparentIndex = index;
        var transparent = index == 1;
        var mat = models[ModelIndex].mesh.material;
        if (transparent)
        {
            // 设置为透明渲染模式
            mat.SetFloat("_Mode", 2); // 2 对应 Transparent 渲染模式
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        else
        {
// 设置为不透明渲染模式
            mat.SetFloat("_Mode", 0); // 0 对应 Opaque 渲染模式
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1; // 恢复默认的渲染队列
        }
    }

    public void ChangeModel(int index)
    {
        ModelIndex = index;
        for (int i = 0; i < models.Length; i++)
        {
            models[i].gameObject.SetActive(i == index);
        }

        UIController.Instance.RefreshModel(models[ModelIndex]);
    }
}