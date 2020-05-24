using System;

namespace Androtomist.Models.Database.Inserters
{
    public abstract class InserterAbstract : DBClass
    {
        protected DateTime dateNow = DateTime.Now;

        abstract public long Insert(bool is_insert = false);
    }
}
