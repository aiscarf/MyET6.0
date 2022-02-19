using System;
using System.Collections.Generic;

namespace ET
{
    public class UIMediatorManagerComponentAwakeSystem : AwakeSystem<UIMediatorManager>
    {
        public override void Awake(UIMediatorManager self)
        {
            UIMediatorManager.Instance = self;
            self.Awake();
        }
    }
    
    public class UIMediatorManagerComponentDestroySystem : DestroySystem<UIMediatorManager>
    {
        public override void Destroy(UIMediatorManager self)
        {
            self.AllMediator.Clear();
            self.AllMediator = null;

            UIMediatorManager.Instance = null;
        }
    }

    public static class UIMediatorManagerComponentSystem
    {
        public static void Init(this UIMediatorManager self, UI ui)
        {
            try
            {
                var mediator = self.AllMediator[ui.Name];
                var type = mediator.GetGenericType();
                mediator.ViewUI = ui;
                mediator.referenceCollector = ui.GameObject.GetComponent<ReferenceCollector>();
                mediator.Bind(ui.GetComponent(type));
                mediator.OnInit();
            }
            catch (Exception e)
            {
                throw new Exception($"UIMediatorManagerComponentSystem.Init: {ui.Name}Mediator error", e);
            }
        }

        public static void Destroy(this UIMediatorManager self, string uiType)
        {
            try
            {
                self.AllMediator[uiType].OnDestroy();
            }
            catch (Exception e)
            {
                throw new Exception($"UIMediatorManagerComponentSystem.Destroy: {uiType}Mediator error", e);
            }
        }

        public static void Open(this UIMediatorManager self, string uiType, object data)
        {
            try
            {
                self.AllMediator[uiType].OnOpen(data);
            }
            catch (Exception e)
            {
                throw new Exception($"UIMediatorManagerComponentSystem.Open: {uiType}Mediator error", e);
            }
        }

        public static void Close(this UIMediatorManager self, string uiType)
        {
            try
            {
                self.AllMediator[uiType].OnClose();
            }
            catch (Exception e)
            {
                throw new Exception($"UIMediatorManagerComponentSystem.Open: {uiType}Mediator error", e);
            }
        }

        public static void BeCover(this UIMediatorManager self, string uiType)
        {
            try
            {
                self.AllMediator[uiType].OnBeCover();
            }
            catch (Exception e)
            {
                throw new Exception($"UIMediatorManagerComponentSystem.BeCover: {uiType}Mediator error", e);
            }
        }

        public static void UnCover(this UIMediatorManager self, string uiType)
        {
            try
            {
                self.AllMediator[uiType].OnUnCover();
            }
            catch (Exception e)
            {
                throw new Exception($"UIMediatorManagerComponentSystem.UnCover: {uiType}Mediator error", e);
            }
        }

        public static void Awake(this UIMediatorManager self)
        {
            self.AllMediator = new Dictionary<string, IMediator>();

            var mediatorAttributes = Game.EventSystem.GetTypes(typeof(MediatorTagAttribute));
            foreach (Type type in mediatorAttributes)
            {
                var uiMediator = Activator.CreateInstance(type) as IMediator;
                if (uiMediator == null)
                {
                    continue;
                }

                var uiType = uiMediator.GetGenericType();
                var uiAttrs = uiType.GetCustomAttributes(typeof(UITagAttribute), false);
                if (uiAttrs.Length <= 0)
                {
                    continue;
                }

                var uiTag = uiAttrs[0] as UITagAttribute;
                if (uiTag == null)
                {
                    continue;
                }

                self.AllMediator.Add(uiTag.Name, uiMediator);
            }
        }

        public static void Reload(this UIMediatorManager self)
        {
            self.ClearAllMediator();
            self.Awake();
            self.RevertAllMediator();
            Log.Info("UIMediatorManager Reload Success!");
        }

        private static void ClearAllMediator(this UIMediatorManager self)
        {
            if (self.AllMediator == null)
                return;
            
            // DONE: 清除所有的IMediator.
            foreach (var kv in UIManager.Instance.m_allUiMap)
            {
                var ui = kv.Value;
                if (ui.IsActived)
                {
                    self.Close(ui.Name);
                }

                if (ui.IsCovered)
                {
                    self.UnCover(ui.Name);
                }

                self.Destroy(ui.Name);
            }
            
            self.AllMediator.Clear();
        }

        private static void RevertAllMediator(this UIMediatorManager self)
        {
            foreach (var kv in UIManager.Instance.m_allUiMap)
            {
                var ui = kv.Value;
                self.Init(ui);
            }
            
            foreach (var uiCell in UIManager.Instance.m_uiStack)
            {
                foreach (var coverUi in uiCell.m_coverList)
                {
                    self.BeCover(coverUi.Name);
                }

                // TODO 数据要怎么进行获取?
                self.Open(uiCell.m_ui.Name, null);
            }
        }
    }
}