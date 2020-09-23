using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class InfiniteScrollBehaviour : MonoBehaviour
    {
        public GameObject itemCard;
        int totalCards = 50;
        void Start()
        {
            int activeCards = 8;

            // CREATE ALL THE INSTANCES OF CARDS TO ADD TO THE SCROLL VIEW
            int[] a = new int[totalCards];
            addCardsToView(a, a.Length);

            // DEACTIVATES CARDS IF ITEMS IN LIST IS < 50
            deactivateCards(activeCards);

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
                newCard.name = "itemCard_" + i;

            }

        }

        void deactivateCards(int deactivateAfterAndIncluding)
        {
            for (int i = deactivateAfterAndIncluding; i < totalCards; i++)
            {
                GameObject.Find("itemCard_" + i).SetActive(false);
            }
        }

    }

}