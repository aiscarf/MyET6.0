namespace ET
{
	[UIEventTag(UIType.UIMain)]
	public class UIMainEvent : UIEvent
	{
		public override async ETTask PreOpen()
		{
			await UIManager.Instance.OpenAndCoverAll(UIType.UIMain);
		}
	}
}
