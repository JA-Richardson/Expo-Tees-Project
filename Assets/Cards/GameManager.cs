using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int BaseMaxHealth = 100;
    public int BaseHealth = 100;
    public int howManyCards = 3;
    public TextMeshProUGUI waveNumber;
    public int roundNumber = 1;
    public bool noCardsLeft = false;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager is Null!");

            return _instance;
        }
    }

    private List<int> handOfCards = new List<int>();

    private int currentCardHeld = 0;

    private void Awake()
    {
        _instance = this;
    }


    public void useCard(int slot)
    {
        handOfCards.RemoveAt(slot);
        handOfCards.Insert(slot, Random.Range(0, 3));       
    }

    public int showCurrentCard()
    {
        switch (handOfCards[currentCardHeld])
        {
            case 0:
                return 0;
                break;

            case 1:
                return 1;
                break;

            case 2:
                return 2;
                break;

            case 3:
                return 3;
                break;

            default:
                return 10;
        }
    }

    public int showCardSlot(int index)
    {
        switch (handOfCards[index])
        {
            case 0:
                return 0;
                break;

            case 1:
                return 1;
                break;

            case 2:
                return 2;
                break;

            case 3:
                return 3;
                break;

            default:
                return 10;
        }
    }

    public int showIndex()
    {
        return currentCardHeld;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            handOfCards.Add(Random.Range(0, 4));
        }
    }

    public void NewCards()
    {
        for (int i = 0; i < 5; i++)
        {
            handOfCards.Add(Random.Range(0, 4));
        }
    }


    // Update is called once per frame
    void Update()
    {
        waveNumber.text = roundNumber.ToString();
    }


    public bool CheckRoundEnded()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
