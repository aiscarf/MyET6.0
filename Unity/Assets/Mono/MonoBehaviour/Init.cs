using System.Threading;
using UnityEngine;

namespace ET
{
	// 1 mono模式 2 ILRuntime模式 3 mono热重载模式
	public enum CodeMode
	{
		Mono = 1,
		ILRuntime = 2,
		Reload = 3,
	}
	/// <summary>
	/// TODO 分三种加载代码的方式
	/// TODO 分三种加载资源的方式
	/// </summary>
	public class Init: MonoBehaviour
	{
		public CodeMode CodeMode = CodeMode.Mono;
		public bool IsDebug = false;
		
		private void Awake()
		{
#if ENABLE_IL2CPP
			this.CodeMode = CodeMode.ILRuntime;
			this.IsDebug = false;
#endif
			
			System.AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};
			
			SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
			
			DontDestroyOnLoad(gameObject);

			ETTask.ExceptionHandler += Log.Error;

			Log.ILog = new UnityLogger();

			Options.Instance = new Options();
		}

		private void Start()
		{
			CodeLoader.Instance.Start(this.CodeMode, this.IsDebug);
		}

		private void Update()
		{
			CodeLoader.Instance.Update?.Invoke();
		}

		private void LateUpdate()
		{
			CodeLoader.Instance.LateUpdate?.Invoke();
		}

		private void OnApplicationQuit()
		{
			CodeLoader.Instance.OnApplicationQuit?.Invoke();
			CodeLoader.Instance.Dispose();
		}
	}
}