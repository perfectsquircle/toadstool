using System;
using System.Data;
using System.Threading.Tasks;

namespace Toadstool
{
    public interface IDbConnectionWrapper : IDisposable
    {
        IDbConnection DbConnection { get; }
        IDbTransaction DbTransaction { get; }
        IDbCommand CreateCommand();
        CommandBehavior CommandBehavior { get; }
    }
}