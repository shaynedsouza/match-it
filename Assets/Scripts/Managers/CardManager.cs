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





    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }





    private void Start()
    {
        SpawnCards(16);
    }




    public void SpawnCards(int numberOfCards)
    {
        //We have to mark the cards that have been used to avoid duplicates
        List<Cards> cards = new List<Cards>(CardDescriptionScriptableObject.cards);




        //Dividing by 2 since we want pairs of cards
        for (int i = 0; i < numberOfCards / 2; i++)
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

        Debug.Log("Card clicked: " + card.CardID);
        if (m_isFirstCardFlipped)
        {
            if (m_cachedCard.CardID == card.CardID)
            {
                Debug.Log("Match!");
                AudioManager.Instance.PlaySFX("correct");
                m_isFirstCardFlipped = false;
                m_cachedCard = null;
            }
            else
            {
                Debug.Log("No match!");
                AudioManager.Instance.PlaySFX("wrong");
                m_isFirstCardFlipped = false;
                m_cachedCard.Hide();
                card.Hide();
                m_cachedCard = null;
            }
        }
        else
        {
            m_isFirstCardFlipped = true;
            m_cachedCard = card;
        }
    }
}