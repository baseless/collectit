using System.Collections.Generic;
using CollectIt.Web.ChannelService;

namespace CollectIt.Web.Models.ViewModels
{
    public class MemberManageViewModel
    {
        public ICollection<RichChannel> Channels { get; set; } 
    }
}