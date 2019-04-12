using System;
using System.Collections.Generic;
using System.Text;

namespace Axologger.Lib.Models
{
    public enum UnitOfTimeEnum
    {
        Minutes = 1,
        Hours = 2,
        Days = 3
    }

    public class AxoWorklog : ICopyable<AxoWorklog>
    {
        public decimal LogValue { get; set; }
        public UnitOfTimeEnum? UnitOfTime { get; set; }

        public AxoWorklog DeepCopy()
        {
            return new AxoWorklog
            {
                LogValue = this.LogValue,
                UnitOfTime = this.UnitOfTime
            };
        }
    }
}
