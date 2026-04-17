using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.interfaces
{
    public interface IChurchRepository
    {
        // MassTypye
        Task<int> CreateAsync(MassType massType);
        Task<int> DeleteMassType(int id);
        // Church
        Task<Church?> GetByIdAsync(int id);
        Task<IEnumerable<Church>> GetAllAsync();
        Task<int> CreateAsync(Church church);
        Task<int> UpdateAsync(Church church);


        // Dicosce
        Task<bool> GetDioceseExistsAsync(int dioceseId);
        Task<bool> UniqueNameDiocese(string name);
        Task<Diocese?> GetDioceseByIdAsync(int id);
        Task<IEnumerable<Diocese>> GetAllDiocesesAsync();
        Task<int> CreateAsync(Diocese diocese);
        Task<int> UpdateAsync(Diocese diocese);
        Task<int> DeleteDiocese(int id);
    }
}
