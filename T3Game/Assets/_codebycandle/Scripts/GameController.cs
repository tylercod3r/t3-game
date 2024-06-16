#region IMPORT
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace Codebycandle.T3Game
{
    [System.Serializable]
    public class Player
    {
        public Image panel;
        public TMP_Text text;
    }

    [System.Serializable]
    public class PlayerColor
    {
        public Color panelColor;
        public Color textColor;
    }

    public class GameController : MonoBehaviour, IGameController
    {
        #region VARIABLE
        public bool MultiPlayerMode { get; private set; }
        public bool PlayerMove { get; private set; }

        [SerializeField] private AudioController audioController;

        [SerializeField] private GameObject gameStartPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private GameObject playerIndicator;

        [SerializeField] private Player playerX;
        [SerializeField] private Player playerO;

        [SerializeField] private TMP_Text[] buttonLabelList;

        [SerializeField] private PlayerColor activePlayerColor;
        [SerializeField] private PlayerColor inactivePlayerColor;

        private readonly Dictionary<string, List<int>> winDict = new()
    {
        { "row1", new List<int>(){0, 1, 2 } },
        { "row2", new List<int>(){3, 4, 5 } },
        { "row3", new List<int>(){6, 7, 8 } },

        { "col1", new List<int>(){0, 3, 6 } },
        { "col2", new List<int>(){1, 4, 7 } },
        { "col3", new List<int>(){2, 5, 8 } },

        { "diag1", new List<int>(){0, 4, 8 } },
        { "diag2", new List<int>(){2, 4, 6 } },
    };

        private const string winText = " Wins!";
        private const string drawText = "It's a Draw!";
        private const string player1Name = "X";
        private const string player2Name = "O";
        private const string computerName = "C";
        private const string computerSide = computerName;
        private const int maxMoveCount = 9;
        private const int computerMoveDelaySeconds = 1;

        private string playerSide = player1Name;
        private string winner;
        private int moveCount;
        #endregion

        #region METHOD - PUBLIC
        public string GetPlayerSide()
        {
            return playerSide;
        }

        public string GetComputerSide()
        {
            return computerSide;
        }

        public void EndTurn()
        {
            moveCount++;

            audioController.PlayClickSound();

            if (CheckWin())
            {
                EndGame(winner + winText);

                if (winner == computerSide)
                {
                    audioController.PlayLossSound();
                }
                else
                {
                    audioController.PlayWinSound();
                }
            }
            else if (moveCount >= maxMoveCount)
            {
                EndGame(drawText);

                audioController.PlayDrawSound();
            }
            else
            {
                ChangeSides();

                if (!MultiPlayerMode && !PlayerMove)
                {
                    StartCoroutine(StartComputerTurn());
                }
            }
        }

        public void StartGameSingle()
        {
            StartGame(false);
        }

        public void StartGamePvP()
        {
            StartGame(true);
        }

        public void EndGame(string endText)
        {
            EnableBoard(false);

            EnableGameOverPanel(true);

            gameOverText.text = endText;
        }

        public void RestartGame()
        {
            winner = "";
            playerSide = player1Name;
            moveCount = 0;
            MultiPlayerMode = false;

            EnableGameOverPanel(false);
            EnablePlayerIndicator(false);
            ResetPlayerColors();

            EnableBoard(true);
            EnableGameStartPanel(true);
        }
        #endregion

        #region METHOD - MONOBEHAVIOUR
        private void Awake()
        {
            EnableGameOverPanel(false);
            EnablePlayerIndicator(false);

            EnableGameStartPanel(true);

            InitButtons();

            ResetPlayerColors();

            PlayerMove = true;
        }
        #endregion

        #region METHOD - PRIVATE
        private void StartGame(bool multiPlayer)
        {
            MultiPlayerMode = multiPlayer;

            EnableGameStartPanel(false);
            EnablePlayerIndicator(true);

            EnableBoard(true);
        }

        private void SetPlayerColors(Player newPlayer, Player oldPlayer)
        {
            newPlayer.panel.color = activePlayerColor.panelColor;
            newPlayer.text.color = activePlayerColor.textColor;

            oldPlayer.panel.color = inactivePlayerColor.panelColor;
            oldPlayer.text.color = inactivePlayerColor.textColor;
        }

        private void ResetPlayerColors()
        {
            SetPlayerColors(playerX, playerO);
        }

        private void EnableBoard(bool enable)
        {
            for (int i = 0; i < buttonLabelList.Length; i++)
            {
                buttonLabelList[i].GetComponentInParent<Button>().interactable = enable;

                if (enable)
                {
                    buttonLabelList[i].text = "";
                }
            }
        }

        private void EnableGameStartPanel(bool enable)
        {
            gameStartPanel.SetActive(enable);
        }

        private void EnableGameOverPanel(bool enable)
        {
            gameOverPanel.SetActive(enable);
        }

        private void EnablePlayerIndicator(bool enable)
        {
            playerIndicator.SetActive(enable);
        }

        private void InitButtons()
        {
            for (int i = 0; i < buttonLabelList.Length; i++)
            {
                buttonLabelList[i].GetComponentInParent<GridSpace>().SetGameController(this);
            }
        }

        private bool CheckWin()
        {
            var keys = winDict.Keys.ToArray();

            foreach (var key in keys)
            {
                if (winDict.ContainsKey(key))
                {
                    var values = winDict[key];
                    if (buttonLabelList[values[0]].text == playerSide
                        && buttonLabelList[values[1]].text == playerSide
                        && buttonLabelList[values[2]].text == playerSide)
                    {
                        Debug.Log(playerSide + " won by: " + key);

                        winner = playerSide;

                        return true;
                    }

                    if (!MultiPlayerMode)
                    {
                        if (buttonLabelList[values[0]].text == computerSide
                            && buttonLabelList[values[1]].text == computerSide
                            && buttonLabelList[values[2]].text == computerSide)
                        {
                            Debug.Log(computerSide + " won by: " + key);

                            winner = computerSide;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void ChangeSides()
        {
            if (MultiPlayerMode)
            {
                playerSide = playerSide == player1Name ? player2Name : player1Name;
            }

            PlayerMove = !PlayerMove;

            if (PlayerMove)
            {
                ResetPlayerColors();
            }
            else
            {
                SetPlayerColors(playerO, playerX);
            }
        }

        private IEnumerator StartComputerTurn()
        {
            yield return new WaitForSeconds(computerMoveDelaySeconds);

            if (!MultiPlayerMode && !PlayerMove)
            {
                var spaceFound = false;
                while (!spaceFound)
                {
                    var randomIndex = Random.Range(0, maxMoveCount - 1);
                    if (buttonLabelList[randomIndex].GetComponentInParent<Button>().interactable)
                    {
                        spaceFound = true;

                        buttonLabelList[randomIndex].text = GetComputerSide();
                        buttonLabelList[randomIndex].GetComponentInParent<Button>().interactable = false;

                        EndTurn();
                    }

                    yield return new WaitForEndOfFrame();
                }
            }

            yield return null;
        }
        #endregion
    }
}
