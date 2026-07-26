using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RegionComponent : MonoBehaviour
{
    [SerializeField] private int _regionID;
    [SerializeField] private string _regionName;

    //地区状态
    public float UnrestLevel;   //混乱度
    public float RenownLevel;   //
    public float HonorLevel;//
    public float CohesionLevel;//
    public float IntelLevel;//
    public float HumanEpochProgress;//

    public int regionId => _regionID;
    public string regionName => _regionName;

    public void me(RegionValueType rvt, float value)
    {
        switch (rvt)
        {
            case RegionValueType.Unrest:
                UnrestLevel += value;
                break;
            case RegionValueType.Renown:
                RenownLevel += value;
                break;
            case RegionValueType.Honor:
                HonorLevel += value;
                break;
            case RegionValueType.Cohesion:
                CohesionLevel += value;
                break;
            case RegionValueType.Intel:
                IntelLevel += value;
                break;
            case RegionValueType.HumanEpoch:
                HumanEpochProgress += value;
                break;
            default:
                Debug.LogWarning($"[RegionManager] 未处理的 RegionValueType: {rvt}");
                break;
        }
    }


}

public enum RegionValueType
{
    Unrest,
    Renown,
    Honor,
    Cohesion,
    Intel,
    HumanEpoch,
}
