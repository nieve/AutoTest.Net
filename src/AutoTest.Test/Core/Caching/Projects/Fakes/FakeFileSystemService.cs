﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoTest.Core.FileSystem;
using NUnit.Framework;

namespace AutoTest.Test.Core.Caching.Projects.Fakes
{
    class FakeFileSystemService : IFileSystemService
    {
        private string _searchPattern = "";
        private string[] _projectFiles = null;
        private bool _getFilesCalled = false;
        private bool _directoryExists = true;

        public FakeFileSystemService WhenCrawlingFor(string searchPattern)
        {
            _searchPattern = searchPattern;
            return this;
        }

        public void WhenValidatingDirectoryReturn(bool returnValue)
        {
            _directoryExists = returnValue;
        }

        public void Return(string projectFiles)
        {
            _projectFiles = new string[] {projectFiles};
        }

        #region IFileSystemService Members

        public string[] GetFiles(string path, string searchPattern)
        {
            _getFilesCalled = true;
            if (searchPattern.Equals(_searchPattern))
                return _projectFiles;
            return new string[] {};
        }

        public string ReadFileAsText(string path)
        {
            return new FileSystemService().ReadFileAsText(path);
        }

        public bool DirectoryExists(string path)
        {
            return _directoryExists;
        }

        #endregion

        internal void GetFilesWasNotCalled()
        {
            _getFilesCalled.ShouldBeFalse();
        }
    }
}
