using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace GameJAM.Scripts.Gameplay
{
    public class Itens
    {
        private string Name;
        private float Price;
        private ItensType TypeItem;

        public Itens(string Name, float Price, IntensType TypeItem)
        {
            this.Name = Name;
            this.Price = Price;
            this.TypeItem = TypeItem;
        }

        //Tirou o joker é vitoria garantida fih
        public int AplicarEfeitoJoker()
        {
            return 21;
        }
    }
}