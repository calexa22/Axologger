using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Axologger.Lib.Models;
namespace Tests
{
    public class AxoStoryTests
    {
        private AxoStory story;

        [SetUp]
        public void Setup()
        {
            story = new AxoStory(1)
            {
                Assignee = "Some.Developer+123@gmail.com",
                Release = "Year-Long Sprint 1"
            };
            story.RelateTasks(new List<AxoTask>
            {
                new AxoTask(3),
                new AxoTask(4)
            });
        }

        [Test]
        public void RelateTasks_AddsExpectedTasksToMap()
        {
            var relatedTasks = story.RelatedTasks.OrderBy(task => task.Id).ToList();

            Assert.That(relatedTasks.Count, Is.EqualTo(2));

            Assert.That(relatedTasks[0].Id, Is.EqualTo(3));
            Assert.That(relatedTasks[1].Id, Is.EqualTo(4));
        }

        [Test]
        public void RelateTasks_NewAddsContainsAlreadyRelatedStory_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                story.RelateTasks(new List<AxoTask> { new AxoTask(4) });
            });
        }

        [Test]
        public void RelateTasks_NewAddsHaveDuplicates_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                story.RelateTasks(new List<AxoTask> { new AxoTask(5), new AxoTask(5) });
            });
        }

        [Test]
        public void UnrelateTask_DeletesExpectedTaskFromMap()
        {
            var deletedTask = story.UnrelateTask(3);

            Assert.That(deletedTask.Id, Is.EqualTo(3));

            var relatedTask = story.RelatedTasks.Single();

            Assert.That(relatedTask.Id, Is.EqualTo(4));
        }

        [Test]
        public void UnrelateTask_TaskIsNotRelatedToStory_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                story.UnrelateTask(100);
            });
        }

        [Test]
        public void DeepCopy_ReturnsExpectedValues()
        {
            var copy = story.DeepCopy();

            Assert.That(copy.Id, Is.EqualTo(story.Id));
            Assert.That(copy.Assignee, Is.EqualTo(story.Assignee));
            Assert.That(copy.Release, Is.EqualTo(story.Release));

            var expectedTasks = story.RelatedTasks.OrderBy(task => task.Id).ToList();
            var actualTasks = copy.RelatedTasks.OrderBy(task => task.Id).ToList();

            CollectionAssert.AreEquivalent(expectedTasks.Select(task => task.Id), actualTasks.Select(task => task.Id));

            for (var i = 0; i < expectedTasks.Count; ++i)
            {
                Assert.That(ReferenceEquals(actualTasks[i], expectedTasks[i]), Is.False);
            }
        }
    }
}