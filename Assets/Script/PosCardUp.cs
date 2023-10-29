using System.Collections.Generic;
using UnityEngine;

public sealed class PosCardUp : MonoBehaviour
{
    public List<Transform> Card = new();
    public GameObject GlupCard;

    private void Update()
    {
        Card.Clear();

        foreach (Transform item in GlupCard.transform)
        {
            Card.Add(item);
        }

        if (Card.Count > 0)
        {
            for (var i = 0; i < Card.Count; i++)
            {
                Card[i].GetComponent<Selectable>().FaceUp = i == Card.Count - 1;
            }
        }
    }
}
