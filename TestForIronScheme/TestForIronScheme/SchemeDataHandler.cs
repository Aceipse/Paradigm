using IronScheme.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestForIronScheme
{
    //((ERROR),(errorString))
    //((TEXT),(textString),(x,y))
    //((FIGURE),(colorString),((x1,y1),(x2,y2))) ...

    public abstract class SchemeDataHandler
    {
        protected Window1 w { get; }
        public SchemeDataHandler(Window1 window)
        {
            w = window;
        }

        protected Coordinate offsetPoint(double x, double y)
        {
            var centerX = w.myCanvas.ActualWidth / 2;
            var centerY = w.myCanvas.ActualHeight / 2;
            return new Coordinate(centerX + x, centerY - y);
        }

        public abstract void Handle(Cons data);
    }

    public class ErrorHandler : SchemeDataHandler
    {
        public ErrorHandler(Window1 window) : base(window) { }

        public override void Handle(Cons data)
        {
            w.Log(data.car.ToString());
        }
    }

    public class TextHandler : SchemeDataHandler
    {
        public TextHandler(Window1 window) : base(window) { }

        public override void Handle(Cons data)
        {
            List<object> list = data.ToList();
            List<object> coor = ((Cons)list[1]).ToList();

            double x = Double.Parse(coor[0].ToString());
            double y = Double.Parse(coor[1].ToString());

            Coordinate offsetpoint = offsetPoint(x, y);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = ((Cons)list[0]).car.ToString();
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(textBlock, offsetpoint.x);
            Canvas.SetTop(textBlock, offsetpoint.y);
            w.myCanvas.Children.Add(textBlock);
        }
    }

    public class FigureHandler : SchemeDataHandler
    {
        public FigureHandler(Window1 window) : base(window) { }

        public override void Handle(Cons data)
        {
            double x = 0;
            double y = 0;

            string colorStr = (string)data.car;
            Color color;

            try
            {
               color = (Color)ColorConverter.ConvertFromString(colorStr);
            }
            catch (Exception e)
            {
                throw new Exception("Color-string '" + colorStr + "' could not be converted to a known color. See inner exception", e);
            }

            foreach (var cord in (IEnumerable)data.cdr)
            {
                int count = 0;
                foreach (var numb in (IEnumerable)cord)
                {
                    if (count == 0)
                    {
                        x = Convert.ToDouble(numb.ToString());
                    }
                    else
                    {
                        y = Convert.ToDouble(numb.ToString());
                    }
                    count++;
                }
                makeDot(x, y, color);
            }
        }

        private void makeDot(double x, double y, Color color)
        {
            /*Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Colors.Red;
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.White;
            myEllipse.Width = 5;
            myEllipse.Height = 5;
            Canvas.SetTop(myEllipse, centerY - y);
            Canvas.SetLeft(myEllipse, centerX + x);
            myCanvas.Children.Add(myEllipse);*/

            var offsetpoint = offsetPoint(x, y);

            Rectangle myRec = new Rectangle();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = color;
            myRec.Fill = mySolidColorBrush;
            myRec.StrokeThickness = 1;
            myRec.Width = 1;
            myRec.Height = 1;
            Canvas.SetLeft(myRec, offsetpoint.x);
            Canvas.SetTop(myRec, offsetpoint.y);
            w.myCanvas.Children.Add(myRec);
        }
    }
}
