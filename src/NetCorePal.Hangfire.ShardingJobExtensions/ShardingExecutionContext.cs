using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    /// <summary>
    /// sharding job execution context
    /// </summary>
    public class ShardingExecutionContext
    {
        /// <summary>
        /// total sharding count
        /// </summary>
        public int ShardingCount { get; set; }
        /// <summary>
        /// current sharding index
        /// </summary>
        public int ShardingIndex { get; set; }
    }
}
