using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AstraNodes.DragDrop
{
    public interface INodeRepresentation
    {
        INode NodeTemplate { get; set; }

    }
}
