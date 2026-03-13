using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Session;
using Commands;

namespace TUI
{
    public class TUI
    {
        private Auth session;
        private Commands.Commands commands;
        public TUI(Auth userSession) {
            session = userSession;
            commands = new Commands.Commands(this);
        }

        public async void Main()
        {
            Console.Clear();
            DrawBox("Menu");

            DrawBoxOpening();
            foreach(string item in Commands.Commands.commands["MenuCommand"])
            {
                DrawBoxRow(item);
            }
            DrawBoxBottom();
            Console.Write("\t\t\t\t\t\tCommand:");
            string command = Console.ReadLine();
            await Commands.Commands.rout(command, session);

        }

        public async void Tickets(List<dynamic> result)
        {
            Console.Clear();
            DrawBox("Tickets");
            DrawBoxOpening(80);
            foreach (var ticket in result)
            {
                DrawBoxRow($"[{ticket["ID"]}] {ticket["Subject"].ToString().Trim()} ({ticket["Date"]})", 80);
            }
            DrawBoxBottom(80);
            Console.Write("\t\t\t\t\t\tCommand:");
            string command = Console.ReadLine();
            await Commands.Commands.rout(command, session);
        }

        public async void Ticket(List<dynamic> result, string id)
        {
            string myID = await session.getEmail();
            Console.Clear();
            DrawBox("Ticket #" + id);
            DrawBoxOpening(80);
            foreach (var message in result)
            {
                if (message["EmailID"] == myID)
                {
                    DrawBoxRow($"[{message["EmailID"]}] {message["Content"].ToString().Trim()} ({message["Date"]})", 80, ConsoleColor.Green);
                } else
                {
                    DrawBoxRow($"[{message["EmailID"]}] {message["Content"].ToString().Trim()} ({message["Date"]})", 80);
                }
                
            }
            DrawBoxBottom(80);
            Console.Write("\t\t\t\t\t\tCommand:");
            string command = Console.ReadLine();
            await Commands.Commands.rout(command, session);
        }

        public async Task login()
        {
            Console.Clear();
            DrawBox("Login");
            Console.Write("\t\t\t\tEmail:\t\t");
            string Email = Console.ReadLine();
            Console.Write("\t\t\t\tPassword:\t");
            string Password = Console.ReadLine();
            await session.Login(Email, Password);
        }

        private void WriteColored(string text, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(text);
            Console.ResetColor();
        }

        private void DrawBoxOpening(int width = 40, ConsoleColor color = ConsoleColor.Cyan)
        {
            int padding = (Console.WindowWidth - width) / 2;
            string pad = new string(' ', padding);
            string top = "╔" + new string('═', width - 2) + "╗";
            WriteColored(pad + top + "\n", color);
        }
        private void DrawBoxRow(string content, int width = 40, ConsoleColor color = ConsoleColor.Cyan)
        {
            int padding = (Console.WindowWidth - width) / 2;
            string pad = new string(' ', padding);
            string mid = "║ " + content.PadRight(width - 3) + "║";
            WriteColored(pad + mid + "\n", color);
        }
        private void DrawBoxBottom(int width = 40, ConsoleColor color = ConsoleColor.Cyan)
        {
            int padding = (Console.WindowWidth - width) / 2;
            string pad = new string(' ', padding);
            string bottom = "╚" + new string('═', width - 2) + "╝";
            WriteColored(pad + bottom + "\n", color);
        }
        private void DrawBox(string title, int width = 40, ConsoleColor color = ConsoleColor.Cyan)
        {
            int padding = (Console.WindowWidth - width) / 2;
            string pad = new string(' ', padding);
            string top = "╔" + new string('═', width - 2) + "╗";
            string bottom = "╚" + new string('═', width - 2) + "╝";
            string mid = "║" + title.PadLeft((width - 2 + title.Length) / 2)
                                      .PadRight(width - 2) + "║";

            WriteColored(pad + top + "\n", color);
            WriteColored(pad + mid + "\n", color);
            WriteColored(pad + bottom + "\n", color);
        }

        static public void Spinner(string message, CancellationToken token)
        {
            string[] frames = { ".", "...", "....", "...", "..", "."};
            int i = 0;
            while (!token.IsCancellationRequested)
            {
                Console.Write($"\r  {frames[i % frames.Length]} {message}");
                Thread.Sleep(80);
                i++;
            }
        }
    }
}
