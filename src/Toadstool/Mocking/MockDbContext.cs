using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Toadstool.Mocking
{
    public class MockDbContext : IDbContext, IDbTransactionWrapper, IDbConnectionProvider
    {
        private readonly Dictionary<string, IEnumerable<object>> _fakes;

        public MockDbContext()
        {
            _fakes = new Dictionary<string, IEnumerable<object>>();
        }

        public MockDbContext WithFakes(string commandText, IEnumerable<object> fakes)
        {
            _fakes[commandText] = fakes;
            return this;
        }

        public Task<IDbTransactionWrapper> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IDbTransactionWrapper)this);
        }

        public IDbCommandBuilder Command(string commandText)
        {
            IEnumerable<object> fakes = Enumerable.Empty<object>();
            _fakes.TryGetValue(commandText, out fakes);
            return new MockDbCommandBuilder()
                .WithFakes(fakes)
                .WithDbContext(this)
                .WithCommandText(commandText);
        }

        public void Commit()
        {
        }

        public void Dispose()
        {
        }

        public void Rollback()
        {
        }

        public Task<IDbConnectionWrapper> GetOpenConnectionAsync(CancellationToken cancellationToken)
        {
            return null;
        }
    }
}