﻿using System;
using System.Text;
using SharpWnfServer.Library;

namespace SharpWnfServer.Handler
{
    internal class Execute
    {
        public static void Run(CommandLineParser options)
        {
            if (options.GetFlag("help"))
            {
                options.GetHelp();
            }
            else
            {
                using (var wnfServer = new WnfCom())
                {
                    if (wnfServer.CreateServer() != 0UL)
                    {
                        string input;
                        wnfServer.PrintInternalName();
                        wnfServer.Write(Encoding.ASCII.GetBytes("Hello, world!"));

                        while (true)
                        {
                            Console.Write("[INPUT]> ");
                            input = Console.ReadLine();
                            wnfServer.Write(Encoding.ASCII.GetBytes(input));
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n[-] Failed to initialize WNF server.\n");
                    }
                }
            }
        }
    }
}
