﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphSimilarityByMatching;

namespace DoppleWebDemo.Controllers.Helpers
{
    public class EdgeForJS
    {
        public int from { get; set; }
        public int to { get; set; }
        public string color { get; set; }
        public EdgeType type { get; internal set; }
        public bool isLayoutPositioned { get; internal set; }
    }
}