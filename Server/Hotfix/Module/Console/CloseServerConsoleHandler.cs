namespace ET
{
    [ConsoleHandler(ConsoleMode.Server)]
    public class CloseServerConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(ModeContex contex, string content)
        {
            var lines = content.Split(' ');
            if (lines.Length < 2)
                return;
            switch (lines[1])
            {
                case "-h":
                    LogHelper.Console(SceneType.Process, $"提供一些帮助命令");
                    break;
                case "close":
                    // TODO 询问是否真的要关闭, 服务器.
                    LogHelper.Console(SceneType.Process, $"server close........................ {Game.Scene.Id}, Process:{Game.Options.Process:000000}");
                    Game.Close();
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}