using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using CollectIt.Web.ItemService;

namespace CollectIt.Web.Models.ViewModels
{
    public class MemberIndexViewModel
    {
        public ICollection<ItemContract> Items { get; set; }

        public string StripHtml(string input)
        {
            var regex = new Regex("<[^>]*(>|$)");
            return regex.Replace(input, "");
        }
    }
}