using Stats.Model;
using System;
using System.Threading.Tasks;


namespace Stats.Repositories
{
    public interface IJournalRepository
    {
        Task<Journal> AddEventToDay(Activity activity, DateTime day);

        Task<Journal> ReadDay(DateTime day);
    }
}
