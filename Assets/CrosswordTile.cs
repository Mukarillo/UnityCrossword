using UnityEngine;
using UnityEngine.UI;

namespace crossword.view
{
    public class CrosswordTile : MonoBehaviour
    {
        public Text textElement;

        public void SetupTile(string element){
            textElement.text = element;
        }
    }
}