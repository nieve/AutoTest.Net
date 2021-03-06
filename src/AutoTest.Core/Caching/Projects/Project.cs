﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTest.Core.Caching.Projects
{
    public class Project : IRecord
    {
        public string Key { get; private set; }
        public ProjectDocument Value { get; private set; }

        public Project(string key, ProjectDocument value)
        {
            Key = key;
            Value = value;
        }

        public void Reload()
        {
            Value = new ProjectDocument(Value.Type);
        }
    }
}
