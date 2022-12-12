using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GamePanelImge : MonoBehaviour
{
    public Image gameCard1;
    public Image gameCard2;
    public Image gameCard3;
    public Image gameCard4;
    public Image gameCard5;
    public Image gameCard6;
    public Image gameCard7;
    public Image gameCard8;

    public int[] orders = new int[8];

    public Sprite[] gameCard = new Sprite[4];

    public System.Random rand = new System.Random();


    private void GamecardOpen()
    {
        
        for (int i = 0; i <= 7; i++)
        {
            int temp = rand.Next(0, 7 - i);
            for (int j = 0; j <= temp; j++)
            {
                if (orders[j] != 0)
                    temp++;
            }
            orders[temp] = i + 1;
        }

        for (int i = 0; i < 8; i++)
        {
            int cardNum = (orders[i] - 1) / 2;
            //gameCard[i].gameCard.(false);
            //gameCard[i].thisCardImage.CardSetter(cardNum);
            //gameCard[i].cardNum = cardNum;
        }


    }
}
