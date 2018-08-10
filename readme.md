# NetCorePal Hangfire Extensions

## NetCorePal.Hangfire.ShardingJobExtensions

Create sharding job
```
public class MyJob : ShardingJobBase
{
    protected override bool DoExecute(ShardingExecutionContext context)
    {
        //do your work

        return hasMoreWork;
    }
}
```

Manage sharding job
```
ShardingJob.AddOrUpdate("myjob", 7, () => new MyJob(), Cron.Minutely(), TimeZoneInfo.Local);

ShardingJob.RemoveIfExists("myjob");

ShardingJob.Trigger("myjob"); //trigger all sharding 

ShardingJob.Trigger("myjob", 1);  //trigger the sharding which index is 1
```