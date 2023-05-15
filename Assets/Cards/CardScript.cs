using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public Image blankImage;

    public Sprite NormalCard;
    public Sprite HeavyCard;
    public Sprite LightCard;
    public Sprite WallCard;

    public int Slot;
    public int activeSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.showIndex() == Slot)
        {
            blankImage.enabled = false;
        }
        else if (Slot != 5)
        {
            blankImage.enabled = true;
            switch (GameManager.Instance.showCardSlot(Slot))
            {
                case 0:
                    blankImage.sprite = LightCard;
                    break;

                case 1:
                    blankImage.sprite = NormalCard;
                    break;

                case 2:
                    blankImage.sprite = HeavyCard;
                    break;

                case 3:
                    blankImage.sprite = WallCard;
                    break;
            }
        }
        else if (Slot == 5)
        {
            switch (GameManager.Instance.showCurrentCard())
            {
                case 0:
                    blankImage.sprite = HeavyCard;
                    break;

                case 1:
                    blankImage.sprite = NormalCard;
                    break;

                case 2:
                    blankImage.sprite = LightCard;
                    break;

                case 3:
                    blankImage.sprite = WallCard;
                    break;
            }
        }
        if(GameManager.Instance.CheckRoundEnded())
        {
            Slot = activeSlot;
        }
    }
}
