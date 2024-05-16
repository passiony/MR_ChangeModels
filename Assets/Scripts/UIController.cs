using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Slider lightTypeSlider;
    public Slider LightPosSlider;
    public Slider LightPowerSlider;

    public Slider ShadowOnSlider;
    public Slider ShadowStrengthSlider;

    public Slider MaterialSlider;
    public Slider MatMetallicSlider;
    public Slider MatTransparentSlider;

    public Slider ModelSlider;

    void Awake()
    {
        Instance = this;
        lightTypeSlider.onValueChanged.AddListener(OnLightTypeChange);
        LightPosSlider.onValueChanged.AddListener(OnLightPosChange);
        LightPowerSlider.onValueChanged.AddListener(OnLightPowerChange);
        ShadowOnSlider.onValueChanged.AddListener(OnShadowOnChange);
        ShadowStrengthSlider.onValueChanged.AddListener(OnShadowStrengthChange);
        MaterialSlider.onValueChanged.AddListener(OnMaterialChange);
        MatMetallicSlider.onValueChanged.AddListener(OnMatMetallicChange);
        MatTransparentSlider.onValueChanged.AddListener(OnMatTransparentChange);
        ModelSlider.onValueChanged.AddListener(OnModelChange);
    }

    public void RefreshModel(Model model)
    {
        MatMetallicSlider.value = model.metallicIndex;
        MatTransparentSlider.value = model.transparentIndex;
        MaterialSlider.value = model.matIndex;
    }

    private void OnLightTypeChange(float arg0)
    {
        LightController.Instance.ChangeLight((int)arg0);
    }

    private void OnLightPosChange(float arg0)
    {
        LightController.Instance.ChangeLightPos((int)arg0);
        ModelController.Instance.ChangeShadowDirection((int)arg0);
    }

    private void OnLightPowerChange(float arg0)
    {
        LightController.Instance.ChangeLightPower((int)arg0);
    }

    float[] strength = { 0.2f, 0.4f, 0.6f, 0.8f, 1 };
    private void OnShadowOnChange(float arg0)
    {
        var index = (int)arg0;
        ModelController.Instance.ChangeShadowOn(index == 0);
    }

    private void OnShadowStrengthChange(float arg0)
    {
        var index = (int)arg0;
        ModelController.Instance.ChangeShadowStrength(strength[index]);
    }


    private void OnMaterialChange(float arg0)
    {
        ModelController.Instance.ChangeMat((int)arg0);
    }

    private void OnMatMetallicChange(float arg0)
    {
        ModelController.Instance.ChangeMatMetallic((int)arg0);
    }

    private void OnMatTransparentChange(float arg0)
    {
        ModelController.Instance.ChangeMatTransparent((int)arg0);
    }

    private void OnModelChange(float arg0)
    {
        ModelController.Instance.ChangeModel((int)arg0);
    }
}