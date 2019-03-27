using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AstraNodes.Docking;
using AstraNodes.DragDrop;

namespace AstraNodes
{
    
    /// <summary>
    /// Interaktionslogik für NodeGraph.xaml
    /// </summary>
    public partial class NodeGraph : Border
    {
        public readonly NodeGraphContext context;

        private INode LastDropItemNode = null;
        private INodeRepresentation LastDropItemNodeRepresenation = null;

        private double _pipeStiffness = 50;
        public double pipeStiffness
        {
            get
            {
                return _pipeStiffness;
            }
            set
            {
                if (_pipeStiffness == value)
                    return;
            }
        }

        public NodeGraph(NodeGraphContext context)
        {
            InitializeComponent();

            context.propertyChanged += Context_propertyChanged;
            this.context = context;

            this.MouseDown += NodeGraph_MouseDown;
            this.DragEnter += Canvas_DragEnter;
            this.Drop += NodeGraph_Drop;
            this.AllowDrop = true;
            this.PreviewKeyDown += OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (Connection.Current != null)
                {
                    Connection.Current.Dispose();
                    Connection.Current = null;
                }
            }
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

        private void NodeGraph_Drop(object sender, DragEventArgs e)
        {
            if (LastDropItemNode != null || LastDropItemNodeRepresenation != null)
                return;

            INode node = e.Data.GetData("node") as INode;
            LastDropItemNode = node;
            INodeRepresentation representation = e.Data.GetData("node-template") as INodeRepresentation;
            LastDropItemNodeRepresenation = representation;

            // Fix for double firing event
            Thread disposeThread = new Thread(() =>
            {
                Thread.Sleep(1);
                Dispatcher.Invoke(new Action(() => {
                    LastDropItemNode = null;
                    LastDropItemNodeRepresenation = null;
                }));
            });
            disposeThread.Start();

            if (node != null)
            {
                var copy = Activator.CreateInstance(node.UICopyClass) as INode;
                addNode(copy.Node);
                copy.Node.position = e.GetPosition(canvas);
            }
            if (representation != null)
            {
                var copy = Activator.CreateInstance(representation.NodeTemplate.UICopyClass) as INode;
                addNode(copy.Node);
                copy.Node.position = e.GetPosition(canvas);
            }
            e.Handled = true;
        }

        

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            INode node = e.Data.GetData("node") as INode;
            if (node == null || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void NodeGraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Connection.Current != null && (Mouse.DirectlyOver == this || Mouse.DirectlyOver == canvas))
            {
                Connection.Current.Dispose();
                Connection.Current = null;
            }
        }

        public T addNode<T>(T node) where T : Node
        {
            canvas.Children.Add(node.node.Element);
            node.node.SetConnection();
            return node;
        }

        public bool tryAddNode(Node node)
        {
            if (!canvas.Children.Contains(node.node.Element))
            {
                canvas.Children.Add(node.node.Element);
                node.node.SetConnection();
                return true;
            }
            return false;
        }

        public void removeNode(Node node)
        {
            canvas.Children.Remove(node.node.Element);
            node.Dispose();
        }

        public bool tryRemoveNode(Node node)
        {
            if (canvas.Children.Contains(node.node.Element))
            {
                canvas.Children.Remove(node.node.Element);
                node.Dispose();
                return true;
            }
            return false;
        }

        public void autoArrange()
        {

            Node[] nodes = getNodes();
            Dictionary<int, int> columns = new Dictionary<int, int>();
            int maxMaxDepth = -1;

            for (int i = 0; i < nodes.Length; i++)
            {
                int maxDepth = nodes[i].getMaximumDepth();
                if (maxDepth > maxMaxDepth)
                    maxMaxDepth = maxDepth;
            }

            for (int i = 0; i < nodes.Length; i++)
            {
                int maxDepth = nodes[i].getMaximumDepth();

                if (columns.ContainsKey(maxDepth))
                {
                    columns[maxDepth]++;
                }
                else
                {
                    columns.Add(maxDepth, 1);
                }

                switch (context.orientation)
                {
                    case NodeGraphOrientation.LeftToRight:
                        nodes[i].position = new Point(maxDepth * 350, columns[maxDepth] * 100);
                        break;
                    case NodeGraphOrientation.RightToLeft:
                        nodes[i].position = new Point((maxMaxDepth - maxDepth) * 350, columns[maxDepth] * 100);
                        break;
                    case NodeGraphOrientation.UpToBottom:
                        nodes[i].position = new Point(columns[maxDepth] * 270, maxDepth * 150);
                        break;
                    case NodeGraphOrientation.BottomToUp:
                        nodes[i].position = new Point(columns[maxDepth] * 270, (maxMaxDepth - maxDepth) * 150);
                        break;
                }
            }
        }

        public Node[] getNodes()
        {
            var x = canvas.Children.OfType<INode>();
            List<Node> nodes = new List<Node>();
            foreach (INode node in x)
            {
               nodes.Add(node.Node);
            }
            return nodes.ToArray();
        }

        public void process()
        {
            var nodes = getNodes();
            // Clears all the previous run results
            foreach (Node node in nodes)
            {
                foreach (OutputConnector dock in node.getOutputs())
                {
                    foreach (Connection pipe in dock.Connections)
                    {
                        pipe.Result = null;
                    }
                }
            }
            // Runs !
            foreach (Node node in nodes.Where(x => x.getInputs().Length == 0))
            {
                Thread thread = new Thread(new ThreadStart(() => {
                    node.queryProcess();
                }));
                thread.Start();
            }
        }
    }
}