using System;
using System.Collections.Generic;
using System.Text;
using Hangfire;
using System.Linq.Expressions;
namespace NetCorePal.Hangfire.ShardingJobExtensions
{
    /// <summary>
    /// 分片job帮助类
    /// </summary>
    public static class ShardingJob
    {
        private const int MaxSharding = 100;
        /// <summary>
        /// 添加分片job
        /// </summary>
        /// <typeparam name="T">分片job实现类型</typeparam>
        /// <param name="name">job名称，会根据分片索引生成jobid，如：myjob_0，myjob_1</param>
        /// <param name="shardingCount"></param>
        /// <param name="factory"></param>
        /// <param name="cron"></param>
        /// <param name="timeZone"></param>
        /// <param name="queue"></param>
        public static void AddOrUpdate<T>(string name, int shardingCount, Func<T> factory, string cron, TimeZoneInfo timeZone = default(TimeZoneInfo), string queue = "default") where T : ShardingJobBase
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (shardingCount > MaxSharding)
            {
                throw new ArgumentOutOfRangeException(nameof(shardingCount), $"分片数不能大于{MaxSharding}");
            }

            for (int i = 0; i < shardingCount; i++)
            {
                RecurringJob.AddOrUpdate($"{name}_{i}", () => factory().Execute(new ShardingExecutionContext { ShardingCount = shardingCount, ShardingIndex = i }, TimeSpan.FromSeconds(20)), cron, timeZone, queue);
            }
            for (var i = shardingCount; i < MaxSharding; i++)
            {
                RecurringJob.RemoveIfExists($"{name}_{i}");
            }
        }
        /// <summary>
        /// 删除分片job
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveIfExists(string name)
        {
            for (int i = 0; i < MaxSharding; i++)
            {
                RecurringJob.RemoveIfExists($"{name}_{i}");
            }
        }

        /// <summary>
        /// 触发执行job，该方法会触发该名称job的所有分片任务
        /// </summary>
        /// <param name="name">job 名称</param>
        public static void Trigger(string name)
        {
            for (int i = 0; i < MaxSharding; i++)
            {
                RecurringJob.Trigger($"{name}_{i}");
            }
        }
    }
}
