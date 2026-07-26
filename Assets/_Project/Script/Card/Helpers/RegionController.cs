using Unity.VisualScripting;
using UnityEngine;

public class RegionController
{
    private RegionComponent _currentRegion;
    private RegionDetector _detector;
    private CardComponent _selfCardComponent;
    private bool _debugMode;

    public RegionController(CardComponent cardComponent,RegionComponent region)
    {
        _selfCardComponent = cardComponent;
        _currentRegion = region;
        _detector = new RegionDetector();
    }

    /// <summary>
    /// 执行一次完整的检测和状态处理（通常在鼠标点击时调用）
    /// </summary>
    public void DetectRegion()
    {
        Debug.Log("startDetect");
        RegionComponent detectedRegion = _detector.Detect();

        if(detectedRegion != null)
        {
            Debug.Log(detectedRegion.regionName);
        }
        
        if(detectedRegion != null)
        {
            if(_currentRegion == detectedRegion)
            {
                return;//注意此处有return
            }
            else if(_currentRegion != null &&_currentRegion != detectedRegion)
            {
                SwitchRegion(detectedRegion);
            }
            else
            {
                AssignRegion(detectedRegion);
            }
        }
        else
        {
            if(_currentRegion != null)
            {
                ReturnFromRegion();
            }
        }
    }

    // 以下四种方法对应四种状态，可独立调用

    /// <summary>
    /// 赋值区域（进入新区域）
    /// </summary>
    private void AssignRegion(RegionComponent newRegion)
    {
        _currentRegion = newRegion;
        if (_debugMode) Debug.Log($"[RegionController] 进入区域: {newRegion.name}");
    }

    /// <summary>
    /// 返回（退出当前区域）
    /// </summary>
    private void ReturnFromRegion()
    {
        if (_debugMode) Debug.Log("[RegionController] 返回");
        _currentRegion = null;
    }
    /// <summary>
    /// 切换到新区域
    /// </summary>
    private void SwitchRegion(RegionComponent newRegion)
    {
        if (_debugMode) Debug.Log($"[RegionController] sr = rr (切换到 {newRegion.name})");
        _selfCardComponent.currentRegion = newRegion;
    }

    /// <summary>
    /// 获取当前选中的RegionComponent
    /// </summary>
    public RegionComponent CurrentRegion => _currentRegion;
}

/// <summary>
/// Region探测器 - 只检测Region层上的RegionComponent
/// </summary>
public class RegionDetector
{
    private LayerMask _regionLayerMask;

    public RegionDetector()
    {
        _regionLayerMask = LayerMask.GetMask("Region");
    }

    /// <summary>
    /// 检测鼠标下方是否存在RegionComponent
    /// </summary>
    public RegionComponent Detect()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, _regionLayerMask);
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<RegionComponent>();
        }
        return null;
    }
}
