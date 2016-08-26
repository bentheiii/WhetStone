using System;
using System.Text.RegularExpressions;
using WhetStone.Funnels;

namespace WhetStone.WordPlay.Parsing
{
    public class Parser<T> : IProccesor<string, T>
    {
        private readonly string _query;
        private readonly Func<Match, T> _converter;
        public Parser(string q, Func<Match, T> c)
        {
            this._query = q;
            this._converter = c;
        }
        public bool TryParse(string s, out T u)
        {
            Match m = Regex.Match(s, this._query);
            u = m.Success ? this._converter(m) : default(T);
            return m.Success;
        }
        public Proccesor<string, T> toProcessor()
        {
            return TryParse;
        }
    }
}