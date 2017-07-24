using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace makemesmarter.Models
{
    public class ChartObject
    {
        public Chart chart { get; set; }
        public Datum[] data { get; set; }
        public Trendline[] trendlines { get; set; }
        public Styles styles { get; set; }
    }

    public class Chart
    {
        public string caption { get; set; }
        public string xaxisname { get; set; }
        public string yaxisname { get; set; }
        public string numberprefix { get; set; }
        public string showvalues { get; set; }
        public string animation { get; set; }
    }

    public class Styles
    {
        public Definition[] definition { get; set; }
        public Application[] application { get; set; }
    }

    public class Definition
    {
        public string name { get; set; }
        public string type { get; set; }
        public string param { get; set; }
        public string start { get; set; }
        public string duration { get; set; }
    }

    public class Application
    {
        public string toobject { get; set; }
        public string styles { get; set; }
    }

    public class Datum
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class Trendline
    {
        public Line[] line { get; set; }
    }

    public class Line
    {
        public string startvalue { get; set; }
        public string istrendzone { get; set; }
        public string valueonright { get; set; }
        public string tooltext { get; set; }
        public string endvalue { get; set; }
        public string color { get; set; }
        public string displayvalue { get; set; }
        public string showontop { get; set; }
        public string thickness { get; set; }
    }

    public class ChartModel
    {

    }
}