using DebatePlatform.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebatePlatform.Helpers
{
    public class CustomHelpers
    {
        public static HtmlString DisplayChildren(Argument argument, string htmlSoFar)
        {
            htmlSoFar += "<ul>";
            foreach(Argument child in argument.Children)
            {
                
            }
        }
    }
}
