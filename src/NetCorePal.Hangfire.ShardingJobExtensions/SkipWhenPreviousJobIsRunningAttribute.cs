using Hangfire.Client;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    /// <summary>
    /// from odinserj's https://gist.github.com/odinserj/a6ad7ba6686076c9b9b2e03fcf6bf74e
    /// </summary>
    class SkipWhenPreviousJobIsRunningAttribute : JobFilterAttribute, IClientFilter, IApplyStateFilter
    {
        public void OnCreating(CreatingContext context)
        {
            // We can't handle old storages
            if (!(context.Connection is JobStorageConnection connection)) return;

            // We should run this filter only for background jobs based on 
            // recurring ones
            if (!context.Parameters.ContainsKey("RecurringJobId")) return;

            var recurringJobId = context.Parameters["RecurringJobId"] as string;

            // RecurringJobId is malformed. This should not happen, but anyway.
            if (String.IsNullOrWhiteSpace(recurringJobId)) return;

            var running = connection.GetValueFromHash($"recurring-job:{recurringJobId}", "Running");
            if ("yes".Equals(running, StringComparison.OrdinalIgnoreCase))
            {
                context.Canceled = true;
            }
        }

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState is EnqueuedState)
            {
                var recurringJobId = JobHelper.FromJson<string>(context.Connection.GetJobParameter(context.BackgroundJob.Id, "RecurringJobId"));
                if (String.IsNullOrWhiteSpace(recurringJobId)) return;

                transaction.SetRangeInHash(
                    $"recurring-job:{recurringJobId}",
                    new[] { new KeyValuePair<string, string>("Running", "yes") });
            }
            else if (context.NewState.IsFinal)
            {
                var recurringJobId = JobHelper.FromJson<string>(context.Connection.GetJobParameter(context.BackgroundJob.Id, "RecurringJobId"));
                if (String.IsNullOrWhiteSpace(recurringJobId)) return;

                transaction.SetRangeInHash(
                    $"recurring-job:{recurringJobId}",
                    new[] { new KeyValuePair<string, string>("Running", "no") });
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}