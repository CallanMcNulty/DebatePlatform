using DebatePlatform.Models;
using Microsoft.AspNetCore.Html;
using System;

namespace DebatePlatform.Helpers
{
    public class CustomHelpers
    {
        public static string DisplayChildrenRecursion(Argument argument)
        {
            string htmlSoFar = "";
            if(argument.Children.Count > 0)
            {
                string percent = (100 / argument.Children.Count).ToString();
                htmlSoFar += "<div class='outer-container'>";
                foreach (Argument child in argument.Children)
                {
                    htmlSoFar += 
                    "<div style='width:"+percent+"%;' class='inner-container'>" +
                        "<div class='connector-container'>" +
                            "<div class='connector'></div>"+
                        "</div>"+
                        "<div class='argument "+(child.IsAffirmative ? "aff" : "neg")+"'>"+
                            child.Text+" Base Strength: "+child.Strength.ToString()+" Total Strength: "+child.GetTotalStrength().ToString()+
                            "<form action='/Arguments/Vote/"+child.ArgumentId.ToString()+"' method='post'><button>I'm Convinced</button></form>"+
                            "<a href='/Arguments/Create/" + child.ArgumentId.ToString() + "'>Respond</a>"+
                            "<br>"+
                            "<a href='/Arguments/Edit/" + child.ArgumentId.ToString() + "'>Edit</a>"+
                        "</div>"+
                        DisplayChildrenRecursion(child)+
                    "</div>";
                }
                htmlSoFar += "</div>";
            }
            return htmlSoFar;
        }
        public static HtmlString DisplayChildren(Argument argument)
        {
            return new HtmlString(DisplayChildrenRecursion(argument));
        }
        public static HtmlString BeginTreeContainer(Argument argument)
        {
            int totalWidth = (int)(200F / argument.GetMinWidth(1F));
            return new HtmlString("<div style='min-width:"+totalWidth.ToString()+"px'>");
        }
    }
}
