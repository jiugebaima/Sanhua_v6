using UnityEngine;

[CreateAssetMenu(fileName = "DebugModeConfig", menuName = "DataObj/DebugModeConfig")]
public class DebugModeConfig : ScriptableObject
{
    public bool MergeDebug = false;
    public bool DropDetector = false;
    public bool DragableDebug = false;
    public bool StackControllerDebug = false;
}