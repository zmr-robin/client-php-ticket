using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Session;
using Tickets;
using API;
using Exceptions;
using Commands;

namespace Client
{
    public class UI
    {

    }

    internal class Program
    {

        static async Task Main(string[] args)  // async Task!
        {
            Session.Auth session = new Session.Auth();
            TUI.TUI tui = new TUI.TUI(session);
            while (true)
            {
                if (session.auth)
                {
                    tui.Main();
                }
                else
                {
                    await tui.login();  // await!!
                }
            }
        }

    }
}
