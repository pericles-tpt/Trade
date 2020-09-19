using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class InfiniteScrollBehaviour : MonoBehaviour
    {
        // We can fit 8 cards in the scrollview
        UnityEngine.Object itemCard = Resources.Load("Assets/Prefabs/ItemCard");
        float cardHeight = 92.32861f;
        Vector3 lastCardPosition;

        void Start()
        {
            lastCardPosition = this.transform.position;

            int[] a = new int[100];
            Debug.Log("a is " + a[0]);

            if (a.Length < 8)
            {
                addCardsToView(a, a.Length);
            } else
            {
                addCardsToView(a, 16);
            }

        }

        void addCardsToView(int[] arr, int noCards)
        {
            Vector3 newCardPosition;
            for (int i = 0; i < noCards; i++)
            {
                newCardPosition = new Vector3(lastCardPosition.x + cardHeight + 11f, lastCardPosition.y, lastCardPosition.z);
                var newCard = Instantiate(itemCard, newCardPosition, Quaternion.identity);
                newCard.transform.parent = this.GetComponent<ScrollRect>().content.gameObject;
                lastCardPosition = newCardPosition;
                
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}