using UnityEngine;

/// <summary>
/// 输入处理器 - 检测输入并转发给 CardDragHandler，并处理相机移动
/// </summary>
public class InputProcessor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _cardLayerMask;
    [SerializeField] private float _cameraMoveSpeed = 5f;       // WASD 移动速度
    [SerializeField] private float _cameraZoomSpeed = 10f;      // 滚轮缩放速度（Z轴移动）
    [SerializeField] private float _minZoomSize = 3f;  // 最小尺寸（最放大）
    [SerializeField] private float _maxZoomSize = 15f; // 最大尺寸（最缩小）

    [Header("Debug")]
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private bool _mouseDebugMode = false;

    private CardDragHandler _dragHandler;
    private Camera _mainCamera;

    private void Awake()
    {
        _dragHandler = new CardDragHandler(_cardLayerMask, _debugMode, _mouseDebugMode);
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // 原有拖拽输入
        if (Input.GetMouseButtonDown(0))
            _dragHandler.OnMouseDown();
        if (Input.GetMouseButton(0))
            _dragHandler.OnMouseDrag();
        if (Input.GetMouseButtonUp(0))
            _dragHandler.OnMouseUp();

        // 相机移动 WASD
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D
        float vertical = Input.GetAxisRaw("Vertical");     // W/S
        Vector3 move = new Vector3(horizontal, vertical, 0) * _cameraMoveSpeed * Time.deltaTime;
        _mainCamera.transform.Translate(move, Space.World);

        // 鼠标滚轮缩放（改变 Camera.orthographicSize）
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && _mainCamera.orthographic)
        {
            float newSize = _mainCamera.orthographicSize - scroll * _cameraZoomSpeed;
            newSize = Mathf.Clamp(newSize, _minZoomSize, _maxZoomSize);
            _mainCamera.orthographicSize = newSize;
        }
    }
}