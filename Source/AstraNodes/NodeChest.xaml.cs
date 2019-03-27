using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AstraNodes
{
    /// <summary>
    /// Interaktionslogik für NodeChest.xaml
    /// </summary>
    public partial class NodeChest : Border
    {
        public ScrollViewer ScrollViewer
        {
            get { return this.scrollViewer; }
        }

        public readonly NodeGraphContext context;

        public NodeChest(NodeGraphContext context)
        {
            InitializeComponent();
            context.propertyChanged += Context_propertyChanged;
            this.context = context;
        }

        private void Context_propertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "orientation":
                    foreach (Node node in getNodes())
                    {
                        node.updateOrientation();
                    }
                    break;
            }
        }

        public Node[] getNodes()
        {
            return wrapPanel.Children.OfType<Node>().ToArray();
        }

        public void addNode(Node node)
        {
            (node.node.Element as UserControl).Margin = new Thickness(15);
            wrapPanel.Children.Add(node.node.Element);
            node.node.SetConnection();
        }

        public void removeNode(Node node)
        {
            if (wrapPanel.Children.Contains(node.node.Element))
            {
                wrapPanel.Children.Remove(node.node.Element);
            }
        }
    }
}
