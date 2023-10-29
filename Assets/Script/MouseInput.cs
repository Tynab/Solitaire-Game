using System.Linq;
using UnityEngine;
using static UnityEngine.Camera;
using static UnityEngine.GameObject;
using static UnityEngine.Input;
using static UnityEngine.Physics2D;
using static UnityEngine.Time;
using static UnityEngine.Vector2;

public sealed class MouseInput : MonoBehaviour
{
    public GameObject Slot;
    private const float _yPos = 0.9f;
    private const float _zPos = 0.01f;
    private readonly float _doubleClickTime = 0.5f;
    private ManagerCard _managerCard;
    private float _times;
    private float _setTimes;
    private int _clickCount;

    private void Start()
    {
        _managerCard = FindObjectOfType<ManagerCard>();
        _setTimes = _times;
        Slot = gameObject;
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

        return !s2.IsDeckPile && (s2.Top ? (s1.Suit == s2.Suit || s1.Values is 1 && s2.Suit is null) && s1.Values == s2.Values + 1 : s1.Values == s2.Values - 1 && s1.Suit is not "C" and not "S" != s2.Suit is not "C" and not "S");
    }

    private void GetMouseClick()
    {
        if (GetMouseButtonDown(default))
        {
            var hit = Raycast(main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1f)), zero);

            if (hit)
            {
                var s1 = Slot.GetComponent<Selectable>();

                if (hit.collider.CompareTag("Deck"))
                {
                    _managerCard.DealFromDeck();
                    Slot = gameObject;
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    var selected = hit.collider.gameObject;
                    var s2 = selected.GetComponent<Selectable>();

                    if (!Blocked(selected))
                    {
                        if (!s2.FaceUp)
                        {
                            s2.FaceUp = true;
                            Slot = gameObject;

                            if (Slot == selected && DoubleClick())
                            {
                                AutoStack(selected);
                            }
                            else
                            {
                                Slot = selected;
                            }
                        }
                        else if (s2.IsDeckPile)
                        {
                            Slot = selected;
                        }
                    }

                    if (Slot == gameObject)
                    {
                        Slot = selected;
                    }
                    else if (Slot != selected)
                    {
                        if (StackAble(selected))
                        {
                            StackCard(selected);
                        }
                        else
                        {
                            Slot = selected;
                        }
                    }
                }
                else if (hit.collider.CompareTag("PosTop") && Slot.CompareTag("Card") && s1.FaceUp)
                {
                    for (var i = 1; i <= 13; i++)
                    {
                        if (s1.Values == i)
                        {
                            StackPos(hit.collider.gameObject);
                            break;
                        }
                    }
                }
                else if (hit.collider.CompareTag("PosBot") && Slot.CompareTag("Card") && s1.FaceUp)
                {
                    for (var i = 1; i <= 13; i++)
                    {
                        if (s1.Values == i)
                        {
                            StackPos(hit.collider.gameObject);
                            break;
                        }
                    }
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
            var s1 = Slot.GetComponent<Selectable>();
            var s2 = selected.GetComponent<Selectable>();

            if (s2.Values is 1 && stack.Values is 0)
            {
                Slot = selected;
                StackCard(stack.gameObject);
                break;
            }
            else if (stack.Suit == s1.Suit && stack.Values == s1.Values - 1 && HasNoChildren(Slot))
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

        var yPos = _yPos;
        var zPos = _zPos;

        if (s2.Top || s1.Values is 13)
        {
            yPos = default;
        }

        Slot.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yPos, selected.transform.position.z - zPos);
        Slot.transform.parent = selected.transform;

        if (s1.IsDeckPile)
        {
            _ = _managerCard.TripsOnDisplay.Remove(Slot.name);
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
            _ = _managerCard.Bots[s1.Row].Remove(Slot.name);
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

    private void StackPos(GameObject selected)
    {
        var s1 = Slot.GetComponent<Selectable>();
        var s2 = selected.GetComponent<Selectable>();

        var yPos = _yPos;
        var zPos = _zPos;

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
            _ = _managerCard.TripsOnDisplay.Remove(Slot.name);

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
            _ = _managerCard.Bots[s1.Row].Remove(Slot.name);
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
        var s2 = selected.GetComponent<Selectable>();

        return s2.IsDeckPile ? s2.name != _managerCard.TripsOnDisplay.LastOrDefault() : s2.name != _managerCard.Bots[s2.Row].LastOrDefault();
    }
}
