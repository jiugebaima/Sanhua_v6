using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Core
{
    /// <summary>
    /// 全局回合计时任务调度器
    /// 负责管理所有延迟 N 回合执行的回调任务
    /// </summary>
    public class TurnScheduler
    {
        // 内部任务结构
        private class ScheduledTask
        {
            public Action Callback;
            public int RemainingTurns;
            public string TaskId; // 用于取消
            public bool IsExecuted; // 防止重复执行
        }

        private List<ScheduledTask> _tasks = new List<ScheduledTask>();
        private int _nextTaskId = 0;

        /// <summary>
        /// 每回合调用一次（由 TurnManager.OnTurnExecuted 触发）
        /// </summary>
        public void Tick()
        {
            // 从后向前遍历，以便在迭代中删除已执行任务
            for (int i = _tasks.Count - 1; i >= 0; i--)
            {
                var task = _tasks[i];
                if (task.IsExecuted)
                {
                    _tasks.RemoveAt(i);
                    continue;
                }

                task.RemainingTurns--;
                if (task.RemainingTurns <= 0)
                {
                    // 执行回调
                    try
                    {
                        task.Callback?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[TurnScheduler] 任务执行异常: {e}");
                    }

                    task.IsExecuted = true;
                    _tasks.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 安排一个任务，在 delayTurns 个回合后执行
        /// </summary>
        /// <param name="callback">要执行的回调</param>
        /// <param name="delayTurns">延迟回合数（必须 >= 1）</param>
        /// <returns>任务ID，可用于取消</returns>
        public string Schedule(Action callback, int delayTurns)
        {
            if (callback == null)   
            {
                Debug.LogWarning("[TurnScheduler] 拒绝安排空回调");
                return null;
            }
            if (delayTurns < 1)
            {
                Debug.LogWarning("[TurnScheduler] 延迟回合数必须 >= 1，自动设为1");
                delayTurns = 1;
            }

            string taskId = $"Task_{_nextTaskId++}_{DateTime.Now.Ticks}";
            _tasks.Add(new ScheduledTask
            {
                Callback = callback,
                RemainingTurns = delayTurns,
                TaskId = taskId
            });

            Debug.Log($"[TurnScheduler] 安排任务 {taskId}，{delayTurns} 回合后执行");
            return taskId;
        }

        /// <summary>
        /// 取消一个尚未执行的任务
        /// </summary>
        /// <returns>是否成功取消</returns>
        public bool CancelTask(string taskId)
        {
            if (string.IsNullOrEmpty(taskId)) return false;

            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].TaskId == taskId && !_tasks[i].IsExecuted)
                {
                    _tasks.RemoveAt(i);
                    Debug.Log($"[TurnScheduler] 任务 {taskId} 已取消");
                    return true;
                }
            }
            Debug.LogWarning($"[TurnScheduler] 未找到任务 {taskId} 或任务已执行");
            return false;
        }

        /// <summary>
        /// 获取当前待执行任务数量（用于调试或UI显示）
        /// </summary>
        public int PendingTaskCount => _tasks.Count;
    }


    // 在某个业务方法中（例如 StackController 合成后）
    // 调用
    // void OnMergeComplete()
    // {
    //     // 3回合后执行生成新卡操作
    //     GameRoot.Instance.Scheduler.Schedule(() => {
    //         // 实际的生成逻辑，可能调用对象池
    //         var newCard = CardPool.Instance.GetCard();
    //         newCard.transform.position = GetRandomPosition();
    //         // 注册到上下文等
    //         Debug.Log("3回合后，新卡已生成！");
    //     }, 3);
    // }

    //     string taskId = GameRoot.Instance.Scheduler.Schedule(() => { ... }, 5);
    // // 如果条件变化，取消任务
    // GameRoot.Instance.Scheduler.CancelTask(taskId);
}