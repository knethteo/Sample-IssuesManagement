using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet_B2C.General
{
    public class Sentiment
    {
        
        public List<documents> docs { get; set; }
        //public List<errors> errrs { get; set; }


    }

    public class documents
    {
        public string score { get; set; }
        public int id { get; set; }
    }

    //public class errors
    //{

    //}
}
    