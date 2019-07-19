using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Toadstool.Mocking
{
    internal class MockDbCommandBuilder : DbCommandBuilder
    {
        private IEnumerable<object> _fakes;

        public MockDbCommandBuilder()
        {
        }

        public MockDbCommandBuilder WithFakes(IEnumerable<object> fakes)
        {
            _fakes = fakes;
            return this;
        }

        protected override Task<TReturn> WithReader<TReturn>(Func<IDataReader, TReturn> callback, CancellationToken cancellationToken)
        {
            return Task.FromResult(callback.Invoke(null));
        }

        protected override IEnumerable<T> ToEnumerable<T>(IDataReader dataReader)
        {
            return _fakes.Cast<T>();
        }
    }
}