namespace Codebycandle.T3Game
{
    public interface IGameController
    {
        string GetPlayerSide();
        string GetComputerSide();

        void StartGameSingle();
        void StartGamePvP();
        void EndTurn();
        void EndGame(string endText);
        void RestartGame();
    }
}
