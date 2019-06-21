using System.Collections.Generic;

namespace InterviewTest.BLL.Models
{
    //public class Tag
    //{
    //    public string name { get; set; }
    //    public double confidence { get; set; }
    //}

    //public class Metadata
    //{
    //    public int width { get; set; }
    //    public int height { get; set; }
    //    public string format { get; set; }
    //}

    //public class ImageTagResponse
    //{
    //    public List<Tag> tags { get; set; }
    //    public string requestId { get; set; }
    //    public Metadata metadata { get; set; }
    //}

    public class Color
    {
        public string dominantColorForeground { get; set; }
        public string dominantColorBackground { get; set; }
        public List<string> dominantColors { get; set; }
        public string accentColor { get; set; }
        public bool isBwImg { get; set; }
        public bool isBWImg { get; set; }
    }

    public class Caption
    {
        public string text { get; set; }
        public double confidence { get; set; }
    }

    public class Description
    {
        public List<string> tags { get; set; }
        public List<Caption> captions { get; set; }
    }

    public class Rectangle
    {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Parent3
    {
        public string @object { get; set; }
        public double confidence { get; set; }
    }

    public class Parent2
    {
        public string @object { get; set; }
        public double confidence { get; set; }
        public Parent3 parent { get; set; }
    }

    public class Parent
    {
        public string @object { get; set; }
        public double confidence { get; set; }
        public Parent2 parent { get; set; }
    }

    public class Object
    {
        public Rectangle rectangle { get; set; }
        public string @object { get; set; }
        public double confidence { get; set; }
        public Parent parent { get; set; }
    }

    public class Metadata
    {
        public int width { get; set; }
        public int height { get; set; }
        public string format { get; set; }
    }

    public class ImageTagResponse
    {
        public List<object> categories { get; set; }
        public Color color { get; set; }
        public Description description { get; set; }
        public List<object> faces { get; set; }
        public List<Object> objects { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
        public int ImageId { get; set; }
    }
}
