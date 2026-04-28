using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Document
{
    public class DocumentAnnotation
    {
        public List<drawing> drawing { get; set; }
        public List<textbox> textbox { get; set; }
        public List<strikeout> strikeout { get; set; }
        public List<highlight> highlight { get; set; }
        public List<area> area { get; set; }
        public DocumentAnnotation()
        {
            drawing = new List<drawing>();
            textbox = new List<textbox>();
            strikeout = new List<strikeout>();
            highlight = new List<highlight>();
            area = new List<area>();
        }
    }

    public class area
    {
        public double height { get; set; }
        public double width { get; set; }
        public int page { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }
    public class highlight
    {
        public string color { get; set; }
        public int page { get; set; }
        public List<rectangle> rectangles { get; set; }
        public highlight()
        {
            rectangles = new List<rectangle>();
        }
    }
    public class strikeout
    {
        public string color { get; set; }
        public int page { get; set; }
        public List<rectangle> rectangles { get; set; }
        public strikeout()
        {
            rectangles = new List<rectangle>();
        }
    }
    public class rectangle
    {
        public double height { get; set; }
        public double width { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }
    public class textbox
    {
        public string color { get; set; }
        public int page { get; set; }
        public string content { get; set; }
        public int size { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }
    public class drawing
    {
        public string color { get; set; }
        public int width { get; set; }
        public int page { get; set; }
        public List<List<double>> lines { get; set; }

        public drawing()
        {
             lines = new List<List<double>>();
        }
    }
}
