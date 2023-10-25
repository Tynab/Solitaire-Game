using UnityEngine;
using static ManagerCard;
using static UnityEngine.Color;

public sealed class CardSprite : MonoBehaviour
{
    public Sprite FaceCard;
    public Sprite BackCard;
    private SpriteRenderer _displayCard;
    private ManagerCard _managerCard;
    private Selectable _selectable;
    private MouseInput _mouseInput;

    private void Start() => SetDeckCard();

    private void Update()
    {
        _displayCard.sprite = _selectable.FaceUp ? FaceCard : BackCard;

        if (_mouseInput.Slot)
        {
            _displayCard.color = name == _mouseInput.Slot.name ? green : white;
        }
    }

    private void SetDeckCard()
    {
        _managerCard = FindObjectOfType<ManagerCard>();
        _mouseInput = FindObjectOfType<MouseInput>();

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
