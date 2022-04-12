using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scarf.Moba
{
    public enum CoroutineState
    {
        Dead = 0,
        Ready,
        Running,
        Suspend,
    }

    public sealed class CCoroutineManager
    {
        private Dictionary<int, CCoroutine> m_dicCoroutineMap = new Dictionary<int, CCoroutine>();
        private List<CCoroutine> m_lstAddCoroutines = new List<CCoroutine>();
        private List<CCoroutine> m_lstCoroutines = new List<CCoroutine>();

        private static CCoroutineManager m_inst;
        public static CCoroutineManager instance => m_inst ?? (m_inst = new CCoroutineManager());

        private static int CoroutineId = 0;

        #region 对象池

        private int initSize;
        private Queue<CCoroutine> m_lstCoroutinePool = new Queue<CCoroutine>();

        private void InitPool(int size)
        {
            this.initSize = size;

            while (m_lstCoroutinePool.Count < size)
            {
                Recycle(new CCoroutine());
            }
        }

        private CCoroutine Get()
        {
            CCoroutine cCoroutine = null;

            if (this.m_lstCoroutinePool.Count > 0)
            {
                cCoroutine = this.m_lstCoroutinePool.Dequeue();
            }
            else
            {
                cCoroutine = new CCoroutine();
            }

            return cCoroutine;
        }

        private void Recycle(CCoroutine coroutine)
        {
            coroutine.Reset();
            if (this.m_lstCoroutinePool.Count >= this.initSize)
                return;
            this.m_lstCoroutinePool.Enqueue(coroutine);
        }

        #endregion

        public void Init()
        {
            CCoroutineManager.CoroutineId = 0;
            this.m_dicCoroutineMap.Clear();
            this.m_lstAddCoroutines.Clear();
            this.m_lstCoroutines.Clear();

            // 初始化对象池
            InitPool(50);
        }

        public void Clear()
        {
            CCoroutineManager.CoroutineId = 0;

            this.m_dicCoroutineMap.Clear();
            this.m_lstAddCoroutines.Clear();
            this.m_lstCoroutines.Clear();
            this.m_lstCoroutinePool.Clear();
        }

        public int StartCoroutine(IEnumerator enumerator)
        {
            ++CCoroutineManager.CoroutineId;

            var coroutine = this.Get();
            coroutine.Init(CCoroutineManager.CoroutineId, enumerator);
            coroutine.Tick();

            this.m_dicCoroutineMap.Add(CCoroutineManager.CoroutineId, coroutine);
            this.m_lstAddCoroutines.Add(coroutine);

            return CCoroutineManager.CoroutineId;
        }

        public void StopCoroutine(int eId)
        {
            if (!this.m_dicCoroutineMap.TryGetValue(eId, out var cCoroutine))
            {
                Debug.LogWarning($"CCoroutineManager.StopCoroutine Failed, not existent CoroutineId=[{eId}]");
                return;
            }

            cCoroutine.Stop();
        }

        public void SuspendCoroutine(int eId)
        {
            if (!this.m_dicCoroutineMap.TryGetValue(eId, out var cCoroutine))
            {
                Debug.LogWarning($"CCoroutineManager.SuspendCoroutine Failed, not existent CoroutineId=[{eId}]");
                return;
            }

            cCoroutine.Suspend();
        }

        public void ResumeCoroutine(int eId)
        {
            if (!this.m_dicCoroutineMap.TryGetValue(eId, out var cCoroutine))
            {
                Debug.LogWarning($"CCoroutineManager.ResumeCoroutine Failed, not existent CoroutineId=[{eId}]");
                return;
            }

            cCoroutine.Resume();
        }

        public CoroutineState StatusCoroutine(int eId)
        {
            if (!this.m_dicCoroutineMap.TryGetValue(eId, out var cCoroutine))
            {
                Debug.LogWarning($"CCoroutineManager.StatusCoroutine Failed, not existent CoroutineId=[{eId}]");
                return CoroutineState.Dead;
            }

            return cCoroutine.GetState();
        }

        public void Tick()
        {
            foreach (CCoroutine cCoroutine in this.m_lstAddCoroutines)
            {
                this.m_lstCoroutines.Add(cCoroutine);
            }

            this.m_lstAddCoroutines.Clear();

            foreach (CCoroutine cCoroutine in this.m_lstCoroutines)
            {
                var state = cCoroutine.GetState();
                if (state == CoroutineState.Dead || state == CoroutineState.Suspend)
                    continue;
                cCoroutine.Tick();
            }

            for (int i = this.m_lstCoroutines.Count - 1; i >= 0; i--)
            {
                var coroutine = this.m_lstCoroutines[i];
                if (coroutine.GetState() == CoroutineState.Dead)
                {
                    this.m_dicCoroutineMap.Remove(coroutine.Id);
                    this.m_lstCoroutines.RemoveAt(i);
                    this.Recycle(coroutine);
                }
            }
        }

        private class CCoroutine
        {
            private Stack<IEnumerator> m_stack = new Stack<IEnumerator>();
            private CoroutineState m_eState = CoroutineState.Ready;

            private IEnumerator m_Curr = null;

            public int Id { get; private set; }

            public void Init(int id, IEnumerator root)
            {
                this.Id = id;
                this.m_stack.Clear();
                this.m_stack.Push(root);
                this.m_Curr = root;
                this.m_eState = CoroutineState.Ready;
            }

            public void Tick()
            {
                if (this.m_eState == CoroutineState.Dead)
                    return;
                if (this.m_eState == CoroutineState.Suspend)
                    return;

                this.m_eState = CoroutineState.Running;

                // 递归遍历
                RecursIteration();

                if (this.m_stack.Count > 0)
                    return;
                this.m_eState = CoroutineState.Dead;
            }

            public CoroutineState GetState()
            {
                return this.m_eState;
            }

            private void RecursIteration()
            {
                while (this.m_stack.Count > 0)
                {
                    this.m_Curr = this.m_stack.Peek();

                    if (this.m_Curr.Current is LogicYieldInstruction yieldInstruction)
                    {
                        if (yieldInstruction.MoveNext())
                        {
                            break;
                        }
                    }
                    else if (this.m_Curr.Current is IEnumerator childIe)
                    {
                        this.m_stack.Push(childIe);
                        continue;
                    }

                    // 子完成, 父步进
                    while (!this.m_Curr.MoveNext())
                    {
                        this.m_stack.Pop();

                        if (this.m_stack.Count <= 0)
                        {
                            break;
                        }

                        this.m_Curr = this.m_stack.Peek();
                    }
                }
            }

            public void Stop()
            {
                this.m_eState = CoroutineState.Dead;
            }

            public void Suspend()
            {
                if (this.m_eState == CoroutineState.Dead)
                    return;
                this.m_eState = CoroutineState.Suspend;
            }

            public void Resume()
            {
                if (this.m_eState != CoroutineState.Suspend)
                    return;
                this.m_eState = CoroutineState.Running;
            }

            public void Reset()
            {
                this.m_stack.Clear();
                this.m_eState = CoroutineState.Dead;
                this.m_Curr = null;
                this.Id = 0;
            }
        }
    }
}