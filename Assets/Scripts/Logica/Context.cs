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
        public List<Card> otherhand{get;set;}
        public List<Card> deck {get;set;}
        public List<Card> otherdeck {get;set;}
        public List<Card> field{get;set;}
        public List<Card> otherfield{get;set;}
        public List<Card> board{get;set;}
        public List<Card> graveyard{get;set;}
        public List<Card> othergraveyard{get;set;}
    }
}