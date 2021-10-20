using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Assignment04
{
    class Program
    {
        static void Main(string[] args)
        {
            //interfaces are types.
            List<IDrawable> face = new List<IDrawable>();
            face.Add(new DrawableEllipse(Color.Yellow, true, new Rectangle(75, 160, 40, 70))); //left ear
            face.Add(new DrawableEllipse(Color.BlueViolet, false, new Rectangle(75, 160, 40, 70))); //left ear
            face.Add(new DrawableEllipse(Color.Yellow, true, new Rectangle(285, 160, 40, 70))); //right ear
            face.Add(new DrawableEllipse(Color.BlueViolet, false, new Rectangle(285, 160, 40, 70)));//right ear
            face.Add(new DrawableRectangle(Color.Salmon, true, new Rectangle(100, 100, 200, 300))); //face
            face.Add(new DrawableRectangle(Color.White, true, new Rectangle(140, 165, 45, 60))); //right eye
            face.Add(new DrawableRectangle(Color.White, true, new Rectangle(220, 165, 45, 60))); //left eye
            face.Add(new DrawableRectangle(Color.Black, true, new Rectangle(150, 183, 25, 40))); //right pupil
            face.Add(new DrawableRectangle(Color.Black, true, new Rectangle(230, 183, 25, 40))); //left pupil
            face.Add(new DrawableRectangle(Color.Brown, true, new Rectangle(90, 10, 220, 120))); //hat top
            face.Add(new DrawableRectangle(Color.Brown, true, new Rectangle(10, 100, 380, 20))); //hat rim
            face.Add(new DrawableBezier(Color.Black, new Point(195, 240), new Point(135, 280), new Point(275, 280), new Point(215, 240))); //nose
            face.Add(new DrawableEllipse(Color.Red, false, new Rectangle(150, 300, 100, 35))); //lips
            face.Add(new DrawableEllipse(Color.Wheat, true, new Rectangle(160, 305, 80, 25))); //mouth
            face.Add(new DrawableLine(Color.Red, new Point(105, 30), new Point(105, 100))); //lines
            face.Add(new DrawableLine(Color.Orange, new Point(127, 30), new Point(127, 100))); //lines
            face.Add(new DrawableLine(Color.Yellow, new Point(155, 30), new Point(155, 100))); //lines
            face.Add(new DrawableLine(Color.Green, new Point(200, 30), new Point(200, 100))); //lines
            face.Add(new DrawableLine(Color.Blue, new Point(245, 30), new Point(245, 100))); //lines
            face.Add(new DrawableLine(Color.Indigo, new Point(273, 30), new Point(273, 100))); //lines
            face.Add(new DrawableLine(Color.Violet, new Point(295, 30), new Point(295, 100))); //lines
            int width = 400;
            int length = 450;
            Bitmap bitmap = new Bitmap(width, length);
            Graphics graphic = Graphics.FromImage(bitmap);
            foreach (var item in face)
            {
                item.Draw(graphic);
                if (item is IWritable)
                {
                    ((IWritable)item).Write(System.Console.Out);
                }
            }
            graphic.Dispose();
            bitmap.Save("person.png", ImageFormat.Png);
            bitmap.Dispose();
            Console.ReadKey();
        }
    }

    interface IDrawable
    {
        void Draw(Graphics g);
    }

    interface IWritable
    {
        void Write(TextWriter writer);
    }

    abstract class Primitive
    {
        protected Color color;
        protected bool filled;
        protected Rectangle boundingRectangle;

        public Primitive(Color color, bool filled, Rectangle rectangle)
        {
            this.color = color;
            this.filled = filled;
            boundingRectangle = rectangle;
        }
    }

    class DrawableRectangle : Primitive, IDrawable, IWritable
    {
        public DrawableRectangle(Color color, bool filled, Rectangle rectangle) : base(color, filled, rectangle)
        {
        }

        public void Draw(Graphics g)
        {
            if (filled)
            {
                SolidBrush solidBrush = new SolidBrush(color);
                g.FillRectangle(solidBrush, boundingRectangle);
            }
            else
            {
                Pen pen = new Pen(color);
                g.DrawRectangle(pen, boundingRectangle);
            }
        }

        public void Write(TextWriter writer)
        {
            writer.WriteLine($"{color,16}; {"Filled?:",10} {filled,4};  {"Rectangle:",12}  {boundingRectangle,20}");

        }
        public double Area()
        {
            return boundingRectangle.Height * boundingRectangle.Width;
      
        }
    }

    class DrawableEllipse : Primitive, IDrawable
    {
        public DrawableEllipse(Color color, bool filled, Rectangle rectangle) : base(color, filled, rectangle)
        {
        }

        public void Draw(Graphics g)
        {
            if (filled)
            {
                SolidBrush solidBrush = new SolidBrush(color);
                g.FillEllipse(solidBrush, boundingRectangle);
            }
            else
            {
                Pen pen = new Pen(color);
                g.DrawEllipse(pen, boundingRectangle);
            }
        }

        public double Area()
        {
            return boundingRectangle.Width * boundingRectangle.Height * Math.PI / 4;
        }
    }

    class DrawableLine : IDrawable, IWritable
    {
        protected Color color;
        protected Point lineStart;
        protected Point lineEnd;

        public DrawableLine(Color color, Point start, Point end)
        {
            this.color = color;
            lineStart = start;
            lineEnd = end;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(color);
            g.DrawLine(pen, lineStart, lineEnd);
        }

        public void Write(TextWriter writer)
        {
            writer.WriteLine($"{color,16}; {"Line Start:",13} {lineStart}; {"Line End:",12} {lineEnd}");
        }
    }

    class DrawableBezier : IDrawable, IWritable
    {
        protected Color color;
        protected Point curveStart;
        protected Point controlFirst;
        protected Point controlSecond;
        protected Point curveEnd;

        public DrawableBezier(Color color, Point start, Point first, Point second, Point end)
        {
            this.color = color;
            curveStart = start;
            controlFirst = first;
            controlSecond = second;
            curveEnd = end;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(color);
            g.DrawBezier(pen, curveStart, controlFirst, controlSecond, curveEnd);
        }

        public void Write(TextWriter writer)
        {
            writer.WriteLine($"\n {color,15}; {"Bezier Start:",12} {curveStart}; Control Point 1: {controlFirst}; Control Point 2: {controlSecond}; Bezier End:  {curveEnd} \n");
        }
    }

    class DrawableArc : Primitive, IDrawable, IWritable
    {
        protected float start;
        protected float end;

        public DrawableArc(Color color, bool filled, Rectangle rectangle, float start, float end) : base(color, filled, rectangle)
        {
            this.start = start;
            this.end = end;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(color);
            g.DrawArc(pen, boundingRectangle,start, end);
        }
        public void Write(TextWriter writer)
        {
            writer.WriteLine($"{ color}, {filled}, {boundingRectangle}, {start}, {end}");
        }
    }
}

