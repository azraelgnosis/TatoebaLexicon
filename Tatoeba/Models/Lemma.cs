using System;
namespace Tatoeba.Models
{
    public class Lemma : IComparable
    {
        public int id { get; set; }
        public Language lang { get; set; }
        public string lexeme { get; set; }
        public string gloss { get; set; }

        public static Lemma FromLine(string line) {
            string[] arr = line.Split(',');
            Lemma lemma = new Lemma {
                id = int.Parse(arr[0]),
                lang = (Language) Enum.Parse(typeof(Language), arr[1]),
                lexeme = arr[2],
                gloss = arr[3]
            };
            return lemma;
        }

        public int CompareTo(object obj)
        {
            Lemma otherLemma = (Lemma)obj;
            return this.lexeme.CompareTo(otherLemma.lexeme);
        }

        public string ToLine() {
            return $"{id},{lang},{lexeme},{gloss}";
        }
    }
}