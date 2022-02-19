namespace ET
{
    public class UITagAttribute : BaseAttribute
    {
        public string Name;
        public UITagAttribute(string name)
        {
            Name = name;
        }
    }
}