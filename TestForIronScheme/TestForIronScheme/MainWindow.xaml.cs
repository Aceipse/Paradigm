using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace TestForIronScheme
{
    using IronScheme; // the extension methods are exported from this namespace
    using IronScheme.Runtime;
    using Newtonsoft.Json;
    using System.Windows.Media;
    using System.Windows.Shapes;
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
            drawGrid();
            schemeHandler = new SchemeHandler();
            var path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string text = System.IO.File.ReadAllText(path + @"SchemeCode.ss");
            schemeHandler.Evaluate(text);

        }

        private void drawGrid()
        {
            var myLine1 = new Line();
            myLine1.Stroke = Brushes.LightSteelBlue;
            myLine1.X1 = 0;
            myLine1.X2 = myCanvas.ActualWidth;
            myLine1.Y1 = myCanvas.ActualHeight / 2;
            myLine1.Y2 = myCanvas.ActualHeight / 2;

            myLine1.StrokeThickness = 1;
            myCanvas.Children.Add(myLine1);

            var myLine2 = new Line();
            myLine2.Stroke = Brushes.LightSteelBlue;
            myLine2.X1 = myCanvas.ActualWidth / 2;
            myLine2.X2 = myCanvas.ActualWidth / 2;
            myLine2.Y1 = 0;
            myLine2.Y2 = myCanvas.ActualHeight;

            myLine2.StrokeThickness = 1;
            myCanvas.Children.Add(myLine2);
        }

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightShift)
            {
                for (int i = 0; i < Input.LineCount; i++)
                {
                    //try
                    //{
                        var val = schemeHandler.Evaluate("(EvalFunc '" + Input.GetLineText(i) + ")");
                        if (val.GetType()!= typeof(Cons))
                        {
                            continue;
                        }
                        //Log(val.ToString());

                        Cons valueList = (Cons)val;
                        string datatype = ((Cons)valueList.car).car.ToString();

                        SchemeDataHandler schemeWorker;
                        switch (datatype)
                        {
                            case "ERROR":
                                schemeWorker = new ErrorHandler(this);
                                break;
                            case "TEXT":
                                schemeWorker = new TextHandler(this);
                                break;
                            case "FIGURE":
                                schemeWorker = new FigureHandler(this);
                                break;
                            default:
                                throw new Exception("datatype '" + datatype + "' received from scheme is not supported");
                        }

                        Cons dataList = (Cons)valueList.cdr;
                        schemeWorker.Handle(dataList);
                    //}
                    //catch (Exception ex)
                    //{
                    //    try
                    //    {
                    //        var json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                    //        Log(json);
                    //    }
                    //    catch (Exception)
                    //    {
                    //        Log(ex.Message);
                    //    }
                    //}
                }
            }
            else if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                myCanvas.Children.Clear();
                drawGrid();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myCanvas.Children.Clear();
            drawGrid();
        }

        public void Log(string txt)
        {
            DisplayArea.Text = DisplayArea.Text.Insert(0, "\n");
            DisplayArea.Text = DisplayArea.Text.Insert(0, txt);
            DisplayArea.Text = DisplayArea.Text.Insert(0, "\n");
            DisplayArea.Text = DisplayArea.Text.Insert(0, "------------NEW ENTRY " + DateTime.Now.ToLongTimeString());
        }
    }
}
