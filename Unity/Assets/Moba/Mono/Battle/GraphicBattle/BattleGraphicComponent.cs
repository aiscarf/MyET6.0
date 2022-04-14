using System.Collections.Generic;
using ET;
using UnityEngine;

namespace Scarf.Moba
{
    public class BattleGraphicComponent: GraphicComponent
    {
        public GameObject SceneRoot { get; private set; }
        public GameObject MapRoot { get; private set; }
        public GameObject UnitRoot { get; private set; }
        public ChaseCamera ChaseCamera { get; private set; }
        public MobaLauncher MobaLauncher { get; private set; }

        #region UnityGraphic

        private List<GraphicComponent> m_lstAllGraphicComponent = new List<GraphicComponent>();

        public void AddGraphic(GraphicComponent graphicComponent)
        {
            if (graphicComponent == this)
                return;
            m_lstAllGraphicComponent.Add(graphicComponent);
        }

        public void RemoveGraphic(GraphicComponent graphicComponent)
        {
            if (graphicComponent == this)
                return;
            m_lstAllGraphicComponent.Remove(graphicComponent);
        }

        #endregion

        public long Uid { get; set; }

        public void BindGraphic(GameObject[] rootArr)
        {
            for (int i = 0; i < rootArr.Length; i++)
            {
                var root = rootArr[i];
                if (root.name == "SceneRoot")
                {
                    this.SceneRoot = root;
                }
                else if (root.name == "MapRoot")
                {
                    this.MapRoot = root;
                }
                else if (root.name == "UnitRoot")
                {
                    this.UnitRoot = root;
                }
                else if (root.name == "ChaseCamera")
                {
                    this.ChaseCamera = root.GetComponent<ChaseCamera>();
                    this.ChaseCamera.Init();
                }
                else if (root.name == "MobaLauncher")
                {
                    MobaLauncher = root.GetComponent<MobaLauncher>();
                }
            }
        }

        protected override void OnInit()
        {
            // TODO 遍历所有GraphicComponent.
        }

        protected override void OnStart()
        {
            this.MobaLauncher.UnityUpdate += this.UnityUpdate;
            this.MobaLauncher.UnityApplicationFocus += this.UnityApplicationFocus;
        }

        protected override void OnUnityUpdate()
        {
            for (int i = 0; i < this.m_lstAllGraphicComponent.Count; i++)
            {
                this.m_lstAllGraphicComponent[i].UnityUpdate();
            }
        }

        void UnityApplicationFocus(bool hasFocus)
        {
        }
    }
}