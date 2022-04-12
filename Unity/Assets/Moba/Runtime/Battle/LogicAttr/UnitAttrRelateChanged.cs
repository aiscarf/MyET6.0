namespace Scarf.Moba
{
    public class UnitAttrRelateChanged: IRelateChanged
    {
        private UnitAttrComponent m_cAttr;
        private int m_nBaseType;
        private int m_nAddType;
        private int m_nTargetType;

        public UnitAttrRelateChanged(UnitAttrComponent unitAttr, int baseType, int addType, int targetType)
        {
            this.m_cAttr = unitAttr;
            this.m_nBaseType = baseType;
            this.m_nAddType = addType;
            this.m_nTargetType = targetType;
        }

        public void Handler()
        {
            this.m_cAttr.SetValue(this.m_nTargetType,
                this.m_cAttr.GetValue(this.m_nBaseType) + this.m_cAttr.GetValue(this.m_nAddType));
        }

        public void Reset()
        {
        }

        public void Clear()
        {
            this.m_cAttr = (UnitAttrComponent)null;
        }
    }

    public class UnitAttrRelateChanged2: IRelateChanged
    {
        private UnitAttrComponent m_cAttr;
        private int m_nBaseType;
        private int m_nAddType;
        private int m_nTargetType;
        private int m_nRatioType;

        public UnitAttrRelateChanged2(UnitAttrComponent unitAttr, int baseType, int addType, int ratioType, int targetType)
        {
            this.m_cAttr = unitAttr;
            this.m_nBaseType = baseType;
            this.m_nAddType = addType;
            this.m_nTargetType = targetType;
            this.m_nRatioType = ratioType;
        }

        public void Handler()
        {
            this.m_cAttr.SetValue(this.m_nTargetType,
                (int)(this.m_cAttr.GetValue(this.m_nBaseType) * (1000L + this.m_cAttr.GetValue(this.m_nRatioType)) / 1000L +
                    this.m_cAttr.GetValue(this.m_nAddType)));
        }

        public void Reset()
        {
        }

        public void Clear()
        {
            this.m_cAttr = (UnitAttrComponent)null;
        }
    }
}