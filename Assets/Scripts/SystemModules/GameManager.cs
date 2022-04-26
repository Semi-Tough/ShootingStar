/****************************************************
    文件：GameManager.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/26 19:51:37
    功能：游戏管理器
*****************************************************/

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState { get; set; } = GameState.Playing;
}

public enum GameState
{
    Playing,
    Pause,
    GameOve
}