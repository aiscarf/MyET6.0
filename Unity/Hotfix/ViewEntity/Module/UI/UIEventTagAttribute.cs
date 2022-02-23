namespace ET
{
    public class UIEventTagAttribute : BaseAttribute
    {
        public string Name { get; }

        public UIEventTagAttribute(string name)
        {
            this.Name = name;
        }
    }
}