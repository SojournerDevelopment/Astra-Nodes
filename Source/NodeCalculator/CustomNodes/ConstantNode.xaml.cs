﻿using System;
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
using AstraNodes;

namespace NodeCalculator.CustomNodes
{
    /// <summary>
    /// Interaktionslogik für ConstantNode.xaml
    /// </summary>
    public partial class ConstantNode : UserControl, INode
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
        public ConstantNode()
        {
            Properties = new Dictionary<string, object>();
            InitializeComponent();
            Node = new Node(this);

            Inputs = stack_input;
            Outputs = stack_output;

            Pause = 1;

            this.PreviewMouseLeftButtonDown += (sender, args) => NodePreviewMouseLeftButtonDown?.Invoke(sender, args);
            this.MouseDown += (sender, args) => NodeMouseDown?.Invoke(sender, args);
            this.MouseMove += (sender, args) => NodeMouseMove?.Invoke(sender, args);
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
            object[] toRet = new object[1];
            toRet[0] = properties["value"];
            return toRet;
        }

        /// <summary>
        /// This method is called by the node que to make the node
        /// able to output the current processed data. -> The result.
        /// </summary>
        /// <param name="processed"></param>
        public void Output(object[] processed)
        {
            //
        }

        /// <summary>
        /// Set up the input and outputs
        /// </summary>
        public void SetConnection()
        {
            Node.addOutput(150);

            Properties["value"] = 1;
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

        private void Tb_output_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Properties["value"] = tb_output.Text;
        }
    }
}
