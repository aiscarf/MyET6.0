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