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

namespace AstraNodes.DragDrop
{
    /// <summary>
    /// Interaktionslogik für SimpleRepresentation.xaml
    /// </summary>
    public partial class SimpleRepresentation : UserControl, INodeRepresentation
    {
        private Nullable<Point> dragStart = null;
        private Point startPoint;

        public SimpleRepresentation()
        {
            InitializeComponent();

            this.lbl_name.Content = "Simple Node";
            
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.startPoint = (e.GetPosition(null));
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (sender as UIElement);
            dragStart = e.GetPosition(element);
            element.CaptureMouse();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (sender as UIElement);
            dragStart = null;
            element.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("node-template", this); // TODO: Doublecheck here!
                System.Windows.DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);
            }
        }



        public INode NodeTemplate
        {
            get
            {
                return new SimpleNode();
            }
            set
            {
                return;
            }
        }

    }
}
