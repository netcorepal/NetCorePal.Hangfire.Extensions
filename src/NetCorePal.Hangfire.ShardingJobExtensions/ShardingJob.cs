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
        /// Add sharding job to hangfrie as RecurringJob
        /// </summary>
        /// <typeparam name="T">typeof sharding job which inherit <see cref="ShardingJobBase"/> </typeparam>
        /// <param name="name">job name,will be recurringJobId as: myjob_0，myjob_1</param>
        /// <param name="shardingCount">the total sharding count</param>
        /// <param name="factory">job factory</param>
        /// <param name="cron">cron for job</param>
        /// <param name="timeZone">timeZoneInfo</param>
        /// <param name="queue">queue name</param>
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
        /// Remove sharding job, will remove all RecurringJob that we added
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
        /// Trigger sharding job, trigger all RecurringJob we added
        /// </summary>
        /// <param name="name">job name</param>
        public static void Trigger(string name)
        {
            for (int i = 0; i < MaxSharding; i++)
            {
                RecurringJob.Trigger($"{name}_{i}");
            }
        }
        /// <summary>
        /// Trigger RecurringJob which sharding index Equals shardingIndex
        /// </summary>
        /// <param name="name">job name</param>
        /// <param name="shardingIndex">the sharding index we want trigger</param>
        public static void Trigger(string name, int shardingIndex)
        {
            RecurringJob.Trigger($"{name}_{shardingIndex}");
        }
    }
}
