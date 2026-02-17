using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform CardContainer;
    public CardDescription CardDescriptionScriptableObject;
    public float ComparisionDelay = 0.5f;
    public static CardManager Instance;


    private bool m_isFirstCardFlipped = false;
    private Card m_cachedCard;

    private int m_numberOfPairsLeft;
    private int m_score = 0;
    private int bonusMultiplierCount = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }




    public void SpawnCards(int numberOfPairs)
    {
        //We have to mark the cards that have been used to avoid duplicates
        List<Cards> cards = new List<Cards>(CardDescriptionScriptableObject.cards);
        m_numberOfPairsLeft = numberOfPairs;
        m_score = 0;
        bonusMultiplierCount = 0;

        for (int i = 0; i < numberOfPairs; i++)
        {
            //Reset our list of cards if we have used all the cards in the scriptable object
            if (cards.Count == 0)
                cards = new List<Cards>(CardDescriptionScriptableObject.cards);
            else
                Debug.Log(cards);

            //Select a random card
            int randomIndex = Random.Range(0, cards.Count);

            //Get a card from the object pool and initialise it
            Card newCard = ObjectPool.Instance.GetCard();
            newCard.transform.SetParent(CardContainer);
            newCard.transform.localScale = Vector3.one;
            newCard.GetComponent<Card>().InitialiseCard(cards[randomIndex].CardID, cards[randomIndex].CardImage);

            //Get a second card from the object pool and initialise it
            newCard = ObjectPool.Instance.GetCard();
            newCard.transform.SetParent(CardContainer);
            newCard.transform.localScale = Vector3.one;
            newCard.GetComponent<Card>().InitialiseCard(cards[randomIndex].CardID, cards[randomIndex].CardImage);


            //Remove the index so that we don't get duplicates
            cards.RemoveAt(randomIndex);
        }

        ShuffleChildren();


    }


    public void HideAllCards()
    {
        foreach (Transform child in CardContainer)
            child.GetComponent<Card>().Hide();
    }


    /// <summary>
    /// Returns all the cards in the CardContainer to the object pool
    /// </summary>
    public void ReturnCards()
    {
        Debug.Log("Returning cards to pool, CardContainer count" + CardContainer.childCount);
        for (int i = CardContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = CardContainer.GetChild(i);
            ObjectPool.Instance.ReturnCard(child.GetComponent<Card>());
        }
    }



    /// <summary>
    /// Shuffles the children of the CardContainer so that the pairs are not next to each other
    /// </summary>
    public void ShuffleChildren()
    {
        int childCount = CardContainer.childCount;

        for (int i = childCount - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Transform childA = CardContainer.GetChild(i);
            Transform childB = CardContainer.GetChild(randomIndex);

            childA.SetSiblingIndex(randomIndex);
            childB.SetSiblingIndex(i);
        }
    }




    /// <summary>
    /// Logic to handle card comparison and matching. This will be called from the Card script when a card is clicked
    /// </summary>
    /// <param name="card"></param>
    // public void OnCardClicked(Card card)
    public IEnumerator OnCardClicked(Card card)
    {
        yield return new WaitForSeconds(ComparisionDelay);

        if (m_isFirstCardFlipped)
        {
            if (m_cachedCard.CardID == card.CardID)
            {
                AudioManager.Instance.PlaySFX("correct");
                m_isFirstCardFlipped = false;
                m_cachedCard = null;
                m_numberOfPairsLeft--;

                //Combos system that rewards player if they get multiple right clicks in a row
                bonusMultiplierCount++;
                m_score += 10 * bonusMultiplierCount;
                CanvasManager.Instance.SetRoundScore(m_score);


                //Show the multiplier if the player has more than 1 combo
                if (bonusMultiplierCount > 1)
                    CanvasManager.Instance.ShowBonusMultiplier(bonusMultiplierCount);


                //Check if any pairs are left, if not finish the game
                if (m_numberOfPairsLeft <= 0)
                    GameManager.Instance.FinishedGame(m_score);

            }
            else
            {
                AudioManager.Instance.PlaySFX("wrong");
                m_isFirstCardFlipped = false;
                m_cachedCard.Hide();
                card.Hide();
                m_cachedCard = null;
                bonusMultiplierCount = 0;
            }

        }
        else
        {
            m_isFirstCardFlipped = true;
            m_cachedCard = card;
        }
    }
}