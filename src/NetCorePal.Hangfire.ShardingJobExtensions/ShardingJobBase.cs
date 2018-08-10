using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    /// <summary>
    /// 分片Job基类
    /// </summary>
    public abstract class ShardingJobBase
    {
        /// <summary>
        /// 执行job
        /// </summary>
        /// <param name="context"></param>
        /// <param name="timeout"></param>
        [SkipWhenPreviousJobIsRunning]
        public void Execute(ShardingExecutionContext context, TimeSpan timeout)
        {
            Stopwatch watch = Stopwatch.StartNew();
            while (watch.Elapsed < timeout && DoExecute(context)) ;
        }

        /// <summary>
        /// 执行job，实际job的内容，返回true表示可以继续重复执行
        /// </summary>
        /// <param name="context">job执行上下文</param>
        /// <returns></returns>
        protected abstract bool DoExecute(ShardingExecutionContext context);
    }
}
