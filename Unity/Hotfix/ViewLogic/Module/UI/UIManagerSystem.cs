using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UIManagerComponentAwakeSystem : AwakeSystem<UIManager>
    {
        public override void Awake(UIManager self)
        {
            UIManager.Instance = self;

            self.m_allUiMap = new Dictionary<string, UI>();
            self.m_allUiType = new Dictionary<string, Type>();
            self.m_openUis = new List<UI>();
            self.m_uiStack = new List<UiCell>();

            var uiTagAttributes = Game.EventSystem.GetTypes(typeof(UITagAttribute));
            foreach (Type type in uiTagAttributes)
            {
                var attrs = type.GetCustomAttributes(typeof(UITagAttribute), false);
                if (attrs.Length <= 0)
                    continue;

                var uiTagAttribute = attrs[0] as UITagAttribute;
                if (uiTagAttribute == null)
                    continue;
                self.m_allUiType.Add(uiTagAttribute.Name, type);
            }
        }
    }

    public static class UIManagerSystem
    {
        public static async ETTask<UI> CreateUI(this UIManager self, string uiType)
        {
            await ETTask.CompletedTask;
            
            // TODO 加载数据包.
            
            await ResourcesComponent.Instance.LoadBundleAsync(uiType.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(uiType.StringToAB(), uiType);
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);

            UI ui = self.AddChild<UI, string, GameObject>(uiType, go);
            Scene curScene = Game.Scene.GetComponent<ZoneSceneManagerComponent>().CurScene;
            ui.ZoneSceneId = curScene.Zone;
            ui.GameObject.transform.SetParent(GlobalComponent.Instance.UI, false);
            ui.GameObject.AddComponent<ComponentView>().Component = ui;
            ui.GameObject.AddComponent<UIDepth>();
            ui.GameObject.SetActive(false);
            var type = self.m_allUiType[uiType];
            ui.AddComponent(type);
            self.m_allUiMap.Add(uiType, ui);
            UIMediatorManager.Instance.Init(ui);
            return ui;
        }

        public static void DestroyUI(this UIManager self, string uiType)
        {
            if (!self.m_allUiMap.TryGetValue(uiType, out UI ui))
            {
                return;
            }

            // 该UI处于打开状态, 先关闭了再销毁.
            if (self.GetUIStatus(uiType))
            {
                self.CloseUI(uiType);
            }

            UIMediatorManager.Instance.Destroy(uiType);
            // 卸载AB包.
            ResourcesComponent.Instance.UnloadBundle(uiType);
            self.m_allUiMap.Remove(uiType);
            ui.Dispose();
        }

        public static UI GetUI(this UIManager self, string name)
        {
            UI ui = null;
            self.m_allUiMap.TryGetValue(name, out ui);
            return ui;
        }

        /// <summary>
        /// 打开UI, 不冻结Ui.
        /// </summary>
        public static async ETTask OpenUI(this UIManager self, string uiType, object data)
        {
            string coverUi = null;
            if (self.m_uiStack.Count > 0)
                coverUi = self.m_uiStack[self.m_uiStack.Count - 1].m_ui.Name;
            await self.OpenAndCoverUI(uiType, data, coverUi);
        }

        /// <summary>
        /// 永久打开UI, 不记录打开的流程.
        /// </summary>
        public static async ETTask OpenForeverUI(this UIManager self, string uiType, object data)
        {
            await self.OpenUIWithCoverList(uiType, data, null, true);
        }

        /// <summary>
        /// 打开UI并且冻结coverUi
        /// </summary>
        public static async ETTask OpenAndCoverUI(this UIManager self, string uiType, object data,
            string coverUi = null)
        {
            List<string> coverUis = null;
            if (coverUi != null)
            {
                coverUis = new List<string>() { coverUi };
            }

            await self.OpenUIWithCoverList(uiType, data, coverUis, false);
        }

        /// <summary>
        /// 打开UI并且冻结所有UI
        /// </summary>
        public static async ETTask OpenAndCoverAll(this UIManager self, string uiType, object data)
        {
            List<string> coverUis = new List<string>();
            for (int i = 0; i < self.m_openUis.Count; i++)
            {
                if (!self.m_openUis[i].IsCovered)
                    coverUis.Add(self.m_openUis[i].Name);
            }

            await self.OpenUIWithCoverList(uiType, data, coverUis, false);
        }

        public static async ETTask OpenUIWithCoverList(this UIManager self, string uiType, object data,
            List<string> coverUis, bool forever)
        {
            // TODO 动画放在这里统一进行实现.
            await ETTask.CompletedTask;

            var ui = self.GetUI(uiType);
            if (ui == null)
            {
                Log.Info($"要打开的ui不存在: {uiType}");
                return;
            }

            if (self.GetUIStatus(ui.Name))
            {
                Log.Info($"该ui已经打开: {ui.Name}");
                return;
            }

            UiCell uiCell = new UiCell(ui);
            if (coverUis != null)
            {
                for (int i = 0; i < coverUis.Count; i++)
                {
                    var coverUi = self.GetUI(coverUis[i]);
                    if (coverUi == null)
                    {
                        continue;
                    }

                    uiCell.AddCoverUi(coverUi);
                }
            }

            self.m_uiStack.Add(uiCell);
            uiCell.CoverUis();

            ui.IsActived = true;
            if (forever)
            {
                UIMediatorManager.Instance.Open(uiType, data);
                ui.GameObject.SetActive(true);
            }
            else
            {
                self.m_openUis.Add(ui);
                UIMediatorManager.Instance.Open(uiType, data);
                ui.GameObject.SetActive(true);

                int lastIndex = self.m_openUis.Count - 1;
                if (lastIndex < 0) lastIndex = 0;
                ui.GameObject.GetComponent<UIDepth>().CalcDepth(lastIndex * 100);
            }

        }

        public static void CloseUI(this UIManager self, string uiType)
        {
            var ui = self.GetUI(uiType);
            if (ui == null)
            {
                Log.Info($"要关闭的ui不存在: {uiType}");
                return;
            }

            if (!self.m_openUis.Contains(ui))
            {
                Log.Info($"要关闭的ui已经处于关闭状态: {uiType}");
                return;
            }

            if (ui.IsCovered)
            {
                Log.Info($"正在关闭一个被覆盖的UI: {uiType}");
            }

            ui.IsActived = false;
            ui.GameObject.SetActive(false);
            UIMediatorManager.Instance.Close(uiType);
            self.m_openUis.Remove(ui);
            var frontUi = self.FindUiCell(ui);
            if (frontUi == null)
                return;
            self.m_uiStack.Remove(frontUi);
            frontUi.UnCoverUis();
        }

        public static bool GetUIStatus(this UIManager self, string uiType)
        {
            var ui = self.GetUI(uiType);
            return self.m_openUis.Contains(ui);
        }

        private static UiCell FindUiCell(this UIManager self, UI ui)
        {
            UiCell frontUi = null;
            for (int i = self.m_uiStack.Count - 1; i >= 0; i--)
            {
                UiCell uiCell = self.m_uiStack[i];
                if (uiCell.m_ui == null)
                    continue;
                if (uiCell.m_ui != ui)
                    continue;
                frontUi = uiCell;
                break;
            }

            return frontUi;
        }
    }
}