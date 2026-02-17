using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Card CardPrefab;
    public int InitialPoolSize = 20;


    public static ObjectPool Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < InitialPoolSize; i++)
        {
            Card newCard = Instantiate(CardPrefab, transform);
            newCard.gameObject.SetActive(false);
        }
    }


    public Card GetCard()
    {
        if (transform.childCount > 0)
        {
            Card card = transform.GetChild(0).GetComponent<Card>();
            card.gameObject.SetActive(true);
            card.transform.SetParent(null);
            return card;
        }
        else
        {
            Card newCard = Instantiate(CardPrefab, transform);
            return newCard;
        }
    }


    public void ReturnCard(Card card)
    {
        Debug.Log("Returning card to pool");
        card.gameObject.SetActive(false);
        card.transform.SetParent(transform);
    }

}