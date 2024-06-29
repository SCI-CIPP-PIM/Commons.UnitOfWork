﻿using Commons.UnitOfWork.Scope;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;

namespace Commons.UnitOfWork.Scope
{
    internal class DefaultUnitOfWorkScope : IUnitOfWorkScope
    {
        private IUnitOfWork? unitOfWork;
        public IUnitOfWork Current
        {
            get => unitOfWork ?? throw new InvalidOperationException();
            internal set => unitOfWork = value;
        }

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    unitOfWork?.Dispose();
                }

                unitOfWork = null;
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (this.unitOfWork is IAsyncDisposable unitOfWork)
            {
                await unitOfWork.DisposeAsync();
                this.unitOfWork = null;
            };
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }
    }
}
