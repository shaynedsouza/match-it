using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardID { get; private set; }
    [SerializeField] private GameObject m_frontImage;
    [SerializeField] private GameObject m_backImage;

    // private GameManager gameManager;
    private bool m_isRevealed = false;



    /// <summary>
    /// Setting up the card with the correct ID and image. This will be called from the CardManager when we spawn the cards
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="cardImage"></param>
    public void InitialiseCard(int cardID, Sprite cardImage)
    {
        CardID = cardID;
        m_frontImage.GetComponent<Image>().sprite = cardImage;
    }



    public void OnPointerEnter()
    {
        if (m_isRevealed) return;

        // Add your on-enter logic here
        Debug.Log("Pointer entered card: " + gameObject.name);
    }


    public void OnPointerExit()
    {
        if (m_isRevealed) return;

        // Add your on-enter logic here
        Debug.Log("Pointer exited card: " + gameObject.name);
    }






    public void OnCardClicked()
    {
        if (m_isRevealed) return;

        Debug.Log("Card revealed: " + gameObject.name);
        Reveal();
        // gameManager.CardRevealed(this);
    }

    public void Reveal()
    {
        m_isRevealed = true;
        m_frontImage.SetActive(true);
        m_backImage.SetActive(false);
    }

    // public void Hide()
    // {
    //     isRevealed = false;
    //     front.SetActive(false);
    //     back.SetActive(true);
    // }



}