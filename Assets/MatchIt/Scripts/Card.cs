using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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




    public void Reveal()
    {
        m_isRevealed = true;

        DOTween.Kill(m_backImage.transform);
        DOTween.Kill(m_frontImage.transform);

        AudioManager.Instance.PlaySFX("card_turn");

        m_frontImage.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0f);
        m_backImage.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.3f).OnComplete(() =>
        {
            m_frontImage.SetActive(true);
            m_backImage.SetActive(false);
            m_frontImage.transform.DOLocalRotate(Vector3.zero, 0.3f);
        });

    }

    public void Hide()
    {
        m_isRevealed = false;

        DOTween.Kill(m_backImage.transform);
        DOTween.Kill(m_frontImage.transform);

        // AudioManager.Instance.PlaySFX("card_turn");

        m_backImage.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0f);
        m_frontImage.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.3f).OnComplete(() =>
        {
            m_backImage.SetActive(true);
            m_frontImage.SetActive(false);
            m_backImage.transform.DOLocalRotate(Vector3.zero, 0.3f);
        });

    }


    #region Callbacks
    public void OnPointerEnter()
    {
        if (m_isRevealed) return;

        DOTween.Kill(m_backImage.transform);
        m_backImage.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.3f);
        m_backImage.transform.DOScale(new Vector3(1.1f, 1.1f, 0f), 0.2f).SetEase(Ease.InSine);
    }

    public void OnPointerExit()
    {
        if (m_isRevealed) return;

        DOTween.Kill(m_backImage.transform);
        m_backImage.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.3f);
        m_backImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InSine);
    }

    public void OnCardClicked()
    {
        if (m_isRevealed) return;

        Reveal();
        StartCoroutine(CardManager.Instance.OnCardClicked(this));
    }

    #endregion

}