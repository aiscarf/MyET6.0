namespace ET
{
	[UIEventTag(UIType.UIAGm)]
	public class UIAGmEvent : UIEvent<UIAGmComponent>
	{
		public override async ETTask PreOpen()
		{
			await UIManager.Instance.OpenForeverUI(UIType.UIAGm);
		}

		public override async ETTask PreClose()
		{
			await UIManager.Instance.CloseUI(UIType.UIAGm);
		}
	}
}
