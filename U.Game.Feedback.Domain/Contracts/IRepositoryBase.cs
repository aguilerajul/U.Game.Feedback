using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.Game.Feedback.Domain.Entities;

namespace U.Game.Feedback.Domain.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync(int totalRecords = 15);

        Task<IEnumerable<T>> GetFilteredListAsync(Func<T, bool> filter, int? totalRecords = 15);

        Task<T> GetFilteredAsync(Func<T, bool> filter);

        T Get(Guid id);

        Task<ActionResultMessage> AddAsync(T data);
    }
}
