using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	[ObjectSystem]
	public class UIAwakeSystem : AwakeSystem<UI, string, GameObject>
	{
		public override void Awake(UI self, string name, GameObject gameObject)
		{
			self.Awake(name, gameObject);
		}
	}
	
	public sealed class UI: Entity
	{
		public GameObject GameObject;

		public int ZoneSceneId { get; set; }
		public string Name { get; private set; }
		
		public bool IsActived { get; set; }
		public bool IsCovered { get; set; }
		
		/// <summary>
		/// TODO 如何管理子UI.
		/// </summary>
		public Dictionary<string, UI> m_children = new Dictionary<string, UI>();
		
		public void Awake(string name, GameObject gameObject)
		{
			this.m_children.Clear();
			this.Name = name;
			this.GameObject = gameObject;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (UI ui in this.m_children.Values)
			{
				ui.Dispose();
			}
			
			UnityEngine.Object.Destroy(this.GameObject);
			this.m_children.Clear();
		}

		// public void SetAsFirstSibling()
		// {
		// 	this.GameObject.transform.SetAsFirstSibling();
		// }

		public void Add(UI ui)
		{
			this.m_children.Add(ui.Name, ui);
		}

		public void Remove(string name)
		{
			UI ui;
			if (!this.m_children.TryGetValue(name, out ui))
			{
				return;
			}
			this.m_children.Remove(name);
			ui.Dispose();
		}

		public UI Get(string name)
		{
			UI child;
			if (this.m_children.TryGetValue(name, out child))
			{
				return child;
			}
			GameObject childGameObject = this.GameObject.transform.Find(name)?.gameObject;
			if (childGameObject == null)
			{
				return null;
			}
			child = this.AddChild<UI, string, GameObject>(name, childGameObject);
			this.Add(child);
			return child;
		}
	}
}