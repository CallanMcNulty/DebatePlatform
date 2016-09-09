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
        public static string DisplayChildrenRecursion(Argument argument)
        {
            string htmlSoFar = "";
            if(argument.Children.Count > 0)
            {
                htmlSoFar += "<ul>";
                foreach (Argument child in argument.Children)
                {
                    htmlSoFar += "<li>" + child.Text + "</li>";
                    htmlSoFar += DisplayChildrenRecursion(child);
                }
                htmlSoFar += "</ul>";
            }
            return htmlSoFar;
        }
        public static HtmlString DisplayChildren(Argument argument)
        {
            return new HtmlString(DisplayChildrenRecursion(argument));
        }
    }
}
