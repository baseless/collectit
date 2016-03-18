using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectIt.Web.Models.ViewModels
{
    public class SubscribeViewModel
    {
        [Required, DisplayName("Channel Name")]
        public string Name { get; set; }

        [RegularExpression("^(([0-9a-zA-Z][0-9a-zA-Z_]*)([,][0-9a-zA-Z][0-9a-zA-Z_]*)*)$", ErrorMessage = "Invalid format. Must be a comma-separated string without special characters!")]
        public string Filters { get; set; }
    }
}