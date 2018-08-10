using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    class ShardingExecutionContext
    {
        public int ShardingCount { get; set; }
        public int ShardingIndex { get; set; }
    }
}
