#region IMPORT
using TMPro;
using UnityEngine;
#endregion

public class GameController : MonoBehaviour
{
    #region VARIABLE
    public TMP_Text[] buttonList;
    #endregion

    #region METHOD - PUBLIC
    public string GetPlayerSide()
    {
        return "?";
    }

    public void EndTurn()
    {
        Debug.LogError("end turn isn't implemented.");
    }
    #endregion

    #region METHOD - PRIVATE
    private void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameController(this);
        }
    }
    #endregion
}
