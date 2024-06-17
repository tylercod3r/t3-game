#region IMPORT
using Radishmouse;
using UnityEngine;
#endregion

namespace Codebycandle.T3Game
{
    [RequireComponent(typeof(UILineRenderer))]
    public class WinOverlayController : MonoBehaviour
    {
        #region VARIABLE
        private UILineRenderer lineRenderer;

        private const int gridPixelSize = 512;
        private const int gridCellSize = 3;
        #endregion

        #region METHOD - PUBLIC
        public void EnablePanel(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void DrawWin(int startIndex, int endIndex)
        {
            EnablePanel(true);

            lineRenderer.points = new Vector2[] { GetTargetPosition(startIndex), GetTargetPosition(endIndex) };
        }
        #endregion

        #region METHOD - MONOBEHAVIOUR
        private void Awake()
        {
            lineRenderer = GetComponent<UILineRenderer>();
        }
        #endregion

        #region METHOD - PRIVATE
        private Vector2 GetTargetPosition(int cellIndex)
        {
            var columnW = gridPixelSize / gridCellSize;
            var offsetHalf = columnW / 2;

            var rowIndex = cellIndex / gridCellSize;
            var colIndex = cellIndex % gridCellSize;

            var x1 = 0;
            var y1 = 0;
            switch (rowIndex)
            {
                case 0:
                    y1 = gridPixelSize - offsetHalf;
                    break;
                case 1:
                    y1 = gridPixelSize / 2;
                    break;
                case 2:
                    y1 = offsetHalf;
                    break;
            }

            switch (colIndex)
            {
                case 0:
                    x1 = offsetHalf;
                    break;
                case 1:
                    x1 = gridPixelSize / 2;
                    break;
                case 2:
                    x1 = gridPixelSize - offsetHalf;
                    break;
            }

            return new Vector2(x1, y1);
        }
        #endregion
    }
}