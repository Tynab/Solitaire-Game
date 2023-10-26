using UnityEngine;

public sealed class Selectable : MonoBehaviour
{
    public bool FaceUp = default;
    public bool Top = default;
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
            Suit = transform.name[0].ToString();

            for (var i = 1; i < transform.name.Length; i++)
            {
                ValueString += transform.name[i].ToString();
            }

            switch (ValueString)
            {
                case "A":
                    {
                        Values = 1;
                        break;
                    }
                case "2":
                    {
                        Values = 2;
                        break;
                    }
                case "3":
                    {
                        Values = 3;
                        break;
                    }
                case "4":
                    {
                        Values = 4;
                        break;
                    }
                case "5":
                    {
                        Values = 5;
                        break;
                    }
                case "6":
                    {
                        Values = 6;
                        break;
                    }
                case "7":
                    {
                        Values = 7;
                        break;
                    }
                case "8":
                    {
                        Values = 8;
                        break;
                    }
                case "9":
                    {
                        Values = 9;
                        break;
                    }
                case "10":
                    {
                        Values = 10;
                        break;
                    }
                case "J":
                    {
                        Values = 11;
                        break;
                    }
                case "Q":
                    {
                        Values = 12;
                        break;
                    }
                case "K":
                    {
                        Values = 13;
                        break;
                    }
                default:
                    {
                        Values = default;
                        break;
                    }
            }
        }
    }
}
