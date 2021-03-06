﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;

namespace AstraNodes.Props
{
    public delegate void ChangeHandler();

    public interface INodeProperty
    {
        bool IsUI { get; set; }
        string Label { get; set; }
        object Value { get; set; }

        event ChangeHandler valueChanged;
    }
}
