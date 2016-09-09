using DebatePlatform.Models;
using Microsoft.AspNetCore.Html;

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
                    htmlSoFar += "<li>" + child.Text + "</li>"+
                    "<a href='/Arguments/Create/" + child.ArgumentId.ToString() + "'>Respond</a>"+
                    "<br>"+
                    "<a href='/Arguments/Edit/" + child.ArgumentId.ToString() + "'>Edit</a>";
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
