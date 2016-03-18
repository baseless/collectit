using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CollectIt.Web.ItemService;

namespace CollectIt.Web.Models.ViewModels
{
    public class MemberSearchViewModel
    {
        public MemberSearchViewModel()
        {
            Items = new List<ItemContract>();
        }

        [DisplayName("Search keywords"), RegularExpression("^(([0-9a-zA-Z][0-9a-zA-Z_]*)([,][0-9a-zA-Z][0-9a-zA-Z_]*)*)$", ErrorMessage = "Invalid format. Must be a comma-separated string without special characters!")]
        public string Filters { get; set; }

        public ICollection<ItemContract> Items { get; set; }

        public string StripHtml(string input)
        {
            var regex = new Regex("<[^>]*(>|$)");
            return regex.Replace(input, "");
        }
    }
}