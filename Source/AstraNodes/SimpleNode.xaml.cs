using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using AstraNodes.Props;

namespace AstraNodes
{
    /// <summary>
    /// An Example Node to be used in AstraNodes.
    /// This code shows how to build a node using a UserControl as
    /// base class, within the NodeGraph.
    /// </summary>
    public partial class SimpleNode : UserControl,INode
    {
        /// <summary>
        /// This should return the own type in order to be correctly instatiated
        /// </summary>
        public Type UICopyClass
        {
            get { return this.GetType(); }
        }

        /// <summary>
        /// This is used to define a wait time (sleeping the thread) while executing the node
        /// </summary>
        public int Pause { get; set; }

        /// <summary>
        /// Route this.PreviewMouseLeftButtonDown -> to NodePreviewMouseLeftButtonDown
        /// </summary>
        public event MouseButtonEventHandler NodePreviewMouseLeftButtonDown;

        /// <summary>
        /// Route this.MouseDown -> to NodeMouseDown
        /// </summary>
        public event MouseButtonEventHandler NodeMouseDown;

        /// <summary>
        /// Route this.MouseMove -> to NodeMouseMove
        /// </summary>
        public event MouseEventHandler NodeMouseMove;

        /// <summary>
        /// Route this.MouseUp -> to NodeMouseUp
        /// </summary>
        public event MouseButtonEventHandler NodeMouseUp;

        /// <summary>
        /// Override OnVisualParentChanged and invoke -> to NodeParentChanged
        /// </summary>
        public event ParentChanged NodeParentChanged;

        /// <summary>
        /// Creates a default instance of the this
        /// </summary>
        /// <returns>UIElement default Instance</returns>
        public UIElement CreateDefaultNode()
        {
            return new SimpleNode();
        }

        /// <summary>
        /// The Node element handling the interaction
        /// </summary>
        public Node Node { get; }

        /// <summary>
        /// Returns this as an UI Element.
        /// </summary>
        public UIElement Element
        {
            get { return this; }
        }

        /// <summary>
        /// This Stackpanel is one of the stackpanels created in the
        /// xaml page. It is used to display Input Connectors
        /// </summary>
        public StackPanel Inputs { get; set; }

        /// <summary>
        /// This Stackpanel is one of the stackpanels created in the
        /// xaml page. It is used to display Input Connectors
        /// </summary>
        public StackPanel Outputs { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleNode()
        {
            InitializeComponent();
            Node = new Node(this);

            Inputs = stack_input;
            Outputs = stack_output;

            Pause = 1;

            this.PreviewMouseLeftButtonDown += (sender, args) => NodePreviewMouseLeftButtonDown?.Invoke(sender, args);
            this.MouseDown += (sender, args) => NodeMouseDown?.Invoke(sender, args);
            this.MouseMove += (sender,args ) => NodeMouseMove?.Invoke(sender,args);
            this.MouseUp += (sender, args) => NodeMouseUp?.Invoke(sender, args);
        }

        /// <summary>
        /// Override OnVisualParentChanged and Invoke Event defined in interface
        /// </summary>
        /// <param name="oldParent"></param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            NodeParentChanged?.Invoke(oldParent, this);
        }

        /// <summary>
        /// All the properties the user could define or change in the node
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// This method is used to Process the data given as input via
        /// the node processing.
        /// </summary>
        /// <param name="ins">array of data comming from inputs</param>
        /// <param name="properties">list of properties available in the node</param>
        /// <returns></returns>
        public object[] Process(object[] ins, Dictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is called by the node que to make the node
        /// able to output the current processed data. -> The result.
        /// </summary>
        /// <param name="processed"></param>
        public void Output(object[] processed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set up the input and outputs
        /// </summary>
        public void SetConnection()
        {
            Node.addInput(50);
            Node.addInput(3);
            Node.addInput(3);
            Node.addInput(150);

            Node.addOutput(150);
            Node.addOutput(3);
            Node.addOutput(3);
            Node.addOutput(50);
        }

        /// <summary>
        /// Showing how a custom delete button could work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Node.graph.removeNode(this.Node);
        }
    }
}
