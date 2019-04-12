using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Axologger.Lib.Models
{
    public enum TaskTypeEnum
    {
        DevImplement = 1,
        DevPeerReview = 2,
        PoProductOwnerVerify = 3,
        QaWriteTestCases = 4,
        QaExecuteTestCases = 5,
        TwDocument = 6
    }

    public class AxoTask : ICopyable<AxoTask>
    {
        private readonly List<AxoWorklog> _worklogs = new List<AxoWorklog>();

        public AxoTask(int id)
        {
            Id = id;
        }

        public IEnumerable<AxoWorklog> WorkLogs => _worklogs;
        public int Id { get; }
        public TaskTypeEnum TaskType { get; set; }

        public AxoTask DeepCopy()
        {
            var copy = new AxoTask(this.Id)
            {
                TaskType = this.TaskType
            };

            copy.AddWorklogs(this._worklogs.Select(log => log.DeepCopy()));
            return copy;
        }

        public void AddWorklogs(IEnumerable<AxoWorklog> worklogs)
        {
            _worklogs.AddRange(worklogs);
        }
    }
}
