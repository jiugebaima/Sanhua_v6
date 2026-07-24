using UnityEngine;
using CardGame.Core;

public class GameRoot : MonoBehaviour
{
    [Header("合成配置")]
    [SerializeField] private GameObject _cardPrefab;  // 卡牌预制体
    // private MergeExecutor _mergeExecutor;
    private ICardFactory _cardFactory;
    private SynthesisExecuteFactory _synthesisExecuteFactory;
    // public MergeExecutor mergeExecutor => _mergeExecutor;
    public ICardFactory cardFactory => _cardFactory;



    public DebugModeConfig debugModeConfig;



    private static GameRoot _instance;
    public static GameRoot Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameRoot>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("[GameRoot]");
                    _instance = go.AddComponent<GameRoot>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    // 子模块（公开以便外部访问）
    public TurnManager TurnManager { get; private set; }
    public GameContext Context { get; private set; }
    // public TurnScheduler Scheduler { get; private set; } // 【新增】

    private void Awake()
    {
        // ... 单例初始化 ...
        TurnManager = new TurnManager();
        // Scheduler = new TurnScheduler();

        // 回合结束时，触发调度器的 Tick（处理倒计时任务）
        // TurnManager.OnTurnEnded += Scheduler.Tick;

        // FormulaMatcher matcher = new FormulaMatcher();
        // _mergeExecutor = new MergeExecutor(matcher);
        _cardFactory = new DefaultCardFactory(_cardPrefab);
        _synthesisExecuteFactory = new SynthesisExecuteFactory();
    }

    private void OnDestroy()
    {
        // 取消订阅，防止内存泄漏
        if (TurnManager != null)
        {
            // TurnManager.OnTurnEnded -= Scheduler.Tick;
        }
    }

    public void InitializeGame()
    {
        // 设置默认的回合操作（示例：生成随机卡牌）
        // ITurnAction defaultAction = new SpawnCardAction(Context);
        // TurnManager.SetCurrentAction(defaultAction);

        // Debug.Log("[GameRoot] 游戏初始化完成");
    }

    // ---------- 堆叠/卡牌注册（省略，与之前相同）----------
    // ... RegisterStack, UnregisterStack, RegisterCard, UnregisterCard ...
}
