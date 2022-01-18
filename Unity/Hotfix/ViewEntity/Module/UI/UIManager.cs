using System;
using System.Collections.Generic;

namespace ET
{
    public class UiCell
    {
        public UI m_ui;
        public List<UI> m_coverList = new List<UI>();

        public UiCell(UI ui)
        {
            this.m_ui = ui;
        }

        public void AddCoverUi(UI ui)
        {
            if (m_coverList.Contains(ui))
                return;
            m_coverList.Add(ui);
        }

        public void CoverUis()
        {
            for (int i = 0; i < m_coverList.Count; i++)
            {
                var ui = m_coverList[i];
                ui.IsCovered = true;
                UIMediatorManager.Instance.BeCover(ui.Name);
            }
        }

        public void UnCoverUis()
        {
            for (int i = 0; i < m_coverList.Count; i++)
            {
                var ui = m_coverList[i];
                ui.IsCovered = false;
                UIMediatorManager.Instance.UnCover(ui.Name);
            }
        }
    }

    public class UIManager : Entity
    {
        public static UIManager Instance { get; set; }
        public Dictionary<string, UI> m_allUiMap;
        public List<UI> m_openUis;
        public List<UiCell> m_uiStack;
        public Dictionary<string, Type> m_allUiType;
    }
}