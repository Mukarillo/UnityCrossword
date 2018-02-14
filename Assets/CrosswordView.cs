using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crossword.engine;

namespace crossword.view
{
    public class CrosswordView : MonoBehaviour
    {
        public int sizeWidth;
        public int sizeHeight;

        public GameObject crosswordTilePrefab;
        public Transform crosswordTileParent;

        private CrosswordCreator mCrossCreator;
        private Crossword mCrossword;

        private GridLayoutGroup mGridLayoutGroup;

        // Use this for initialization
        void Start()
        {
            mCrossCreator = new CrosswordCreator();
            mCrossCreator.InitDatabase();
            mCrossword = mCrossCreator.CreateCrossword(sizeWidth, sizeHeight);

            mGridLayoutGroup = crosswordTileParent.gameObject.AddComponent<GridLayoutGroup>();
            mGridLayoutGroup.cellSize = crosswordTilePrefab.GetComponent<RectTransform>().sizeDelta;
            mGridLayoutGroup.spacing = Vector2.zero;
            mGridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            mGridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            mGridLayoutGroup.constraintCount = 9;

            for (int row = 0; row < mCrossword.tiles.GetLength(0); row++)
            {
                for (int column = 0; column < mCrossword.tiles.GetLength(1); column++)
                {
                    CrosswordTile tile = Instantiate(crosswordTilePrefab, crosswordTileParent).GetComponent<CrosswordTile>();
                    tile.SetupTile(mCrossword.GetTile(row,column).element);
                }
            }
        }
    }
}
