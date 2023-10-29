using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Linq.Enumerable;
using static UnityEngine.Quaternion;

public sealed class ManagerCard : MonoBehaviour
{
    public GameObject[] PosTops;
    public GameObject[] PosBots;
    public Sprite[] FaceCard;
    public GameObject CardPrefab;
    public GameObject DeckButton;

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

    public List<List<string>> DeckTrips = new();
    public List<string>[] Tops;
    public List<string>[] Bots;
    public List<string> TripsOnDisplay = new();
    public List<string> DeckCard = new();
    public List<string> DisCardPile = new();
    private readonly List<string> _bot0 = new();
    private readonly List<string> _bot1 = new();
    private readonly List<string> _bot2 = new();
    private readonly List<string> _bot3 = new();
    private readonly List<string> _bot4 = new();
    private readonly List<string> _bot5 = new();
    private readonly List<string> _bot6 = new();
    private int _deckLocation;
    private int _trips;
    private int _tripsRemainder;

    private void Start() => Bots = new List<string>[]
    {
        _bot0,
        _bot1,
        _bot2,
        _bot3,
        _bot4,
        _bot5,
        _bot6
    };

    public void DealCard()
    {
        foreach (var bot in Bots)
        {
            bot.Clear();
        }

        DeckCard = GenerateDeckCard();
        ShuffleCard(DeckCard);
        SolitaireSort();
        _ = StartCoroutine(CreateDeckCard());
        SortDeckIntoTrips();
    }

    public void ShuffleCard(List<string> list)
    {
        var random = new System.Random();
        var n = list.Count;

        while (n > 1)
        {
            var k = random.Next(n);

            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static List<string> GenerateDeckCard() => SetCard.SelectMany(x => Values.Select(y => x + y)).ToList();

    public void DealFromDeck()
    {
        foreach (Transform nameCard in DeckButton.transform)
        {
            if (nameCard.CompareTag("Card"))
            {
                _ = DeckCard.Remove(nameCard.name);
                DisCardPile.Add(nameCard.name);
                Destroy(nameCard.gameObject);
            }
        }

        if (_deckLocation < _trips)
        {
            TripsOnDisplay.Clear();

            var xPos = 6f;
            var zPos = 0f;

            DeckTrips[_deckLocation].ForEach(x =>
            {
                TripsOnDisplay.Add(x);

                var newTopCard = Instantiate(CardPrefab, new Vector3(DeckButton.transform.position.x + xPos, DeckButton.transform.position.y, DeckButton.transform.position.z + zPos), identity, DeckButton.transform);

                newTopCard.name = x;

                var s = newTopCard.GetComponent<Selectable>();

                s.FaceUp = true;
                s.IsDeckPile = true;
                xPos += 1f;
                zPos -= 0.1f;
            });

            _deckLocation++;
        }
        else
        {
            RestackTopDeck();
        }
    }

    private IEnumerator CreateDeckCard()
    {
        for (var i = 0; i < 7; i++)
        {
            var ySet = 0f;
            var zSet = 0.01f;

            foreach (var cardName in Bots[i])
            {
                yield return new WaitForSeconds(0.01f);

                var newCard = Instantiate(CardPrefab, new Vector3(PosBots[i].transform.position.x, PosBots[i].transform.position.y - ySet, PosBots[i].transform.position.z - zSet), identity, PosBots[i].transform);

                newCard.name = cardName;

                var s = newCard.GetComponent<Selectable>();

                s.Row = i;

                if (cardName == Bots[i][^1])
                {
                    s.FaceUp = true;
                }

                DisCardPile.Add(cardName);
                ySet += 0.2f;
                zSet += 0.1f;
            }

            DisCardPile.ForEach(x =>
            {
                if (DeckCard.Contains(x))
                {
                    _ = DeckCard.Remove(x);
                }
            });

            DisCardPile.Clear();
            FindObjectOfType<ManagerGame>().IsPlay = true;
        }
    }

    private void SolitaireSort()
    {
        for (var i = 0; i < 7; i++)
        {
            for (var j = i; j < 7; j++)
            {
                Bots[j].Add(DeckCard.LastOrDefault());
                DeckCard.RemoveAt(DeckCard.Count - 1);
            }
        }
    }

    private void SortDeckIntoTrips()
    {
        _trips = DeckCard.Count / 3;
        _tripsRemainder = DeckCard.Count % 3;
        DeckTrips.Clear();

        var index = 0;

        for (var i = 0; i < _trips; i++)
        {
            DeckTrips.Add(Range(default, 3).Select(x => DeckCard[x + index]).ToList());
            index += 3;
        }

        if (_tripsRemainder is not 0)
        {
            DeckTrips.Add(Range(default, _tripsRemainder).Select(x => DeckCard[DeckCard.Count - _tripsRemainder + x]).ToList());
            _trips++;
        }

        _deckLocation = default;
    }

    private void RestackTopDeck()
    {
        DeckCard.Clear();
        DeckCard.AddRange(DisCardPile);
        DisCardPile.Clear();
        SortDeckIntoTrips();
    }
}
