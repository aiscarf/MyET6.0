using System;
using System.Collections.Generic;

namespace ET
{
    public class UIMediatorManagerComponentAwakeSystem : AwakeSystem<UIMediatorManager>
    {
        public override void Awake(UIMediatorManager self)
        {
            UIMediatorManager.Instance = self;

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
    }
}