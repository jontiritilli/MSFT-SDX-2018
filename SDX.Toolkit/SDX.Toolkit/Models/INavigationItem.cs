using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDX.Toolkit.Models
{
    public interface INavigationItem
    {
        string Name { get; set; }
        string Text { get; set; }
        int Order { get; set; }
    }
}
