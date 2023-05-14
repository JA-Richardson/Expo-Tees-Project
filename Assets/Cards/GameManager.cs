using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int howManyCards = 3;

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

    public void nextCard()
    {
        if (currentCardHeld < handOfCards.Count - 1)
        {
            currentCardHeld++;
        }
    }

    public void previousCard()
    {
        if (currentCardHeld > 0)
        {
            currentCardHeld--;
        }
    }

    public void useCard(int slot)
    {
        handOfCards.RemoveAt(slot);
        handOfCards.Insert(slot, Random.Range(0, 3));
    }

    public void removeSlot()
    {

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
            handOfCards.Add(Random.Range(0, 3));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
