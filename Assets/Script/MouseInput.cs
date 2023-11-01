using System.Collections;
using UnityEngine;
using static UnityEngine.Camera;
using static UnityEngine.Input;
using static UnityEngine.Physics2D;
using static UnityEngine.Time;
using static UnityEngine.Vector2;

public sealed class MouseInput : MonoBehaviour
{
    public GameObject Slot;
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

        _ = StartCoroutine(GetMouseClick());
    }

    public bool StackAble(GameObject selected)
    {
        var s1 = Slot.GetComponent<Selectable>();
        var s2 = selected.GetComponent<Selectable>();

        return !s2.IsDeckPile && s1.Values == s2.Values - 1 && s1.Suit is "D" or "H" != s2.Suit is "D" or "H";
    }

    private IEnumerator GetMouseClick()
    {
        if (GetMouseButtonDown(default))
        {
            var hit = Raycast(main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1f)), zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    yield return _managerCard.DealFromDeck();

                    Slot = gameObject;
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    var selected = hit.collider.gameObject;
                    var s2 = selected.GetComponent<Selectable>();

                    if (s2.FaceUp)
                    {
                        if (s2.IsDeckPile || Slot == gameObject)
                        {
                            Slot = selected;
                        }
                        else if (Slot != selected)
                        {
                            if (StackAble(selected))
                            {
                                Stack(selected, false);
                            }
                            else
                            {
                                Slot = selected;
                            }
                        }
                    }
                }
                else if ((hit.collider.CompareTag("PosTop") || hit.collider.CompareTag("PosBot")) && Slot.CompareTag("Card"))
                {
                    Stack(hit.collider.gameObject, true);
                }
            }
        }
    }

    private void Stack(GameObject selected, bool isPos)
    {
        var s1 = Slot.GetComponent<Selectable>();
        var s2 = selected.GetComponent<Selectable>();
        var yPos = isPos ? 0f : 0.9f;
        var zPos = 0.01f;

        Slot.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yPos, selected.transform.position.z - zPos);
        Slot.transform.parent = selected.transform;
        _ = s1.IsDeckPile ? _managerCard.TripsOnDisplay.Remove(Slot.name) : _managerCard.Bots[s1.Row].Remove(Slot.name);
        s1.IsDeckPile = default;
        s1.Row = s2.Row;
        Slot = gameObject;
    }
}
