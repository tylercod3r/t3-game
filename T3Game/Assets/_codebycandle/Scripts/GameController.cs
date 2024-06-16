#region IMPORT
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

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

public class GameController : MonoBehaviour
{
    #region VARIABLE
    public TMP_Text[] buttonLabelList;

    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject playerIndicator;

    [SerializeField] private Player playerX;
    [SerializeField] private Player playerO;

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

    private readonly string winText = " Wins!";
    private readonly string drawText = "It's a Draw!";

    private const string player1Name = "X";
    private const string player2Name = "O";
    
    private const int maxMoveCount = 9;

    private string playerSide = player1Name;

    private int moveCount;
    #endregion

    #region METHOD - PUBLIC
    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        if (CheckWin())
        {
            EndGame(playerSide + winText);
        }
        else if (moveCount >= maxMoveCount)
        {
            EndGame(drawText);
        }
        else
        {
            ChangeSides();
        }
    }

    public void StartGame()
    {
        EnableGameStartPanel(false);
        EnablePlayerIndicator(true);

        EnableBoard(true);
    }

    public void EndGame(string endText)
    {
        EnableBoard(false);

        EnableGameOverPanel(true);

        gameOverText.text = endText;
    }

    public void RestartGame()
    {
        playerSide = player1Name;
        moveCount = 0;
        EnableGameOverPanel(false);
        EnablePlayerIndicator(false);

        EnableBoard(true);

        ResetPlayerColors();

        EnableGameStartPanel(true);
    }
    #endregion

    #region METHOD - MONOBEHAVIOUR
    private void Awake()
    {
        EnableGameStartPanel(true);
        EnableGameOverPanel(false);
        EnablePlayerIndicator(false);

        InitButtons();

        ResetPlayerColors();
    }
    #endregion

    #region METHOD - PRIVATE
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

            if(enable)
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

        foreach(var key in keys)
        {
            if (winDict.ContainsKey(key))
            {
                var values = winDict[key];
                if (buttonLabelList[values[0]].text == playerSide
                    && buttonLabelList[values[1]].text == playerSide
                    && buttonLabelList[values[2]].text == playerSide)
                {
                    Debug.Log(playerSide + " won by: " + key);

                    return true;
                }
            }
        }

        return false;
    }

    private void ChangeSides()
    {
        playerSide = playerSide == player1Name ? player2Name : player1Name;

        if(playerSide == player1Name)
        {
            ResetPlayerColors();
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }
    #endregion
}
