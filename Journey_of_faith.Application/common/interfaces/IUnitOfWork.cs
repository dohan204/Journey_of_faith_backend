using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.common.interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
