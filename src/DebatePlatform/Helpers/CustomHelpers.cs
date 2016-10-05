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
                            "<div id='"+child.ArgumentId+"' class='argument "+(child.IsCitation ? "cite" : (child.IsAffirmative ? "aff" : "neg"))+"'>"+
                                "<p>"+child.GetTotalStrength().ToString()+ "</p>" + (userType < 1 ? "" : "<a class='vote-button float-right' id='vote"+child.ArgumentId+"'>I'm Convinced</a>")+
                                "<div class='arg-text'>"+(child.IsCitation ? "<strong>Citation: </strong>" : "")+child.Text+"</div>"+
                                (userType<1 ? "" : 
                                    "<a class='float-left' href='/Arguments/Create/" + child.ArgumentId.ToString() + "'>Respond</a>"+
                                    (userType==1 ? "" :
                                        "<a class='float-right' href='/Arguments/Edit/" + child.ArgumentId.ToString() + "'>Edit</a>"
                                    )
                                )+
                                (userType<0 ? "" :
                                    "<a class='float-right' href='/Arguments/Details/"+child.ArgumentId.ToString()+"'>View</a>"
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

        public static HtmlString DescribeEdit(ProposedEdit edit)
        {
            string votes = "<span class='strength float-right'>Votes: " + edit.Votes.ToString() + "</span>";
            string result = "<h4>";
            if(edit.Text != null)
            {
                result += "Edit text to: "+votes+"<h4><p>" + edit.Text + "</p>";
                votes = "";
            }
            if (edit.IsDelete)
            {
                result += "Delete Argument" + votes + "</h4>";
                votes = "";
            }
            if (edit.ParentId != 0)
            {
                result += "Make this a response to Argument #" + edit.ParentId.ToString() + votes + "</h4>";
                votes = "";
            }
            if (result.Length < 5)
            {
                result += "Change to a "+(edit.IsAffirmative ? "supporting" : "opposing")+ " argument" + votes + "</h4>";
                votes = "";
            }
            if(edit.Reason != null)
            {
                result += "<h4>Because: </h4><p>" + edit.Reason + "</p>";
            }
            return new HtmlString(result);
        }
    }
}
