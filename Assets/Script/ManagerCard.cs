using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Quaternion;
using static System.Linq.Enumerable;

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
    public List<string> TripsOnDisplay = new();
    public List<string> DeckCard = new();
    public List<string> DisCardPile = new();
    public List<string>[] Tops;
    public List<string>[] Bots;
    private readonly List<string> _bot0 = new();
    private readonly List<string> _bot1 = new();
    private readonly List<string> _bot2 = new();
    private readonly List<string> _bot3 = new();
    private readonly List<string> _bot4 = new();
    private readonly List<string> _bot5 = new();
    private readonly List<string> _bot6 = new();
    private int _trips;
    private int _tripsRemainder;
    private int _deckLocation;

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
        SolitaireSort();
        _ = StartCoroutine(CreateDeckCard());
        SortDeckIntoTrips();
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

    public static List<string> GenerateDeckCard() => SetCard.SelectMany(x => Values.Select(y => x + y)).ToList();

    public void DealFromDeck()
    {
        if (_deckLocation < _trips)
        {
            TripsOnDisplay.Clear();

            var xPos = 2.5f;
            var zPos = 0.2f;

            DeckTrips[_deckLocation].ForEach(x =>
            {
                var newTopCard = Instantiate(CardPrefab, new Vector3(DeckButton.transform.position.x + xPos, DeckButton.transform.position.y, DeckButton.transform.position.z + zPos), identity, DeckButton.transform);

                xPos += 0.5f;
                newTopCard.name = x;
                TripsOnDisplay.Add(x);
                newTopCard.GetComponent<Selectable>().FaceUp = true;
            });
        }
        else
        {
            RestacktopDeck();
        }
    }

    private IEnumerator CreateDeckCard()
    {
        for (var i = 0; i < 7; i++)
        {
            var ySet = 0f;
            var zSet = 0f;

            foreach (var cardName in Bots[i])
            {
                yield return new WaitForSeconds(0.01f);

                var newCard = Instantiate(CardPrefab, new Vector3(PosBots[i].transform.position.x, PosBots[i].transform.position.y - ySet, transform.position.z - zSet), identity, PosBots[i].transform);

                newCard.name = cardName;

                if (Bots[i][^1] == cardName)
                {
                    newCard.GetComponent<Selectable>().FaceUp = true;
                }

                ySet += 0.2f;
                zSet += 0.05f;
            }
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

    private void SortDeckIntoTrips()
    {
        _trips = DeckCard.Count / 3;
        _tripsRemainder = DeckCard.Count % 3;
        DeckTrips.Clear();

        var index = 0;

        for (var i = 0; i < _trips; i++)
        {
            DeckTrips.Add(Range(0, 3).Select(x => DeckCard[x + index]).ToList());
            index += 3;
        }

        if (_tripsRemainder is not 0)
        {
            var myRemainders = new List<string>();

            index = 0;

            for (var i = 0; i < _tripsRemainder; i++)
            {
                myRemainders.Add(DeckCard[DeckCard.Count - _tripsRemainder + index]);
                index++;
            }

            DeckTrips.Add(myRemainders);
            _trips++;
        }

        _deckLocation = 0;
    }

    private void RestacktopDeck()
    {
        DeckCard.Clear();
        DisCardPile.ForEach(x => DeckCard.Add(x));
        DisCardPile.Clear();
        SortDeckIntoTrips();
    }
}
