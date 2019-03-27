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
using AstraNodes;
using NodeCalculator.CustomNodes;

namespace NodeCalculator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NodeGraph graph;
        private NodeChest nodeChest;
        private NodeGraphContext context;

        public MainWindow()
        {
            InitializeComponent();

            

            context = new NodeGraphContext();

            nodeChest = new NodeChest(context);

            SimpleNode sm = new SimpleNode();
            
            nodeChest.addNode(sm.Node);

            graph = new NodeGraph(context);
            graph.pipeStiffness = 0;

            container.Children.Add(graph);
            //chestContainer.Children.Add(nodeChest);


            nodeChest.ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            ConstantNode node = new ConstantNode();
            node.Node.position = new Point(50,50);

            DisplayNode node2 = new DisplayNode();
            node2.Node.position = new Point(500, 30);

            graph.addNode(node.Node);
            graph.addNode(node2.Node);

            new Connection(node.Node.getOutputs()[0], node2.Node.getInputs()[0]);
        }

        private void Btn_run_Click(object sender, RoutedEventArgs e)
        {
            graph.autoArrange();

            Thread x = new Thread(() =>
            {
                /*
                while (true)
                {
                    Dispatcher.Invoke(new Action(() => { graph.process(); }));
                    Thread.Sleep(5);
                } */
                Dispatcher.Invoke(new Action(() => { graph.process(); }));
            });
            x.Start();
        }
    }
}
