using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Quaternion;

public class ManagerCard : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject[] PosTops;
    public GameObject[] PosBots;
    public Sprite[] FaceCard;

    public static string[] SetCard = new string[]
    {
        "H",
        "D",
        "C",
        "S"
    };

    public static string[] Values = new string[]
    {
        "A",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "J",
        "Q",
        "K"
    };

    public List<string> DeckCard = new();
    public List<string>[] Tops;
    public List<string>[] Bots;
    private List<string> _bot0 = new();
    private List<string> _bot1 = new();
    private List<string> _bot2 = new();
    private List<string> _bot3 = new();
    private List<string> _bot4 = new();
    private List<string> _bot5 = new();
    private List<string> _bot6 = new();

    private void Start()
    {
        Bots = new List<string>[]
        {
            _bot0,
            _bot1,
            _bot2,
            _bot3,
            _bot4,
            _bot5,
            _bot6
        };

        DealCard();
    }

    private void Update()
    {
    }

    public void DealCard()
    {
        DeckCard = GenerateDeckCard();
        ShuffleCard(DeckCard);
        DeckCard.ForEach(x => print(x));
        SolitaireSort();
        CreateDeckCard();
    }

    public void ShuffleCard(List<string> list)
    {
        var rnd = new System.Random();
        var n = list.Count;

        while (n > 1)
        {
            var k = rnd.Next(n);
            var temp = list[k];

            n--;
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public void CreateDeckCard()
    {
        for (var i = 0; i < 7; i++)
        {
            print(i);

            var ySet = 0f;
            var zSet = 0f;

            Bots[i].ForEach(x =>
            {
                var newCard = Instantiate(CardPrefab, new Vector3(PosBots[i].transform.position.x, transform.position.y - ySet, transform.position.z - zSet), identity, PosBots[i].transform);

                newCard.name = x;
                newCard.GetComponent<Selectable>().FaceUp = true;
                ySet += 0.1f;
                zSet += 0.03f;
            });
        }
    }

    private void SolitaireSort()
    {
        for (var i = 0; i < 7; i++)
        {
            for (var j = 0; j < 7; j++)
            {
                Bots[j].Add(DeckCard.LastOrDefault());
                DeckCard.RemoveAt(DeckCard.Count - 1);
            }
        }
    }

    public static List<string> GenerateDeckCard() => SetCard.SelectMany(x => Values.Select(y => x + y)).ToList();
}
