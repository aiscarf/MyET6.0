using UnityEngine;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class ChangedContainer : CComponent
    {
        protected List<int> m_lstKey;
        protected List<int> m_lstValue;
        protected List<int> m_lstTime;
        protected List<IRelateChanged> m_lstRelate;

        public ChangedContainer()
        {
            this.m_lstKey = new List<int>();
            this.m_lstValue = new List<int>();
            this.m_lstTime = new List<int>();
            this.m_lstRelate = new List<IRelateChanged>();
        }

        public ChangedContainer.ChangedHandler OnValueChanged;

        public List<int> GetAllKey()
        {
            return this.m_lstKey;
        }

        public bool ContainsKey(int key)
        {
            return this.m_lstKey.IndexOf(key) >= 0;
        }

        public void Add(int key, IRelateChanged relateChanged = null)
        {
            if (this.m_lstKey.IndexOf(key) == -1)
            {
                this.m_lstKey.Add(key);
                this.m_lstValue.Add(0);
                this.m_lstTime.Add(this.GetTime());
                this.m_lstRelate.Add(relateChanged);
            }
            else
                Debug.Log("[" + this.GetType().ToString() + "]key=" + (object)key + "已存在，不能重复添加");
        }

        public virtual void SetValue(int key, int value)
        {
            int index = this.m_lstKey.IndexOf(key);
            if (index > -1)
            {
                int oldValue = this.m_lstValue[index];
                if (oldValue == value)
                    return;
                this.m_lstValue[index] = value;
                this.m_lstTime[index] = this.GetTime();
                if (this.OnValueChanged != null)
                    this.OnValueChanged(key, oldValue, value);
                this.m_lstRelate[index]?.Handler();
            }
            else
                Debug.Log("[" + this.GetType().ToString() + "]key=" + (object)key + "不存在");
        }

        public virtual int GetValue(int key)
        {
            int index = this.m_lstKey.IndexOf(key);
            if (index > -1)
                return this.m_lstValue[index];
            Debug.Log("[" + this.GetType().ToString() + "]key=" + (object)key + "不存在");
            return 0;
        }

        public virtual int GetLastTime(int key)
        {
            int index = this.m_lstKey.IndexOf(key);
            if (index > -1)
                return this.m_lstTime[index];
            Debug.Log("[" + this.GetType().ToString() + "]key=" + (object)key + "不存在");
            return 0;
        }

        public virtual void Reset()
        {
            for (int index = 0; index < this.m_lstValue.Count; ++index)
            {
                this.m_lstValue[index] = 0;
                this.m_lstTime[index] = this.GetTime();
                if (this.m_lstRelate[index] != null)
                    this.m_lstRelate[index].Reset();
            }
        }

        public virtual void Clear()
        {
            this.m_lstKey.Clear();
            this.m_lstValue.Clear();
            this.m_lstTime.Clear();
            for (int index = 0; index < this.m_lstRelate.Count; ++index)
            {
                if (this.m_lstRelate[index] != null)
                    this.m_lstRelate[index].Clear();
            }

            this.m_lstRelate.Clear();
            this.OnValueChanged = (ChangedContainer.ChangedHandler)null;
        }

        protected virtual int GetTime()
        {
            return 0;
        }

        public delegate void ChangedHandler(int key, int oldValue, int newValue);
    }
}