using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Ports;
using WhetStone.Ports.AutoCommands;

namespace WhetStone
{
    public static class acceptsAutoCommands
    {
        public static bool AcceptsAutoCommand(this IConnection @this, IConnectionAutoCommand c)
        {
            if (c != null && @this.enabledAutoCommands != null && @this.enabledAutoCommands.Contains(c.GetType()))
                return true;
            var glued = c as ConnectionGluedAutoCommand;
            return glued != null && glued.commands.All(@this.AcceptsAutoCommand);
        }
    }
}
