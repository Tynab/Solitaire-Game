using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;
using static UnityEngine.Mathf;
using static UnityEngine.PlayerPrefs;
using static UnityEngine.Time;

public sealed class ManagerGame : MonoBehaviour
{
    public GameObject PopUp;
    public Text TxtMessage;
    public Text TxtYourTime;
    public Text TxtCountTime;
    public bool IsPlay;
    private bool _isTime;
    private float _countTime = default;

    private void Start()
    {
        TxtMessage.text = "Ready!!!";
        TxtYourTime.text = $"Your time {DisplayTime(GetYourTime())}";
        PopUp.SetActive(true);
    }

    private void Update()
    {
        if (IsPlay && FindGameObjectsWithTag("Card").Length <= 0)
        {
            OverGame();
        }

        if (_isTime)
        {
            _countTime += deltaTime;
            TxtCountTime.text = DisplayTime(_countTime);
        }
    }

    public void BtnPlayGame()
    {
        PopUp.SetActive(default);
        BtnNewGame();
    }

    public void BtnNewGame()
    {
        foreach (var card in FindObjectsOfType<CardSprite>())
        {
            Destroy(card.gameObject);
        }

        FindObjectOfType<ManagerCard>().DealCard();
        Find("DeckCard").GetComponent<SpriteRenderer>().sprite = FindObjectOfType<ManagerCard>().CardSleeve;
        _isTime = true;
        _countTime = default;
        IsPlay = default;
    }

    private void OverGame()
    {
        _isTime = default;
        SetYourTime(_countTime);
        TxtMessage.text = "You Win!";
        TxtYourTime.text = $"Your time {DisplayTime(GetYourTime())}";
        PopUp.SetActive(true);
    }

    private void SetYourTime(float time)
    {
        var yourTime = GetYourTime();

        if (yourTime > time || yourTime <= 0)
        {
            SetFloat("time", time);
        }
    }

    private float GetYourTime() => GetFloat("time");

    private string DisplayTime(float time) => $"{FloorToInt(time / 60):00}:{FloorToInt(time % 60):00}";
}
