﻿using AutoTest.Core.Configuration;
using AutoTest.Core.Messaging;
using NUnit.Framework;
using AutoTest.Core.Caching.Projects;
using AutoTest.Core.Caching;
using AutoTest.Core.TestRunners;
using System;
using AutoTest.Core.FileSystem;
using AutoTest.Core.FileSystem.ProjectLocators;
using AutoTest.Core.Presenters;
using AutoTest.Core.Launchers;
using AutoTest.Core.DebugLog;
using AutoTest.Core.Messaging.MessageConsumers;
using AutoTest.Core.BuildRunners;
using AutoTest.Core.Caching.RunResultCache;
using AutoTest.Core.Notifiers;

namespace AutoTest.Test.Core.Configuration
{
    [TestFixture]
    public class DIContainerTests
    {
        private DIContainer _container;
        private IServiceLocator _locator;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _container = new DIContainer();
            _container.Configure();
            _locator = _container.Services;
        }

        [Test]
        public void Should_be_able_to_get_service_locator_from_container()
        {
            _locator.Locate<IServiceLocator>().ShouldBeOfType<IServiceLocator>();
        }

        [Test]
        public void Should_initialize_setting_service()
        {
            _locator.Locate<IConfiguration>().ShouldBeOfType<IConfiguration>();
        }


        [Test]
        public void Should_register_messaging_module()
        {
            var bus = _locator.Locate<IMessageBus>();
            bus.ShouldBeOfType<IMessageBus>();
            _locator.Locate<IMessageBus>().ShouldBeTheSameAs(bus);
        }

        [Test]
        public void Should_register_project_parser()
        {
            var parser = _locator.Locate<IProjectParser>();
            parser.ShouldBeOfType<IProjectParser>();
        }

        [Test]
        public void Should_register_project_factory()
        {
            var factory = _locator.Locate<ICreate<Project>>();
            factory.ShouldBeOfType<ICreate<Project>>();
        }

        [Test]
        public void Should_register_project_preparer()
        {
            var preparer = _locator.Locate<IPrepare<Project>>();
            var preparer2 = _locator.Locate<IPrepare<Project>>();
            preparer.ShouldBeOfType<IPrepare<Project>>();
            preparer.ShouldBeTheSameAs(preparer2);
        }

        [Test]
        public void Should_register_cache()
        {
            var cache = _locator.Locate<ICache>();
            cache.ShouldBeOfType<ICache>();
        }

        [Test]
        public void Should_register_cache_as_singleton()
        {
            var cache = _locator.Locate<ICache>();
            var cache2 = _locator.Locate<ICache>();
            cache.ShouldBeTheSameAs(cache2);
        }

        [Test]
        public void Should_register_build_locator()
        {
            var buildLocator = _locator.Locate<IBlockingConsumerOf<ProjectChangeMessage>>();
            buildLocator.ShouldBeOfType<IBlockingConsumerOf<ProjectChangeMessage>>();
        }

        [Test]
        public void Should_bind_consumer_of_file_change_message()
        {
            var filChangeHandler = _locator.Locate<IConsumerOf<FileChangeMessage>>("MSBuild");
            filChangeHandler.ShouldBeOfType<FileChangeConsumer>();
        }
		
		[Test]
        public void Should_bind_consumer_of_binary_file_change_message()
        {
            var filChangeHandler = _locator.Locate<IConsumerOf<FileChangeMessage>>("NoBuild");
            filChangeHandler.ShouldBeOfType<BinaryFileChangeConsumer>();
        }

        [Test]
        public void Should_register_file_system_service()
        {
            var service = _locator.Locate<IFileSystemService>();
            service.ShouldBeOfType<IFileSystemService>();
        }

        [Test]
        public void Should_register_watch_validator()
        {
            var validator = _locator.Locate<IWatchValidator>();
            validator.ShouldBeOfType<IWatchValidator>();
        }

        [Test]
        public void Should_register_project_locators()
        {
            var locators = _locator.LocateAll<ILocateProjects>();
            locators.Length.ShouldEqual(2);
        }

        [Test]
        public void Should_register_information_feedback_presenter()
        {
            var presenter = _locator.Locate<IInformationFeedbackPresenter>();
            presenter.ShouldBeOfType<IInformationFeedbackPresenter>();
        }

        [Test]
        public void Should_register_run_feedback_presenter()
        {
            var presenter = _locator.Locate<IRunFeedbackPresenter>();
            presenter.ShouldBeOfType<IRunFeedbackPresenter>();
        }

        [Test]
        public void Should_register_application_launcher()
        {
            var launcher = _locator.Locate<ApplicatonLauncher>();
            launcher.ShouldBeOfType<ApplicatonLauncher>();
        }

        [Test]
        public void Should_register_project_dirtifier()
        {
            var reloader = _locator.Locate<IReload<Project>>();
            reloader.ShouldBeOfType<IReload<Project>>();
        }

        [Test]
        public void Should_register_reference_prioritizer()
        {
            var prioritizer = _locator.Locate<IPrioritizeProjects>();
            prioritizer.ShouldBeOfType<IPrioritizeProjects>();
        }

        [Test]
        public void Should_register_build_runner()
        {
            var buildRunner = _locator.Locate<IBuildRunner>();
            buildRunner.ShouldBeOfType<MSBuildRunner>();
        }

        [Test]
        public void Should_register_test_runners()
        {
            var testRunners = _locator.LocateAll<ITestRunner>();
            testRunners.Length.ShouldEqual(3);
        }

        [Test]
        public void Should_register_build_list_generator()
        {
            var generator = _locator.Locate<IGenerateBuildList>();
            generator.ShouldBeOfType<IGenerateBuildList>();
        }

        [Test]
        public void Should_register_run_result_merger()
        {
            var merger = _locator.Locate<IMergeRunResults>();
            merger.ShouldBeOfType<IMergeRunResults>();
        }

        [Test]
        public void Should_register_run_result_cache()
        {
            var cache = _locator.Locate<IRunResultCache>();
            cache.ShouldBeOfType<IRunResultCache>();
        }
		
		[Test]
		public void Should_only_register_one_notifiers()
		{
			var notifiers = _locator.LocateAll<ISendNotifications>();
			notifiers.Length.ShouldEqual(1);
		}
		
		[Test]
		public void Should_register_notify_send_notifier_if_available()
		{
			var isSupported = (new notify_sendNotifier()).IsSupported();
			var notifier = _locator.Locate<ISendNotifications>();
			if (isSupported)
				notifier.ShouldBeOfType<notify_sendNotifier>();
			else
				notifier.ShouldNotBeOfType<notify_sendNotifier>();
		}
		
		[Test]
		public void Should_register_null_notifier_if_nothing_is_available()
		{
            var isSupported = (new notify_sendNotifier()).IsSupported() || (new GrowlNotifier(null)).IsSupported();
			var notifier = _locator.Locate<ISendNotifications>();
			if (isSupported)
				notifier.ShouldNotBeOfType<NullNotifier>();
			else
				notifier.ShouldBeOfType<NullNotifier>();
		}

        [Test]
        public void Should_register_growl_notifier_if_available()
        {
            var isSupported = (new GrowlNotifier(null)).IsSupported();
            var notifier = _locator.Locate<ISendNotifications>();
            if (isSupported)
                notifier.ShouldBeOfType<GrowlNotifier>();
            else
                notifier.ShouldNotBeOfType<GrowlNotifier>();
        }
		
		[Test]
		public void Should_register_assembly_resolver()
		{
			var resolver = _locator.Locate<IResolveAssemblyReferences>();
			resolver.ShouldBeOfType<IResolveAssemblyReferences>();
		}
		
		[Test]
		public void Should_register_assembly_change_consumer()
		{
			var consumer = _locator.Locate<IConsumerOf<AssemblyChangeMessage>>();
			consumer.ShouldBeOfType<IConsumerOf<AssemblyChangeMessage>>();
		}
		
		[Test]
		public void Should_register_assembly_identifier_retriever()
		{
			var retriever = _locator.Locate<IRetrieveAssemblyIdentifiers>();
			retriever.ShouldBeOfType<IRetrieveAssemblyIdentifiers>();
		}
    }
}