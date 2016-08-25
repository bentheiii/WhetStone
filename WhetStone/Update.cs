using System;
using LiteDB;
using WhetStone.SystemExtensions;

namespace WhetStone.Data
{
    public static class update
    {
        public static void Update<T>(this LiteCollection<T> @this, int id, Action<T> mutation) where T : new()
        {
            @this.Update(@this.FindById(id), mutation);
        }
        public static void Update<T>(this LiteCollection<T> @this, T e, Action<T> mutation) where T : new()
        {
            @this.Update(e.Mutate(mutation));
        }
    }
}
