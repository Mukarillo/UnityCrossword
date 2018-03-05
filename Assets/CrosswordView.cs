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

        private List<CrosswordTile> mTileList = new List<CrosswordTile>();

        void Start()
        {
            mGridLayoutGroup = crosswordTileParent.gameObject.AddComponent<GridLayoutGroup>();

            mGridLayoutGroup.cellSize = crosswordTilePrefab.GetComponent<RectTransform>().sizeDelta;
            mGridLayoutGroup.spacing = Vector2.zero;
            mGridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            mGridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            mGridLayoutGroup.constraintCount = 9;

            CreateCrossword();
        }

        private void Update(){
            if(Input.GetKeyDown(KeyCode.Space)){
                CreateCrossword();
            }
        }

        private void CreateCrossword(){
            DeleteAllTiles();

            mCrossCreator = new CrosswordCreator();
            mCrossword = mCrossCreator.CreateCrossword(sizeWidth, sizeHeight);

            for (int row = 0; row < mCrossword.tiles.GetLength(0); row++)
            {
                for (int column = 0; column < mCrossword.tiles.GetLength(1); column++)
                {
                    CrosswordTile tile = Instantiate(crosswordTilePrefab, crosswordTileParent).GetComponent<CrosswordTile>();
                    tile.SetupTile(mCrossword.GetTile(new CrosswordPosition(row, column)).element);
                    mTileList.Add(tile);
                }
            }
        }

        private void DeleteAllTiles(){
            for (int i = 0; i < mTileList.Count; i++)
            {
                Destroy(mTileList[i].gameObject);
            }
            mTileList.Clear();
        }
    }
}
