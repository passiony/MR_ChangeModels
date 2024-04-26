using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;


[Serializable]
public class LightPose
{
    public Light light;
    public int poseIndex;
}

public class LightController : MonoBehaviour
{
    public static LightController Instance;
    public LightPose[] lights;
    public Transform[] pointPoses;
    public Transform[] spotPoses;
    public float[] directionAngles;

    public int LightIndex{ get; set; }
    
    void Awake()
    {
        Instance = this;
    }

    public void ChangeLight(int index)
    {
        LightIndex = index;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].light.gameObject.SetActive(i == index);
        }    
    }
    
    public void ChangeLightPos(int index)
    {
        switch (lights[LightIndex].light.type)
        {
            case LightType.Spot:
                var stopPoint = spotPoses[index];
                lights[LightIndex].poseIndex = index;
                lights[LightIndex].light.gameObject.transform.SetWorldPose(stopPoint.GetWorldPose());
                break;
            case LightType.Directional:
                var direction = directionAngles[index];
                var rotation = lights[LightIndex].light.gameObject.transform.eulerAngles;
                rotation.y = direction;
                lights[LightIndex].light.gameObject.transform.rotation = Quaternion.Euler(rotation);
                break;
            case LightType.Point:
                var pointPose = pointPoses[index];
                lights[LightIndex].poseIndex = index;
                lights[LightIndex].light.gameObject.transform.SetWorldPose(pointPose.GetWorldPose());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void ChangeShadowStrength(float strength)
    {
        lights[LightIndex].light.shadowStrength = strength;
    }
    
    public void ChangeShadowOn(bool isOn)
    {
        lights[LightIndex].light.shadows = isOn ? LightShadows.Soft : LightShadows.None;
    }
}
