using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Axologger.Lib.Models
{
    public class AxoStory : ICopyable<AxoStory>
    {
        private readonly Dictionary<int, AxoTask> _taskMap = new Dictionary<int, AxoTask>();

        public AxoStory (int id)
        {
            Id = id;
        }

        public int Id { get; }
        public string Assignee { get; set; }
        public string Release { get; set; }
        public IEnumerable<AxoTask> RelatedTasks => _taskMap.Values;

        public void RelateTasks(List<AxoTask> tasks)
        {
            var taskIds = tasks.Select(t => t.Id).ToList();
            if (taskIds.Count != taskIds.Distinct().ToList().Count)
                throw new InvalidOperationException("Duplicate tasks found in list");

            foreach (var task in tasks)
            {
                if (_taskMap.ContainsKey(task.Id))
                {
                    throw new InvalidOperationException($"Task {{{task.Id}}} has already been related to story {{{Id}}}");
                }
            }

            foreach (var task in tasks)
            {
                _taskMap[task.Id] = task;
            }
        }

        public AxoTask UnrelateTask(int taskId)
        {
            if (!_taskMap.ContainsKey(taskId))
            {
                throw new InvalidOperationException($"Task {{{taskId}}} is not related to story {{{Id}}}");
            }

            var task = _taskMap[taskId];
            _taskMap.Remove(taskId);
            return task;
        }

        public AxoStory DeepCopy()
        {
            var copy = new AxoStory(this.Id)
            {
                Assignee = this.Assignee,
                Release = this.Release
            };

            copy.RelateTasks(this._taskMap.Values.Select(task => task.DeepCopy()).ToList());

            return copy;
        }
    }
}
