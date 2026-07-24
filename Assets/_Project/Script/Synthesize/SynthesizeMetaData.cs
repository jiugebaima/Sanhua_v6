using UnityEngine;
// 合成元数据
[System.Serializable] 
[CreateAssetMenu(fileName = "DebugModeConfig", menuName = "DataObj/SynthesizeMetaData")]
public class SynthesizeMetaData : ScriptableObject
{
    public CardTag tag;
    public SynthesisMatchMode matchMode;
    public int requir_value;//基准参数Exact与Scale用
    public int scale_num;//倍数
    public bool needDelete;
}

public enum SynthesisMatchMode
{
    EXACT,//精确
    GT,//大于
    SCALE,//倍数
}
