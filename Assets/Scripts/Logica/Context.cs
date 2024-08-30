using System.Globalization;
using System.Runtime.InteropServices;
using System.Dynamic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GameLogic
{
    public class Context
    {
        public List<Card> hand{get;set;}
        public List<Card> otherHand{get;set;}
        public List<Card> deck {get;set;}
        public List<Card> otherDeck {get;set;}
        public List<Card> field{get;set;}
        public List<Card> otherField{get;set;}
        public List<Card> board{get;set;}
        public List<Card> graveyard{get;set;}
        public List<Card> otherGraveyard{get;set;}
    }
}