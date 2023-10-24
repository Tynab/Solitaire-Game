using UnityEngine;
using static UnityEngine.Camera;
using static UnityEngine.Input;
using static UnityEngine.Physics2D;
using static UnityEngine.Vector2;

public sealed class MouseInput : MonoBehaviour
{
    private ManagerCard _managerCard;

    private void Start() => _managerCard = FindObjectOfType<ManagerCard>();

    private void Update() => GetMouseClick();

    private void GetMouseClick()
    {
        if (GetMouseButtonDown(0))
        {
            var hit = Raycast(main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f)), zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    print($"Deck: {hit.collider.name}");
                    _managerCard.DealFromDeck();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    print($"Card: {hit.collider.name}");
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
}
