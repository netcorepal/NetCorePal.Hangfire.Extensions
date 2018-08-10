using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCorePal.Hangfire.ShardingJobExtensions;
namespace NetCorePal.Hangfire.Web.Jobs
{
    public class AbcJob : ShardingJobBase
    {
        protected override bool DoExecute(ShardingExecutionContext context)
        {
            return true;
        }
    }
}
