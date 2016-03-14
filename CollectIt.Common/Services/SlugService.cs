using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollectIt.Common.Services
{

    public class SlugService
    {
        public static string ToSlug(string name)
        {
            name = Regex.Replace(name, @"[^a-zA-Z0-9\s-]", "");
            name = Regex.Replace(name, @"\s+", " ").Trim();
            name = name.Substring(0, name.Length <= 60 ? name.Length : 60).Trim();
            name = Regex.Replace(name, @"\s", "-"); 
            return name.ToLower();
        }
    }
}
