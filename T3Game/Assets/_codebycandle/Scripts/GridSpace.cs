#region IMPORT
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace Codebycandle.T3Game
{
    public class GridSpace : MonoBehaviour
    {
        #region VARIABLE
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;

        private GameController gameController;
        #endregion

        #region METHOD - PUBLIC
        public void SetSpace()
        {
            if (!gameController.MultiPlayerMode && !gameController.PlayerMove) return;

            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
        }

        public void SetGameController(GameController controller)
        {
            gameController = controller;
        }
        #endregion
    }
}
