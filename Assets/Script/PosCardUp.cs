using System.Collections.Generic;
using UnityEngine;

public sealed class PosCardUp : MonoBehaviour
{
    public List<Transform> Card = new();
    public GameObject GlupCard;

    private void Update()
    {
        Card.Clear();

        var cardCount = GlupCard.transform.childCount;

        for (var i = 0; i < cardCount; i++)
        {
            Card.Add(GlupCard.transform.GetChild(i));
            Card[i].GetComponent<Selectable>().FaceUp = i == cardCount - 1;
        }
    }
}
