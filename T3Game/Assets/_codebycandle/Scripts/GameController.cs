#region IMPORT
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class GameController : MonoBehaviour
{
    #region VARIABLE
    public TMP_Text[] buttonList;

    private const string player1Name = "X";
    private const string player2Name = "O";

    private string playerSide = player1Name;

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
    #endregion

    #region METHOD - PUBLIC
    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        if (CheckWin())
        {
            EndGame();
        }
        else
        {
            ChangeSides();
        }
    }

    public void EndGame()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
    }
    #endregion

    #region METHOD - MONOBEHAVIOUR
    private void Awake()
    {
        InitButtons();
    }
    #endregion

    #region METHOD - PRIVATE
    private void InitButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameController(this);
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
                if (buttonList[values[0]].text == playerSide
                    && buttonList[values[1]].text == playerSide
                    && buttonList[values[2]].text == playerSide)
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
    }
    #endregion
}
