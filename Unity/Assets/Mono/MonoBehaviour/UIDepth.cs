using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIDepth: MonoBehaviour
    {
        public int StartOrder = -1;

        private string m_parentSortingLayerName;
        private int m_parentSortingLayerID;
        private int m_childCount;
        private UILayer[] m_arrLayer;
        private Canvas m_canvas;

        private void Awake()
        {
            var canvas = this.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = this.gameObject.AddComponent<Canvas>();
            }

            canvas.overrideSorting = true;
            canvas.sortingLayerName = "Default";
            canvas.sortingLayerID = LayerMask.NameToLayer("Default");
            canvas.additionalShaderChannels = ~AdditionalCanvasShaderChannels.None;
            this.m_canvas = canvas;
            this.m_parentSortingLayerID = canvas.sortingLayerID;
            this.m_parentSortingLayerName = canvas.sortingLayerName;
            
            var graphicRaycaster = this.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null)
            {
                graphicRaycaster = this.gameObject.AddComponent<GraphicRaycaster>();
            }

            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.TwoD;
            graphicRaycaster.blockingMask = int.MaxValue;

            this.m_childCount = this.transform.childCount;
            UpdateUILayer();
        }

        private void UpdateUILayer()
        {
            Canvas[] canvasLst = this.GetComponentsInChildren<Canvas>(true);
            Renderer[] rendererLst = this.GetComponentsInChildren<Renderer>(true);
            foreach (var canvas in canvasLst)
            {
                if (canvas == m_canvas)
                    continue;
                var uiLayer = canvas.gameObject.GetComponent<UILayer>();
                if (uiLayer == null)
                {
                    uiLayer = canvas.gameObject.AddComponent<UILayer>();
                    uiLayer.Canvas = canvas;
                }

                canvas.gameObject.AddComponent<UILayer>();
            }

            foreach (var renderer1 in rendererLst)
            {
                var uiLayer = renderer1.gameObject.GetComponent<UILayer>();
                if (uiLayer == null)
                {
                    uiLayer = renderer1.gameObject.AddComponent<UILayer>();
                    uiLayer.Renderer = renderer1;
                }
            }

            this.m_arrLayer = this.gameObject.GetComponentsInChildren<UILayer>(true);
        }

        private void UpdateSortingOrder()
        {
            int count = 0;
            m_canvas.sortingOrder = this.StartOrder;
            
            foreach (UILayer uiLayer in this.m_arrLayer)
            {
                ++count;
                uiLayer.SortingLayerID = this.m_parentSortingLayerID;
                uiLayer.SortingLayerName = this.m_parentSortingLayerName;
                uiLayer.SortingOrder = this.StartOrder + count * 2;

                if (uiLayer.Canvas != null)
                {
                    uiLayer.Canvas.sortingLayerID = this.m_parentSortingLayerID;
                    uiLayer.Canvas.sortingOrder = uiLayer.SortingOrder;
                }
                else if (uiLayer.Renderer != null)
                {
                    uiLayer.Renderer.sortingLayerID = this.m_parentSortingLayerID;
                    uiLayer.Renderer.sortingOrder = uiLayer.SortingOrder;
                }
            }
        }

        public void CalcDepth(int startOrder)
        {
            int curChildCount = this.transform.childCount;

            // 优化, 不需要重新计算SortingOrder.
            if (this.StartOrder == startOrder && this.m_childCount == curChildCount)
            {
                return;
            }

            if (this.StartOrder == startOrder)
            {
                
            }
            
            this.StartOrder = startOrder;
            if (this.m_childCount == curChildCount)
            {
                this.UpdateSortingOrder();
                return;
            }

            this.m_childCount = curChildCount;
            this.UpdateUILayer();
            this.UpdateSortingOrder();
        }
    }
}