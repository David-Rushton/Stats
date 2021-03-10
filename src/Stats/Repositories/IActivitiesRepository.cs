using Stats.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Stats.Repositories
{
    public interface IActivitiesRepository
    {
        Task<List<Activity>> GetActivities();
    }
}
