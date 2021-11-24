using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zadanie1
{
    public partial class MainWindow : Window
    {
        private object obj;
        private Point pt;
        private Rectangle prostokat;
        private Line line;
        private Ellipse ellipse;
        private Shape shape;
        private string choose = "Linia";
        private int X1, X2, Y1, Y2, R, A, B;
        private Point startPt;

        private void Clean(object sender, RoutedEventArgs e)
        {
            kontener.Children.Clear();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == true)
            {
                FileStream fs = File.Open(saveFile.FileName, FileMode.Create);
                XamlWriter.Save(kontener, fs);
                fs.Close();
            }
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                FileStream fs = File.Open(openFile.FileName, FileMode.Open, FileAccess.Read);
                Canvas saved = XamlReader.Load(fs) as Canvas;
                fs.Close();
                kontener.Children.Clear();

                while (saved.Children.Count > 0)
                {
                    UIElement uie = saved.Children[0];
                    saved.Children.Remove(uie);
                    kontener.Children.Add(uie);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void VisibleLine()
        {
            x2.Visibility = Visibility.Visible;
            x2Label.Visibility = Visibility.Visible;
            y2.Visibility = Visibility.Visible;
            y2Label.Visibility = Visibility.Visible;
            a.Visibility = Visibility.Hidden;
            aLabel.Visibility = Visibility.Hidden;
            b.Visibility = Visibility.Hidden;
            bLabel.Visibility = Visibility.Hidden;
            r.Visibility = Visibility.Hidden;
            rLabel.Visibility = Visibility.Hidden;
        }

        private void VisibleRect()
        {
            x2.Visibility = Visibility.Hidden;
            x2Label.Visibility = Visibility.Hidden;
            y2.Visibility = Visibility.Hidden;
            y2Label.Visibility = Visibility.Hidden;
            a.Visibility = Visibility.Visible;
            aLabel.Visibility = Visibility.Visible;
            b.Visibility = Visibility.Visible;
            bLabel.Visibility = Visibility.Visible;
            r.Visibility = Visibility.Hidden;
            rLabel.Visibility = Visibility.Hidden;
        }

        private void VisibleEllipse()
        {
            x2.Visibility = Visibility.Hidden;
            x2Label.Visibility = Visibility.Hidden;
            y2.Visibility = Visibility.Hidden;
            y2Label.Visibility = Visibility.Hidden;
            a.Visibility = Visibility.Hidden;
            aLabel.Visibility = Visibility.Hidden;
            b.Visibility = Visibility.Hidden;
            bLabel.Visibility = Visibility.Hidden;
            r.Visibility = Visibility.Visible;
            rLabel.Visibility = Visibility.Visible;
        }

        private void ActualPoints(object sender)
        {
            Shape sh = (Shape)sender;
            if (sender.GetType() == typeof(Line))
            {
                VisibleLine();
                line = (Line)sender;
                x1.Text = Math.Round(line.X1).ToString();
                x2.Text = Math.Round(line.X2).ToString();
                y1.Text = Math.Round(line.Y1).ToString();
                y2.Text = Math.Round(line.Y2).ToString();
            }
            else if (sender.GetType() == typeof(Rectangle))
            {
                VisibleRect();
                x1.Text = Math.Round(Canvas.GetLeft(sh)).ToString();
                y1.Text = Math.Round(Canvas.GetTop(sh)).ToString();
                a.Text = Math.Round(sh.Width).ToString();
                b.Text = Math.Round(sh.Height).ToString();
            }
            else
            {
                VisibleEllipse();
                x1.Text = Math.Round(Canvas.GetLeft(sh)).ToString();
                y1.Text = Math.Round(Canvas.GetTop(sh)).ToString();
                r.Text = Math.Round(sh.Width / 2).ToString();
            }
        }


        private void SelectChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)TypeDraw.SelectedItem;
            string value = typeItem.Content.ToString();
            if (value == "Linia")
            {
                choose = "Linia";
                VisibleLine();
            }
            else if (value == "Prostokąt")
            {
                choose = "Prostokąt";
                VisibleRect();
            }
            else if (value == "Koło")
            {
                choose = "Koło";
                VisibleEllipse();
            }
        }

        private void Rysuj(object sender, RoutedEventArgs e)
        {
            if(DrawRadio.IsChecked==true)
            {
                if (int.TryParse(x1.Text.ToString(), out X1) && int.TryParse(y1.Text.ToString(), out Y1))
                {
                    if (choose == "Prostokąt" && (int.TryParse(a.Text.ToString(), out A) && int.TryParse(b.Text.ToString(), out B)))
                    {
                        prostokat = new Rectangle();
                        prostokat.Stroke = new SolidColorBrush(Colors.BlueViolet);
                        prostokat.StrokeThickness = 8;
                        Canvas.SetLeft(prostokat, X1);
                        Canvas.SetTop(prostokat, Y1);
                        prostokat.Width = B;
                        prostokat.Height = A;
                        prostokat.MouseDown += Shape_MouseLeftButtonDown;
                        prostokat.MouseMove += Shape_MouseMove;
                        prostokat.MouseUp += Shape_MouseLeftButtonUp;
                        kontener.Children.Add(prostokat);
                        prostokat = null;
                    }
                    else if (choose == "Koło" && (int.TryParse(r.Text.ToString(), out R)))
                    {
                        ellipse = new Ellipse();
                        ellipse.Stroke = new SolidColorBrush(Colors.BlueViolet);
                        ellipse.StrokeThickness = 8;
                        Canvas.SetLeft(ellipse, X1);
                        Canvas.SetTop(ellipse, Y1);
                        ellipse.Width = 2 * R;
                        ellipse.Height = 2 * R;
                        ellipse.MouseDown += Shape_MouseLeftButtonDown;
                        ellipse.MouseMove += Shape_MouseMove;
                        ellipse.MouseUp += Shape_MouseLeftButtonUp;
                        kontener.Children.Add(ellipse);
                        ellipse = null;
                    }
                    else if (choose == "Linia" && (int.TryParse(x2.Text.ToString(), out X2) && int.TryParse(y2.Text.ToString(), out Y2)))
                    {
                        line = new Line();
                        line.Stroke = new SolidColorBrush(Colors.BlueViolet);
                        line.StrokeThickness = 8;
                        line.X1 = X1;
                        line.X2 = X2;
                        line.Y1 = Y1;
                        line.Y2 = Y2;
                        line.MouseDown += Shape_MouseLeftButtonDown;
                        line.MouseMove += Shape_MouseMove;
                        line.MouseUp += Shape_MouseLeftButtonUp;
                        kontener.Children.Add(line);
                        line = null;
                    }
                    else MessageBox.Show("Proszę podać tylko liczby!");
                }
                else MessageBox.Show("Proszę podać tylko liczby!");
            }
            else if(ShapeRadio.IsChecked==true&&(x1.Text!=""&&y1.Text!=""))
            {
                if(obj.GetType()==typeof(Line))
                {
                    line = (Line)obj;
                    line.X1 = Convert.ToInt32(x1.Text.ToString());
                    line.X2 = Convert.ToInt32(x2.Text.ToString());
                    line.Y1 = Convert.ToInt32(y1.Text.ToString());
                    line.Y2 = Convert.ToInt32(y2.Text.ToString());
                }
                else if(obj.GetType() == typeof(Rectangle))
                {
                    shape = (Shape)obj;
                    Canvas.SetLeft(shape, Convert.ToInt32(x1.Text.ToString()));
                    Canvas.SetTop(shape, Convert.ToInt32(y1.Text.ToString()));
                    shape.Width = Convert.ToInt32(a.Text.ToString());
                    shape.Height = Convert.ToInt32(b.Text.ToString());
                }
                else
                {
                    shape = (Shape)obj;
                    Canvas.SetLeft(shape, Convert.ToInt32(x1.Text.ToString()));
                    Canvas.SetTop(shape, Convert.ToInt32(y1.Text.ToString()));
                    shape.Width = 2*(Convert.ToInt32(r.Text.ToString()));
                    shape.Height = 2*(Convert.ToInt32(r.Text.ToString()));
                }
            }
        }


        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DrawRadio.IsChecked == true)
            {
                line = null;
                prostokat = null;
                ellipse = null;
                Mouse.Capture(null);
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DrawRadio.IsChecked == true)
            {
                Mouse.Capture(kontener);
                pt = e.GetPosition(kontener);
                if (choose == "Prostokąt")
                {
                    prostokat = new Rectangle();
                    prostokat.Stroke = new SolidColorBrush(Colors.BlueViolet);
                    prostokat.StrokeThickness = 8;
                    Canvas.SetLeft(prostokat, pt.X);
                    Canvas.SetTop(prostokat, pt.Y);
                    prostokat.MouseDown += Shape_MouseLeftButtonDown;
                    prostokat.MouseMove += Shape_MouseMove;
                    prostokat.MouseUp += Shape_MouseLeftButtonUp;
                    kontener.Children.Add(prostokat);
                }
                else if (choose == "Koło")
                {
                    ellipse = new Ellipse();
                    ellipse.Stroke = new SolidColorBrush(Colors.BlueViolet);
                    ellipse.StrokeThickness = 8;
                    Canvas.SetLeft(ellipse, pt.X);
                    Canvas.SetTop(ellipse, pt.Y);
                    ellipse.MouseDown += Shape_MouseLeftButtonDown;
                    ellipse.MouseMove += Shape_MouseMove;
                    ellipse.MouseUp += Shape_MouseLeftButtonUp;
                    kontener.Children.Add(ellipse);
                }
                else if (choose == "Linia")
                {
                    line = new Line();
                    line.Stroke = new SolidColorBrush(Colors.BlueViolet);
                    line.StrokeThickness = 8;
                    line.X1 = pt.X;
                    line.X2 = pt.X;
                    line.Y1 = pt.Y;
                    line.Y2 = pt.Y;
                    line.MouseDown += Shape_MouseLeftButtonDown;
                    line.MouseMove += Shape_MouseMove;
                    line.MouseUp += Shape_MouseLeftButtonUp;
                    kontener.Children.Add(line);
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(kontener);
            position.Text = $"Pozycja: ({Convert.ToInt32(pos.X)}, {Convert.ToInt32(pos.Y)})";


            if (DrawRadio.IsChecked == true && e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.LeftButton == MouseButtonState.Released || (prostokat == null && line == null && ellipse == null)) return;
                if (choose == "Prostokąt")
                {
                    if (pos.X > pt.X)
                    {
                        prostokat.Width = (pos.X - pt.X);
                        Canvas.SetRight(prostokat, pos.X);
                    }
                    else
                    {
                        prostokat.Width = (pt.X - pos.X);
                        Canvas.SetLeft(prostokat, pos.X);
                        Canvas.SetRight(prostokat, pt.X);
                    }
                    if (pos.Y > pt.Y)
                    {
                        prostokat.Height = (pos.Y - pt.Y);
                        Canvas.SetBottom(prostokat, pt.Y);
                    }
                    else
                    {
                        prostokat.Height = (pt.Y - pos.Y);
                        Canvas.SetTop(prostokat, pos.Y);
                        Canvas.SetBottom(prostokat, pt.Y);
                    }
                }
                else if (choose == "Koło")
                {
                    if (pos.X > pt.X)
                    {
                        ellipse.Width = (pos.X - pt.X);
                        ellipse.Height = (pos.X - pt.X);
                        Canvas.SetRight(ellipse, pos.X);
                    }
                    else
                    {
                        ellipse.Width = (pt.X - pos.X);
                        ellipse.Height = (pt.X - pos.X);
                        Canvas.SetLeft(ellipse, pos.X);
                        Canvas.SetRight(ellipse, pt.X);
                    }
                    if (pos.Y > pt.Y)
                    {
                        ellipse.Width = (pos.Y - pt.Y);
                        ellipse.Height = (pos.Y - pt.Y);
                        //Canvas.SetBottom(ellipse, pt.Y);
                    }
                    else
                    {
                        ellipse.Height = (pt.Y - pos.Y);
                        ellipse.Width = (pt.Y - pos.Y);
                        Canvas.SetTop(ellipse, pos.Y);
                        //Canvas.SetBottom(ellipse, pt.Y);
                    }
                }
                else if (choose == "Linia")
                {
                    line.X2 = pos.X;
                    line.Y2 = pos.Y;
                }
            }
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Mouse.Capture(kontener);
            pt = e.GetPosition(kontener);
            obj = sender;
            shape = (Shape)sender;

            startPt = new Point(Canvas.GetLeft((Shape)sender), Canvas.GetTop((Shape)sender));
            Mouse.Capture((IInputElement)sender);
            ActualPoints(sender);
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(kontener);
            double diffx = pos.X - pt.X;
            double diffy = pos.Y - pt.Y;
            if (SelectRadio.IsChecked == true && e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender.GetType() == typeof(Line))
                {
                    line = (Line)sender;
                    line.X1 += diffx;
                    line.X2 += diffx;
                    line.Y1 += diffy;
                    line.Y2 += diffy;
                    pt = pos;
                }
                else
                {
                    Canvas.SetLeft(shape, startPt.X + diffx);
                    Canvas.SetTop(shape, startPt.Y + diffy);
                }
                ActualPoints(sender);
            }
            else if (ShapeRadio.IsChecked == true && e.LeftButton == MouseButtonState.Pressed)
            {
                shape = (Shape)sender;
                if (sender.GetType() == typeof(Line))
                {
                    line = (Line)sender;
                    if (Math.Abs(line.X1 - pos.X) < Math.Abs(line.X2 - pos.X)) line.X1 += diffx;
                    else line.X2 += diffx;
                    if (Math.Abs(line.Y1 - pos.Y) < Math.Abs(line.Y2 - pos.Y)) line.Y1 += diffy;
                    else line.Y2 += diffy;
                }
                else
                {
                    double left = Canvas.GetLeft((Shape)sender);
                    double right = startPt.X + ((Shape)sender).Width;
                    double top = Canvas.GetTop((Shape)sender);
                    double bottom = startPt.Y + ((Shape)sender).Height;

                    double odc1 = Math.Sqrt(Math.Pow(pt.X - left, 2) + Math.Pow(pt.Y - top, 2));
                    double odc2 = Math.Sqrt(Math.Pow(pt.X - right, 2) + Math.Pow(pt.Y - bottom, 2));

                    if (odc1 < odc2)
                    {
                        var newX = startPt.X + diffx;
                        var newY = startPt.Y + diffy;
                        Canvas.SetLeft(shape, newX);
                        Canvas.SetTop(shape, newY);
                        shape.Width = Math.Abs(right - newX);
                        shape.Height = Math.Abs(bottom - newY);

                        startPt = new Point(newX, newY);
                    }
                    else
                    {
                        shape.Width += diffx;
                        shape.Height += diffy;
                    }
                }
                ActualPoints(sender);
                pt = pos;
            }
        }

        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ActualPoints(sender);
            Mouse.Capture(null);
        }
    }
}
