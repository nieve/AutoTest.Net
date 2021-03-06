﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoTest.Core.TestRunners;
using AutoTest.Core.TestRunners.TestRunners;

namespace AutoTest.Core.Caching.Projects
{
    public class ProjectDocument
    {
        private bool _isReadFromFile = false;
        private ProjectType _type;
        private string _assemblyname;
        private string _buildConfiguration;
        private string _platform;
        private string _outputPath;
        private string _framework;
        private string _vsVersion;
        private List<Type> _containsTestsFor = new List<Type>();
        private List<string> _references = new List<string>();
        private List<string> _referencedBy = new List<string>();

        public bool IsReadFromFile { get { return _isReadFromFile; } }
        public ProjectType Type { get { return _type; } }
        public string AssemblyName { get { return _assemblyname; } }
        public string BuildConfiguration { get { return _buildConfiguration; } }
        public string Platform { get { return _platform; } }
        public string OutputPath { get { return _outputPath; } }
        public string Framework { get { return _framework; } }
        public string ProductVersion { get { return _vsVersion; } }
        public bool ContainsTests { get { return _containsTestsFor.Count > 0; } }
        public bool ContainsNUnitTests { get { return _containsTestsFor.Contains(typeof (NUnitTestRunner)); } }
        public bool ContainsMSTests { get { return _containsTestsFor.Contains(typeof(MSTestRunner)); } }
        public bool ContainsXUnitTests { get { return _containsTestsFor.Contains(typeof (XUnitTestRunner)); } }
        public string[] References { get { return _references.ToArray(); } }
        public string[] ReferencedBy { get { return _referencedBy.ToArray(); } }

        public ProjectDocument(ProjectType type)
        {
            _type = type;
        }

        public void AddReference(string reference)
        {
            _references.Add(reference);
        }

        public void AddReference(string[] references)
        {
            _references.AddRange(references);
        }

        public void RemoveReference(string reference)
        {
            _references.Remove(reference);
        }

        public void AddReferencedBy(string reference)
        {
            _referencedBy.Add(reference);
        }

        public void AddReferencedBy(string[] references)
        {
            _referencedBy.AddRange(references);
        }

        public void RemoveReferencedBy(string reference)
        {
            _referencedBy.Remove(reference);
        }

        public void HasBeenReadFromFile()
        {
            _isReadFromFile = true;
        }

        public void SetAsNUnitTestContainer()
        {
            _containsTestsFor.Add(typeof (NUnitTestRunner));
        }

        public void SetAsMSTestContainer()
        {
            _containsTestsFor.Add(typeof(MSTestRunner));
        }

        public void SetAsXUnitTestContainer()
        {
            _containsTestsFor.Add(typeof(XUnitTestRunner));
        }

        public bool IsReferencedBy(string reference)
        {
            return _referencedBy.Contains(reference);
        }

        public bool IsReferencing(string reference)
        {
            return _references.Contains(reference);
        }

        public void SetAssemblyName(string assemblyName)
        {
            _assemblyname = assemblyName;
        }

        public void SetConfiguration(string configuration)
        {
            _buildConfiguration = configuration;
        }

        public void SetPlatform(string platform)
        {
            _platform = platform;
        }

        public void SetOutputPath(string outputPath)
        {
            _outputPath = outputPath;
        }

        public void SetFramework(string version)
        {
            _framework = version;
        }

        public void SetVSVersion(string version)
        {
            _vsVersion = version;
        }
    }
}
