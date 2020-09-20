using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class InfiniteScrollBehaviour : MonoBehaviour
    {
        public GameObject itemCard;
        // We can fit 8 cards in the scrollview
        float cardHeight = 92.32861f;
        Vector3 lastCardPosition;

        void Start()
        {
            //sView = GameObject.Find("Scroll View");
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

        // BUG: For some reason the y value gets set to some ridiculously high number... and I don't know why
        void addCardsToView(int[] arr, int noCards)
        {
            Vector3 newCardPosition;
            for (int i = 0; i < noCards; i++)
            {
                var newCard = Instantiate(itemCard, new Vector3(-1,-1,-1), Quaternion.identity);
                newCard.transform.SetParent(this.GetComponent<ScrollRect>().content);

                newCardPosition = new Vector3(lastCardPosition.x, lastCardPosition.y - cardHeight - 11f, lastCardPosition.z);
                newCard.transform.position = newCardPosition;

                Vector3 scale = transform.localScale;
                scale.Set(1, 1, 1);
                newCard.transform.localScale = scale;

                Debug.Log("post-card " + i + " position is " + newCard.transform.position);

                lastCardPosition = newCardPosition;

                Debug.Log("pre-card " + i + " position is " + newCard.transform.position);

            }
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}