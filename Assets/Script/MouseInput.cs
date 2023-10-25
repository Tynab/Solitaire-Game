using UnityEngine;
using static UnityEngine.Camera;
using static UnityEngine.Input;
using static UnityEngine.Physics2D;
using static UnityEngine.Vector2;
using static UnityEngine.Time;

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
        _managerCard = FindObjectOfType<ManagerCard>();
        Slot = gameObject;
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
            _clickCount = 0;
        }

        if (_clickCount is 3)
        {
            _times = 0;
            _clickCount = 1;
        }

        GetMouseClick();
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

                    if(!selected.GetComponent<Selectable>().FaceUp)
                    {
                        if(!blo)
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

    private bool Blocked(GameObject selected)
    {
        var select=selected.GetComponent<Selectable>();

        if(select.i)
    }
}
