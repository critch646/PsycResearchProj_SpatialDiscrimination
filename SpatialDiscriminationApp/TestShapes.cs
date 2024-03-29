﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using SpatialDiscriminationApp.enums;

namespace SpatialDiscriminationApp
{
    public abstract class TestShape
    { 
        public abstract string Name { get; }
        public abstract string Uid { get; set; }

        public abstract Brush Stroke { get; }

        public abstract Brush Fill { get; }

        public abstract int Width { get; set; }
        public abstract int Height { get; set; }

    }

    public class FixationShape : TestShape
    {
        public override string Name => nameof(FixationShape);

        public override string Uid { get ; set; }

        public override Brush Stroke => Brushes.Black;

        public override Brush Fill => Brushes.Black;

        public override int Width { get; set; }

        public override int Height { get; set; }

        public PointCollection Points { get; private set; }

        public Polygon Shape { get; private set; }

        public FixationShape(string Uid) 
        { 
            this.Uid = Uid;

            Width = 120;
            Height = 120;

            //Point Point1 = new Point(0, 40);
            //Point Point2 = new Point(40, 40);
            //Point Point3 = new Point(40, 0);
            //Point Point4 = new Point(80, 0);
            //Point Point5 = new Point(80, 40);
            //Point Point6 = new Point(120, 40);
            //Point Point7 = new Point(120, 80);
            //Point Point8 = new Point(80, 80);
            //Point Point9 = new Point(80, 120);
            //Point Point10 = new Point(40, 120);
            //Point Point11 = new Point(40, 80);
            //Point Point12 = new Point(0, 80);

            this.Points = new PointCollection();

            this.Points.Add(new Point(0, 40));
            this.Points.Add(new Point(40, 40));
            this.Points.Add(new Point(40, 0));
            this.Points.Add(new Point(80, 0));
            this.Points.Add(new Point(80, 40));
            this.Points.Add(new Point(120, 40));
            this.Points.Add(new Point(120, 80));
            this.Points.Add(new Point(80, 80));
            this.Points.Add(new Point(80, 120));
            this.Points.Add(new Point(40, 120));
            this.Points.Add(new Point(40, 80));
            this.Points.Add(new Point(0, 80));

            this.Shape = new Polygon();
            this.Shape.Points = this.Points;
            this.Shape.Stroke = this.Stroke;
            this.Shape.Fill = this.Fill;

            this.Shape.Uid = this.Uid;


        }
    }

    public class TargetShape : TestShape
    {
        public override string Name => nameof(TargetShape);

        public override string Uid { get; set; }

        public override Brush Stroke => Brushes.Black;

        public override Brush Fill => Brushes.Black;

        public override int Width { get; set; }

        public override int Height { get; set; }

        public Ellipse Shape { get; private set; }

        public StimSize StimSize { get; private set; }

        public TargetShape(string Uid, StimSize stimSize)
        {
            this.Uid = Uid;
            this.StimSize = stimSize;

            int val = CalcTargetSize();

            Shape = new Ellipse();
            Shape.Width = val;
            Shape.Height = val;
            Width = val;
            Height = val;
            Shape.Uid = this.Uid;
            Shape.Stroke = this.Stroke;
            Shape.Fill = this.Fill;



        }

        private int CalcTargetSize()
        {
            return 100 + (int)this.StimSize * 5;
        }
    }

    public class MaskShape : TestShape
    {
        public override string Name => nameof(MaskShape);

        public override string Uid { get; set; }

        public override Brush Stroke => Brushes.Black;

        public override Brush Fill => Brushes.Gray;

        public override int Width { get; set; }

        public override int Height { get; set; }

        public Rectangle Shape { get; private set; }

        public MaskShape(string uid, int size)
        {
            Uid = uid;
            Width = size;
            Height = size;

            Shape = new Rectangle { 
                Width = size, 
                Height = size,
                Stroke = this.Stroke,
                Fill = this.Fill,
                Uid = uid
            };

        }
    }
}
