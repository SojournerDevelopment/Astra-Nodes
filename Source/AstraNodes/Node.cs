using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AstraNodes.Docking;
using AstraNodes.Props;

namespace AstraNodes
{
    public class Node : IDisposable
    {
        public delegate void NodeEventHandler(Node node);
        public event NodeEventHandler moved;

        /// <summary>
        /// parent and wrapper for the Node Data
        /// </summary>
        public INode node { get; private set; }

        public NodeGraph graph { get; private set; }
        public NodeChest chest { get; private set; }
        private Nullable<Point> dragStart = null;

        public NodeGraphContext context => (graph != null) ? graph.context : chest.context;

        public InputConnector[] getInputs() => Application.Current.Dispatcher.Invoke(() => node.Inputs.Children.OfType<InputConnector>().ToArray());
        public OutputConnector[] getOutputs() => Application.Current.Dispatcher.Invoke(() => node.Outputs.Children.OfType<OutputConnector>().ToArray());


        public bool isTemplate => chest != null;

        public Node(INode visual)
        {
            this.node = visual;
            initialize();
        }

        private void initialize()
        {
            //Background = (Brush)converter.ConvertFromString("#262626");
            node.NodePreviewMouseLeftButtonDown += Node_PreviewMouseLeftButtonDown;
            node.NodeMouseDown += mouseDown;
            node.NodeMouseMove += mouseMove;
            node.NodeMouseUp += mouseUp;
            // Event if Parent Changed
            node.NodeParentChanged += OnVisualParentChanged;
        }

        Point startPoint;

        private void Node_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        protected void OnVisualParentChanged(DependencyObject oldParent, DependencyObject current)
        {
            for (int i = 0; i < 5; i++)
            {
                current = (current as FrameworkElement)?.Parent;
                if (current == null)
                    break;
                graph = current as NodeGraph;
                chest = current as NodeChest;
                if (graph != null || chest != null)
                    break;
            }
            updateOrientation();
        }

        public void updateOrientation()
        {

            //if (grid != null)
            //    grid.Children.Clear();

            //grid = new Grid();
            switch (context.orientation)
            {
                case NodeGraphOrientation.LeftToRight:
                    // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6) });
                    // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6) });
                    // grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    // grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    //stackInputs.Orientation = Orientation.Vertical;
                    //stackInputs.Margin = new Thickness(-6, 0, 0, 0);
                    //stackInputs.VerticalAlignment = VerticalAlignment.Center;
                    //stackOutputs.Margin = new Thickness(0, 0, -6, 0);
                    //stackOutputs.VerticalAlignment = VerticalAlignment.Center;

                    //Grid.SetColumn(titleUI, 1);
                    //Grid.SetColumn(stackInputs, 0);
                    //Grid.SetColumn(stackParameters, 1);
                    //Grid.SetColumn(stackOutputs, 2);

                    //Grid.SetRow(titleUI, 0);
                    //Grid.SetRow(stackInputs, 1);
                    //Grid.SetRow(stackParameters, 1);
                    //Grid.SetRow(stackOutputs, 1);
                    break;
                case NodeGraphOrientation.RightToLeft:
                    //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6) });
                    //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    //stackInputs.Orientation = Orientation.Vertical;
                    //stackInputs.Margin = new Thickness(0, 0, -6, 0);
                    //stackInputs.VerticalAlignment = VerticalAlignment.Center;
                    //stackOutputs.Margin = new Thickness(-6, 0, 0, 0);
                    //stackOutputs.VerticalAlignment = VerticalAlignment.Center;

                    //Grid.SetColumn(titleUI, 1);
                    //Grid.SetColumn(stackInputs, 2);
                    //Grid.SetColumn(stackParameters, 1);
                    //Grid.SetColumn(stackOutputs, 0);

                    //Grid.SetRow(titleUI, 0);
                    //Grid.SetRow(stackInputs, 1);
                    //Grid.SetRow(stackParameters, 1);
                    //Grid.SetRow(stackOutputs, 1);
                    break;
                case NodeGraphOrientation.UpToBottom:
                    //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(6) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(6) });

                    //stackInputs.Orientation = Orientation.Horizontal;
                    //stackInputs.Margin = new Thickness(0, -6, 0, 0);
                    //stackInputs.HorizontalAlignment = HorizontalAlignment.Center;
                    //stackOutputs.Margin = new Thickness(0, 0, 0, -6);
                    //stackOutputs.HorizontalAlignment = HorizontalAlignment.Center;

                    //Grid.SetRow(stackInputs, 0);
                    //Grid.SetRow(titleUI, 1);
                    //Grid.SetRow(stackParameters, 2);
                    //Grid.SetRow(stackOutputs, 3);
                    break;
                case NodeGraphOrientation.BottomToUp:
                    //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(6) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(6) });

                    //stackInputs.Orientation = Orientation.Horizontal;
                    //stackInputs.Margin = new Thickness(0, 0, 0, -6);
                    //stackInputs.HorizontalAlignment = HorizontalAlignment.Center;
                    //stackOutputs.Margin = new Thickness(0, -6, 0, 0);
                    //stackOutputs.HorizontalAlignment = HorizontalAlignment.Center;

                    //Grid.SetRow(stackInputs, 3);
                    //Grid.SetRow(titleUI, 1);
                    //Grid.SetRow(stackParameters, 2);
                    //Grid.SetRow(stackOutputs, 0);
                    break;
            }

            //Child = grid;
            //grid.Children.Add(titleUI);
            //grid.Children.Add(stackInputs);
            //grid.Children.Add(stackParameters);
            //grid.Children.Add(stackOutputs);

            moved?.Invoke(this);
        }

        private void mouseMove(object sender, MouseEventArgs args)
        {
            if (isTemplate)
            {
                // Get the current mouse position
                Point mousePos = args.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (args.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("node", node); // TODO: Doublecheck here!
                    System.Windows.DragDrop.DoDragDrop(node.Element, dragData, DragDropEffects.Move);
                }
            }
            else
            {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    UIElement element = (UIElement)sender;
                    Point p2 = args.GetPosition(graph.canvas);
                    position = new Point(p2.X - dragStart.Value.X, p2.Y - dragStart.Value.Y);
                    isAboutToBeRemoved = !isPointWithin(p2);
                }
            }
        }

        private bool isPointWithin(Point point)
        {
            if (point.X < 0 || point.Y < 0 || point.X > graph.ActualWidth || point.Y > graph.ActualHeight)
                return false;
            return true;
        }

        private void mouseUp(object sender, MouseEventArgs args)
        {
            var element = (UIElement)sender;
            dragStart = null;
            element.ReleaseMouseCapture();
            if (isAboutToBeRemoved)
                graph.removeNode(this);
        }

        private void mouseDown(object sender, MouseEventArgs args)
        {
            var element = (UIElement)sender;
            dragStart = args.GetPosition(element);
            element.CaptureMouse();
        }

        private bool _isAboutToBeRemoved = false;
        public bool isAboutToBeRemoved
        {
            get { return _isAboutToBeRemoved; }
            private set
            {
                if (_isAboutToBeRemoved == value)
                    return;

                _isAboutToBeRemoved = value;
                if (_isAboutToBeRemoved)
                {
                    // BorderBrush = Brushes.Red;
                    // BorderThickness = new Thickness(2);
                }
                else
                {
                    // BorderBrush = Brushes.White;
                    // BorderThickness = new Thickness(0);
                }
            }
        }

        public Point position
        {
            get { return new Point(Canvas.GetLeft(node.Element), Canvas.GetTop(node.Element)); }
            set
            {
                Canvas.SetLeft(node.Element, value.X);
                Canvas.SetTop(node.Element, value.Y);
                moved?.Invoke(this);
            }
        }

        public Connector addInput(int type)
        {
            InputConnector dock = new InputConnector(this, type);
            node.Inputs.Children.Add(dock);
            //stackInputs.UpdateLayout();
            //RenderSize = MeasureOverride(RenderSize);
            //RenderSize = ArrangeOverride(new Size(double.MaxValue, double.MaxValue));
            //Measure(new Size(double.MaxValue, double.MaxValue));
            return dock;
        }

        public void removeInput(Connector dock)
        {
            node.Inputs.Children.Remove(dock);
        }

        public OutputConnector addOutput(int type)
        {
            OutputConnector dock = new OutputConnector(this, type);
            node.Outputs.Children.Add(dock);
            return dock;
        }

        public void removeOutput(Connector dock)
        {
            node.Outputs.Children.Remove(dock);
        }

        public void clearInputs()
        {
            foreach (InputConnector input in getInputs())
            {
                if (input.Connection != null)
                {
                    input.Connection.Dispose();
                }
            }
            node.Outputs.Children.Clear();
        }

        public void clearOutputs()
        {
            foreach (OutputConnector output in getOutputs())
            {
                foreach (Connection pipe in output.Connections)
                {
                    pipe.Dispose();
                }
            }

            node.Outputs.Children.Clear();
        }

        public IProperty addProperty<IProperty>(IProperty property)
        {
            node.Properties.Add((property as INodeProperty).Label, (property as INodeProperty));
            return property;
        }

        public int getMaximumDepth()
        {
            InputConnector[] inputs = getInputs();
            int maxDepth = 0;
            foreach (InputConnector input in inputs)
            {
                Node previousNode = input.Connection?.OutputConnector?.node;
                if (previousNode == null)
                    continue;
                int previousMaxDepth = previousNode.getMaximumDepth() + 1;
                if (previousMaxDepth > maxDepth)
                    maxDepth = previousMaxDepth;
            }
            return maxDepth;
        }

        public void queryProcess()
        {
            // Makes sure all inputs are ready for processing. Otherwise, cancel.
            InputConnector[] ins = getInputs();
            foreach (InputConnector input in ins)
                if (input.Connection.Result == null)
                    return;

            // Process the data (Overriding class implementation)
            //Application.Current.Dispatcher.Invoke(() => { BorderThickness = new Thickness(2); });

            // Sleep for a time set
            Thread.Sleep(node.Pause);

            object[] results = node.Process(ins.Select(x => x.Connection.Result).ToArray(),node.Properties);
            //Dispatcher.Invoke(() => { BorderThickness = new Thickness(0); });
            
            // Call the Output Visualization
            node.Output(results);

            // Tranfers results to next nodes
            OutputConnector[] outs = getOutputs();
            for (int i = 0; i < outs.Length; i++)
            {
                foreach (Connection pipe in outs[i].Connections)
                {
                    pipe.Result = results[i];
                    pipe.InputConnector.node.queryProcess();
                }
            }
        }

        public void Dispose()
        {
            // Visibility = Visibility.Collapsed;
            foreach (InputConnector input in getInputs())
            {
                if (input.Connection != null)
                    input.Connection.Dispose();
            }
            foreach (OutputConnector output in getOutputs())
            {
                foreach (Connection pipe in output.Connections.ToArray())
                {
                    pipe.Dispose();
                }
            }
        }
    }
}
