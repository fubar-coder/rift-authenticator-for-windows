using System;
using System.Diagnostics;

namespace RiftAuthenticator.Library.Helpers
{
    internal partial class AsyncResult<TResult> : AsyncResultNoResult
    {
        // Field set when operation completes
        private TResult m_result = default(TResult);

        protected void SetResult(TResult result)
        {
            m_result = result;
        }

        protected AsyncResult(
            AsyncCallback asyncCallback,
            object state,
            object owner,
            string operationId) :
            base(asyncCallback, state, owner, operationId)
        {
        }

        new public static TResult End(
            IAsyncResult result, object owner, string operationId)
        {
            AsyncResult<TResult> asyncResult = result as AsyncResult<TResult>;
            Debug.Assert(asyncResult != null);

            // Wait until operation has completed
            AsyncResultNoResult.End(result, owner, operationId);

            // Return the result (if above didn't throw)
            return asyncResult.m_result;
        }
    }
}
