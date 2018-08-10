using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    /// <summary>
    /// 分片job执行上下文
    /// </summary>
    public class ShardingExecutionContext
    {
        /// <summary>
        /// 分片总数
        /// </summary>
        public int ShardingCount { get; set; }
        /// <summary>
        /// 当前分片索引
        /// </summary>
        public int ShardingIndex { get; set; }
    }
}
