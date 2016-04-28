using System;
using System.Collections;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace TestForIronScheme
{

    using IronScheme; // the extension methods are exported from this namespace
    using Newtonsoft.Json;
    public class SchemeHandler
    {
        public object Evaluate(string input)
        {
            return input.Eval(); // calls IronScheme.RuntimeExtensions.Eval(string)
        }
    }
    public partial class Window1 : Window
    {
        private SchemeHandler schemeHandler;
        public Window1()
        {
            InitializeComponent();
            schemeHandler = new SchemeHandler();
            var path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string text = System.IO.File.ReadAllText(path +@"SchemeCode.ss");
            schemeHandler.Evaluate(text);
            
        }

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (/*(Keyboard.Modifiers == ModifierKeys.Control) && */(e.Key == Key.Enter))
            {
                try
                {
                    
                    double x = 0;
                    double y = 0;
                    var val = schemeHandler.Evaluate(Input.Text);
                    DisplayArea.Text = val.ToString();
                    foreach (var cord in (IEnumerable) val)
                    {
                        int count = 0;
                        foreach (var numb in (IEnumerable) cord)
                        {
                            if (count == 0)
                            {
                                x = Convert.ToDouble(numb.ToString());
                                count++;
                            }
                            else
                            {
                                y = Convert.ToDouble(numb.ToString());
                                count++;
                            }
                        }
                        makeDot(x, y);
                    }
                   
                }
                catch(Exception ex)
                {
                    var json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                    DisplayArea.Text = json;
                }
            }
        }

        private void makeDot(double x, double y)
        {
            var centerX = myCanvas.ActualWidth / 2;
            var centerY = myCanvas.ActualHeight / 2;

            var myLine1 = new Line();
            myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine1.X1 = centerX - 30;
            myLine1.X2 = centerX + 30;
            myLine1.Y1 = centerY + 0;
            myLine1.Y2 = centerY + 0;

            myLine1.StrokeThickness = 1;
            myCanvas.Children.Add(myLine1);

            var myLine2 = new Line();
            myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine2.X1 = centerX + 0;
            myLine2.X2 = centerX + 0;
            myLine2.Y1 = centerY - 30;
            myLine2.Y2 = centerY + 30;

            myLine2.StrokeThickness = 1;
            myCanvas.Children.Add(myLine2);

            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Colors.Red;
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.White;
            myEllipse.Width = 5;
            myEllipse.Height = 5;
            Canvas.SetTop(myEllipse, centerY - y);
            Canvas.SetLeft(myEllipse, centerX + x);
            myCanvas.Children.Add(myEllipse);
        }
    }
}
