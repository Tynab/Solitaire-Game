using System.Linq;
using UnityEngine;
using static UnityEngine.Camera;
using static UnityEngine.Input;
using static UnityEngine.Physics2D;
using static UnityEngine.Time;
using static UnityEngine.Vector2;
using static UnityEngine.GameObject;

public sealed class MouseInput : MonoBehaviour
{
    public GameObject Slot;
    private ManagerCard _managerCard;
    private float _times;
    private float _setTimes;
    private float _doubleClickTime = 0.5f;
    private int _clickCount;

    private void Start()
    {
        Slot = gameObject;
        _managerCard = FindObjectOfType<ManagerCard>();
        _setTimes = _times;
    }

    private void Update()
    {
        if (_clickCount is 1)
        {
            _times -= deltaTime;
        }

        if (_times <= 0)
        {
            _times = _setTimes;
            _clickCount = default;
        }

        if (_clickCount is 3)
        {
            _times = default;
            _clickCount = 1;
        }

        GetMouseClick();
    }

    public bool StackAble(GameObject selected)
    {
        var s1 = Slot.GetComponent<Selectable>();
        var s2 = selected.GetComponent<Selectable>();

        return !s2.IsDeckPile && (s2.Top
            ? (s1.Suit == s2.Suit || s1.Values is 1 && s2.Suit is null) && s1.Values == s2.Values + 1
            : s1.Values == s2.Values - 1 && s1.Suit is not "C" and not "S" == s2.Suit is not "C" and not "S");
    }

    private void GetMouseClick()
    {
        if (GetMouseButtonDown(0))
        {
            var hit = Raycast(main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f)), zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    _managerCard.DealFromDeck();
                    Slot = gameObject;
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    var selected = hit.collider.gameObject;

                    if (!selected.GetComponent<Selectable>().FaceUp)
                    {
                        if (!blo)
                    }
                }
                else if (hit.collider.CompareTag("PosTop"))
                {
                    print($"PosTop: {hit.collider.name}");
                }
                else if (hit.collider.CompareTag("PosBot"))
                {
                    print($"PosBot: {hit.collider.name}");
                }
            }
        }
    }

    private bool DoubleClick() => _times < _doubleClickTime && _clickCount is 2;

    private void AutoStack(GameObject selected)
    {
        for (var i = 0; i < _managerCard.PosTops.Length; i++)
        {
            var stack = _managerCard.PosTops[i].GetComponent<Selectable>();

            if (selected.GetComponent<Selectable>().Values is 1 && stack.Values is 0)
            {
                Slot = selected;
                StackCard(stack.gameObject);
                break;
            }
            else if (stack.Suit == Slot.GetComponent<Selectable>().Suit && stack.Values == Slot.GetComponent<Selectable>().Values - 1 && HasNoChildren(Slot))
            {
                Slot = selected;

                StackCard(Find(stack.Suit + stack.Values switch
                {
                    1 => "A",
                    11 => "J",
                    12 => "Q",
                    13 => "K",
                    _ => stack.Values.ToString()
                }));

                break;
            }
        }
    }

    private bool HasNoChildren(GameObject card) => card.transform.childCount is 0;

    private void StackCard(GameObject selected)
    {
        var s1 = Slot.GetComponent<Selectable>();
        var s2 = selected.GetComponent<Selectable>();
        var yPos = 0.3f;
        var zPos = 0.01f;

        if (s2.Top || s1.Values is 13)
        {
            yPos = 0;
        }

        Slot.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yPos, selected.transform.position.z - zPos);
        Slot.transform.parent = selected.transform;

        if (s1.IsDeckPile)
        {
            _managerCard.TripsOnDisplay.Remove(Slot.name);
        }
        else if (s1.Top)
        {
            if (s2.Top && s1.Values is 1)
            {
                _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Values = default;
                _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Suit = default;
            }
            else
            {
                _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Values = s1.Values - 1;
            }
        }
        else
        {
            _managerCard.Bots[s1.Row].Remove(Slot.name);
        }

        s1.IsDeckPile = default;
        s1.Row = s2.Row;

        if (s2.Top)
        {
            _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Values = s1.Values;
            _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Suit = s1.Suit;
            s1.Top = true;
        }
        else
        {
            s1.Top = default;
        }

        Slot = gameObject;
    }

    private void StackBots(GameObject selected)
    {
        var s1 = Slot.GetComponent<Selectable>();
        var s2 = selected.GetComponent<Selectable>();
        var yPos = 0.3f;
        var zPos = 0.01f;

        for (var i = 0; i <= 13; i++)
        {
            if (s2.Top || s1.Values == i)
            {
                yPos = default;
                break;
            }
        }

        Slot.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yPos, selected.transform.position.z - zPos);
        Slot.transform.parent = selected.transform;

        if (s1.IsDeckPile)
        {
            _managerCard.TripsOnDisplay.Remove(Slot.name);
        }
        else if (s1.Top)
        {
            if (s2.Top && s1.Values is 1)
            {
                _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Values = default;
                _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Suit = default;
            }
            else
            {
                _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Values = s1.Values - 1;
            }
        }
        else
        {
            _managerCard.Bots[s1.Row].Remove(Slot.name);
        }

        s1.IsDeckPile = default;
        s1.Row = s2.Row;

        if (s2.Top)
        {
            _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Values = s1.Values;
            _managerCard.PosTops[s1.Row].GetComponent<Selectable>().Suit = s1.Suit;
            s1.Top = true;
        }
        else
        {
            s1.Top = default;
        }

        Slot = gameObject;
    }

    private bool Blocked(GameObject selected)
    {
        var select = selected.GetComponent<Selectable>();

        return select.IsDeckPile ? select.name != _managerCard.TripsOnDisplay.LastOrDefault() : selected.name != _managerCard.Bots[select.Row].LastOrDefault();
    }
}
