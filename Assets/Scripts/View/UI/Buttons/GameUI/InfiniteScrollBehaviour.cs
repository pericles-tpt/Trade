using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Core.Data;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class InfiniteScrollBehaviour : MonoBehaviour
    {
        public GameObject itemCard;
        ItemDataLoader IDL;
        int totalCards;
        void Start()
        {
            int activeCards = 8;

            IDL = GameObject.Find("Camera").GetComponent<ItemDataLoader>();
            int noItems = IDL.GetNoItems();

            // CREATE ALL THE INSTANCES OF CARDS TO ADD TO THE SCROLL VIEW
            addCardsToView(noItems);

            // DEACTIVATES CARDS IF ITEMS IN LIST IS < 50
            deactivateCards(activeCards);

            // BUG FIX: Scroll view for some reason defaults to starting halfway down the scroll space...
            // ... the below code fixes this by setting the scroll view to start at the top instead.
            GameObject.Find("Scroll View").GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1); 

        }

        void addCardsToView(int noItems)
        {
            GameObject newCard;

            for (int i = 0; i < noItems; i++)
            {
                newCard = Instantiate(itemCard, new Vector3(0,0,0), Quaternion.identity);
                newCard.transform.SetParent(this.GetComponent<ScrollRect>().content);

                // BUG FIX: For some reason when instantiated the scale defaults to 36 (instead of 1 like...
                // ... it should be, the below code fixes that
                Vector3 scale = transform.localScale;
                scale.Set(1, 1, 1);
                newCard.transform.localScale = scale;
                newCard.name = "itemCard_" + i;

                Transform ncTransform = newCard.transform;
                ncTransform.FindChild("ItemName").GetComponent<Text>().text = IDL.GetItemName(i);
                ncTransform.FindChild("ItemLocation").GetComponent<Text>().text = IDL.GetItemFound(i)[0].ToString();
                ncTransform.FindChild("ItemCategory").GetComponent<Text>().text = IDL.GetItemCategory(i);
                ncTransform.FindChild("ItemValue").GetComponent<Text>().text = IDL.GetItemBasePrice(i).ToString();

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