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
        TxtMessage.text = "Ready !!!";
        TxtYourTime.text = $"your time {DisplayTime(GetYourTime())}";
        PopUp.SetActive(true);
    }

    private void Update()
    {
        var cards = FindGameObjectsWithTag("Card");

        if (IsPlay && cards.Length <= 0)
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
        var cards = FindObjectsOfType<CardSprite>();

        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }

        FindObjectOfType<ManagerCard>().DealCard();
        _isTime = true;
        _countTime = default;
        IsPlay = default;
    }

    private void OverGame()
    {
        _isTime = default;
        SetYourTime(_countTime);
        TxtMessage.text = "You Win!";
        TxtYourTime.text = $"your time {DisplayTime(GetYourTime())}";
        PopUp.SetActive(true);
    }

    private void SetYourTime(float time)
    {
        var yourTime = GetYourTime();

        if (yourTime > time || yourTime <= 0)
        {
            SetFloat("time", time);
            _ = GetYourTime();
        }
    }

    private float GetYourTime() => GetFloat("time");

    private string DisplayTime(float time) => string.Format("{0:00}:{1:00}", FloorToInt(time / 60), FloorToInt(time % 60));
}
