using UnityEngine;
using static ManagerCard;

public sealed class CardSprite : MonoBehaviour
{
    public Sprite FaceCard;
    public Sprite BackCard;
    private SpriteRenderer _displayCard;
    private ManagerCard _managerCard;
    private Selectable _selectable;

    private void Start() => SetDeckCard();

    private void Update() => _displayCard.sprite = _selectable.FaceUp ? FaceCard : BackCard;

    private void SetDeckCard()
    {
        _managerCard = FindObjectOfType<ManagerCard>();

        var index = 0;

        foreach (var card in GenerateDeckCard())
        {
            if (name == card)
            {
                FaceCard = _managerCard.FaceCard[index];
                break;
            }

            index++;
        }

        _displayCard = GetComponent<SpriteRenderer>();
        _selectable = GetComponent<Selectable>();
    }
}
