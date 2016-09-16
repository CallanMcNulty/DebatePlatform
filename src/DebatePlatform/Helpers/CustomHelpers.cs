using DebatePlatform.Models;
using Microsoft.AspNetCore.Html;
using System;

namespace DebatePlatform.Helpers
{
    public class CustomHelpers
    {
        public static string DisplayChildrenRecursion(Argument argument, int userType)
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
                        "<div class='argument-container'>"+
                            "<div class='argument "+(child.IsAffirmative ? "aff" : "neg")+"'>"+
                                child.Text+" Base Strength: "+child.Strength.ToString()+" Total Strength: "+child.GetTotalStrength().ToString()+
                                (userType==0 ? "" :
                                    "<form action='/Arguments/Vote/"+child.ArgumentId.ToString()+"' method='post'><button>I'm Convinced</button></form>"+
                                    "<a href='/Arguments/Create/" + child.ArgumentId.ToString() + "'>Respond</a>"+
                                    "<br>"+
                                    (userType==1 ? "" :
                                        "<a href='/Arguments/Edit/" + child.ArgumentId.ToString() + "'>Edit</a>"
                                    )
                                )+
                            "</div>"+
                        "</div>"+
                        DisplayChildrenRecursion(child, userType)+
                    "</div>";
                }
                htmlSoFar += "</div>";
            }
            return htmlSoFar;
        }
        public static HtmlString DisplayChildren(Argument argument, int userType)
        {
            return new HtmlString(DisplayChildrenRecursion(argument, userType));
        }

        public static HtmlString BeginTreeContainer(Argument argument)
        {
            int totalWidth = (int)(200F / argument.GetMinWidth(1F));
            return new HtmlString("<div style='min-width:"+totalWidth.ToString()+"px'>");
        }
    }
}
