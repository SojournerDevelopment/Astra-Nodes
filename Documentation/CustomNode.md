# Creating a custom node

In order to create a custom node, you have to implement the INode Interface in your UIElement. This UIElement can be a UserControl that is built up upon XAML.

The following Code shows a guide on how to implement a Node. The following example describes the xaml part and the code behind. The CustomNode shown in this tutorial looks like the following:

![CustomNode.PNG](Images%5CCustomNode.PNG)

```xml
<UserControl x:Class="AstraNodes.SimpleNode"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:AstraNodes"
             mc:Ignorable="d" Height="150" Width="300"
             d:DesignHeight="150" d:DesignWidth="300">
    <Border BorderThickness="1" Background="#FF2F2F2F" CornerRadius="3">
        <Border.Effect>
            <DropShadowEffect Direction="0" ShadowDepth="0" BlurRadius="20" RenderingBias="Quality" Opacity="0.15"/>
        </Border.Effect>
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="6"></ColumnDefinition>
                <ColumnDefinition Width="*"></ColumnDefinition>
                <ColumnDefinition Width="6"></ColumnDefinition>
            </Grid.ColumnDefinitions>

            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"></RowDefinition>
                <RowDefinition Height="*"></RowDefinition>
            </Grid.RowDefinitions>

            <StackPanel Grid.Row="1" Name="stack_input" Grid.Column="0" MaxWidth="12" Margin="-6,0,0,0" HorizontalAlignment="Left"></StackPanel>
            <StackPanel Grid.Row="1" Name="stack_output" Grid.Column="2" MaxWidth="12" Margin="0,0,-6,0" HorizontalAlignment="Right">
            </StackPanel>
            <Grid Name="grid_content" Row="1" Grid.Column="1"></Grid>

            <Border Grid.Column="0" Grid.ColumnSpan="3" Grid.Row="0">
                <Grid Name="grid_controlbar">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="*"></ColumnDefinition>
                        <ColumnDefinition Width="Auto"></ColumnDefinition>
                    </Grid.ColumnDefinitions>

                    <StackPanel Grid.Column="0" Orientation="Horizontal">
                        <TextBlock Margin="5" Name="tb_title" Text="Title" Foreground="#FFE9E9E9" VerticalAlignment="Bottom"/>
                        <TextBlock Margin="5" Text="Subtitle" HorizontalAlignment="Left" VerticalAlignment="Bottom" Foreground="#FF3F3F3F"/>
                    </StackPanel>

                    <StackPanel Orientation="Horizontal" Grid.Column="1" Height="26">
                        <Button Name="btn_start" Content="> " Width="26" Foreground="#FFE9E9E9" BorderBrush="{x:Null}" Background="{x:Null}"></Button>
                        <Button Name="btn_properties" Content="P" Width="26" Foreground="#FFE9E9E9" BorderBrush="{x:Null}" Background="{x:Null}"></Button>
                        <Button Name="btn_visual" Content="V" Width="26" Foreground="#FFE9E9E9" BorderBrush="{x:Null}" Background="{x:Null}"></Button>
                        <Button Name="btn_delete" Content="X" Width="26" Foreground="#FFFF5A5A" BorderBrush="{x:Null}" Background="{x:Null}" Click="Btn_delete_Click"/>
                    </StackPanel>

                </Grid>
            </Border>
        </Grid>
    </Border>
</UserControl>
```

When creating a new custom Node, the class behind should contain the same functionality, as described below.

```csharp

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

```