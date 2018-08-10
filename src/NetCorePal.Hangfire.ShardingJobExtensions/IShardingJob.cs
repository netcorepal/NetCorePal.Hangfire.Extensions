using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    public interface IShardingJob
    {
        bool Execute(ShardingExecutionContext context);
    }
}
