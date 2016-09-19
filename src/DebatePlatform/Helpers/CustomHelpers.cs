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
                                "<p>"+child.GetTotalStrength().ToString()+"</p><form class='float-right' action='/Arguments/Vote/" + child.ArgumentId.ToString()+"' method='post'><button>I'm Convinced</button></form>"+
                                "<div class='arg-text'>"+child.Text+"</div>"+
                                (userType==0 ? "" : 
                                    "<a class='float-left' href='/Arguments/Create/" + child.ArgumentId.ToString() + "'>Respond</a>"+
                                    (userType==1 ? "" :
                                        "<a class='float-right' href='/Arguments/Edit/" + child.ArgumentId.ToString() + "'>Edit</a>"
                                    )
                                )+
                                "<a class='float-right' href='/Arguments/Details/"+child.ArgumentId.ToString()+"'>View</a>"+
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
