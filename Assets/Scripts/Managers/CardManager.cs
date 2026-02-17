using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Card CardPrefab;
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

            //Instantiate and set up the card
            Card newCard = Instantiate(CardPrefab, CardContainer);
            newCard.GetComponent<Card>().InitialiseCard(cards[randomIndex].CardID, cards[randomIndex].CardImage);

            //Instantiate the second card of the pair
            newCard = Instantiate(CardPrefab, CardContainer);
            newCard.GetComponent<Card>().InitialiseCard(cards[randomIndex].CardID, cards[randomIndex].CardImage);


            //Remove the index so that we don't get duplicates
            cards.RemoveAt(randomIndex);
        }

        ShuffleChildren();


    }


    //Destroy all the cards
    public void DestroyCards()
    {
        foreach (Transform child in CardContainer)
            Destroy(child.gameObject);

    }



    /// <summary>
    /// Shuffles the children of the CardContainer so that the pairs are not next to each other
    /// </summary>
    public void ShuffleChildren()
    {
        int childCount = CardContainer.childCount;

        // Copy children to array
        Transform[] children = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = CardContainer.GetChild(i);
        }

        // Shuffle
        for (int i = 0; i < childCount; i++)
        {
            int randomIndex = Random.Range(i, childCount);

            Transform temp = children[i];
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        // Apply new sibling order
        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(i);
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