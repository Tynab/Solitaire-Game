using System.Linq;
using UnityEngine;

public sealed class Selectable : MonoBehaviour
{
    public bool FaceUp = default;
    public bool IsDeckPile = default;
    public int Values;
    public int Row;
    public string Suit;
    public string ValueString;

    private void Start() => CheckValue();

    private void CheckValue()
    {
        if (CompareTag("Card"))
        {
            Suit = transform.name[default].ToString();
            ValueString = string.Concat(transform.name.Skip(1));

            Values = ValueString switch
            {
                "A" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                "8" => 8,
                "9" => 9,
                "10" => 10,
                "J" => 11,
                "Q" => 12,
                "K" => 13,
                _ => default
            };
        }
    }
}
