using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class InfiniteScrollBehaviour : MonoBehaviour
    {
        public GameObject itemCard;

        void Start()
        {
            int[] a = new int[50];
            addCardsToView(a, a.Length);

            // BUG FIX: Scroll view for some reason defaults to starting halfway down the scroll space...
            // ... the below code fixes this by setting the scroll view to start at the top instead.
            GameObject.Find("Scroll View").GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1); 

        }

        void addCardsToView(int[] arr, int noCards)
        {
            GameObject newCard;

            for (int i = 0; i < noCards; i++)
            {
                newCard = Instantiate(itemCard, new Vector3(0,0,0), Quaternion.identity);
                newCard.transform.SetParent(this.GetComponent<ScrollRect>().content);

                // BUG FIX: For some reason when instantiated the scale defaults to 36 (instead of 1 like...
                // ... it should be, the below code fixes that
                Vector3 scale = transform.localScale;
                scale.Set(1, 1, 1);
                newCard.transform.localScale = scale;
                newCard.name = "itemCard_" + i + arr[i];

            }

        }

    }

}