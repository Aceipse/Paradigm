﻿using IronScheme.Runtime;
using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestForIronScheme
{
    //ERROR,errorString
    //TEXT,textString,x,y
    //FIGURE,colorString,x1,y1,x2,y2 ...

    public abstract class SchemeDataHandler
    {
        protected Window1 w { get; }
        public SchemeDataHandler(Window1 window)
        {
            w = window;
        }

        public abstract void Handle(Cons data);
    }

    public class ErrorHandler : SchemeDataHandler
    {
        public ErrorHandler(Window1 window) : base(window) { }

        public override void Handle(Cons data)
        {
            throw new NotImplementedException();
        }
    }

    public class TextHandler : SchemeDataHandler
    {
        public TextHandler(Window1 window) : base(window) { }

        public override void Handle(Cons data)
        {
            throw new NotImplementedException();
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
            var centerX = w.myCanvas.ActualWidth / 2;
            var centerY = w.myCanvas.ActualHeight / 2;

            var myLine1 = new Line();
            myLine1.Stroke = Brushes.LightSteelBlue;
            myLine1.X1 = 0;
            myLine1.X2 = w.myCanvas.ActualWidth;
            myLine1.Y1 = w.myCanvas.ActualHeight / 2;
            myLine1.Y2 = w.myCanvas.ActualHeight / 2;

            myLine1.StrokeThickness = 1;
            w.myCanvas.Children.Add(myLine1);

            var myLine2 = new Line();
            myLine2.Stroke = Brushes.LightSteelBlue;
            myLine2.X1 = w.myCanvas.ActualWidth / 2;
            myLine2.X2 = w.myCanvas.ActualWidth / 2;
            myLine2.Y1 = 0;
            myLine2.Y2 = w.myCanvas.ActualHeight;

            myLine2.StrokeThickness = 1;
            w.myCanvas.Children.Add(myLine2);

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

            Rectangle myRec = new Rectangle();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = color;
            myRec.Fill = mySolidColorBrush;
            myRec.StrokeThickness = 1;
            myRec.Width = 1;
            myRec.Height = 1;
            Canvas.SetTop(myRec, centerY - y);
            Canvas.SetLeft(myRec, centerX + x);
            w.myCanvas.Children.Add(myRec);
        }
    }
}
