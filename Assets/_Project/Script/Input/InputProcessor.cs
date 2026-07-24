using UnityEngine;

/// <summary>
/// 输入处理器 - 仅负责检测输入并转发给 CardDragHandler
/// </summary>
public class InputProcessor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _cardLayerMask; // 在Inspector里设置为 "Card" 层

    [Header("Debug")]
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private bool _mouseDebugMode = false;



    private CardDragHandler _dragHandler;






    private void Awake()
    {
        _dragHandler = new CardDragHandler(_cardLayerMask, _debugMode, _mouseDebugMode);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _dragHandler.OnMouseDown();

        if (Input.GetMouseButton(0))
            _dragHandler.OnMouseDrag();

        if (Input.GetMouseButtonUp(0))
            _dragHandler.OnMouseUp();
    }
}