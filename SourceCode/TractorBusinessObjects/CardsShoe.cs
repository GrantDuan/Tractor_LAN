using System;

namespace Duan.Xiugang.Tractor.Objects
{
    public class CardsShoe
    {
        public int[] Cards = null;
        private int _deckNumber;

        public CardsShoe()
        {
            _deckNumber = 2;
            Cards = new int[54*DeckNumber];
            FillNewCards();
        }

        public int DeckNumber
        {
            get { return _deckNumber; }
            set
            {
                _deckNumber = value;
                Cards = new int[54*DeckNumber];
                FillNewCards();
            }
        }


        public void FillNewCards()
        {
            for (int i = 0; i < DeckNumber; i++)
            {
                for (int j = 0; j < 54; j++)
                {
                    Cards[i*54 + j] = j;
                }
            }
        }

        //Knuth shuffle
        public void Shuffle()
        {
            int N = Cards.Length;
            for (int i = 0; i < N; i++)
            {
                int r = new Random().Next(i + 1);
                int temp = Cards[r];
                Cards[r] = Cards[i];
                Cards[i] = temp;
            }
        }
    }
}