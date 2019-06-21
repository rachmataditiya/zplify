using System;

namespace zplify
{
    public static class ArgumentParser
    {
        public static Arguments Parse(string[] args, out string errorMsg)
        {
            var a = new Arguments();
            errorMsg = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i][0] == '-')
                {
                    var startIndex = 1;
                    if (args[i][1] == '-')
                    {
                        startIndex = 2;
                    }
                    var msg = args[i].Substring(startIndex);
                    if (msg == "h" || msg == "help")
                    {
                        errorMsg = a.SetArgument(msg, null);
                    }
                    else
                    {
                        errorMsg = a.SetArgument(msg, args[i + 1]);
                    }
                    if (errorMsg != null)
                    {
                        return a;
                    }
                    i++;
                }
                else if (i == 0)
                {
                    if (System.IO.File.Exists(args[i]))
                    {
                        a.InPath = args[i];
                    }
                    else
                    {
                        errorMsg = "First argument must be the input image.";
                        return a;
                    }
                }
                else
                {
                    errorMsg = "Invalid formatted argument";
                    return a;
                }
            }
            return a;
        }
        public static string SetArgument(this Arguments arguments, string input, string value)
        {
            switch (input)
            {
                case "l":
                case "length":
                    if (int.TryParse(value, out var length))
                    {
                        arguments.Length = length;
                    }
                    else
                    {
                        return "Length input is invalid";
                    }
                    break;
                case "w":
                case "width":
                    if (int.TryParse(value, out var width))
                    {
                        arguments.Width = width;
                    }
                    else
                    {
                        return "Width input is invalid";
                    }
                    break;
                case "o":
                case "output":
                    try
                    {
                        arguments.OutPath = System.IO.Path.GetFullPath(value);
                    }
                    catch(Exception)
                    {
                        return "Output path is in an invalid format.";
                    }
                    break;
                case "h":
                case "help":
                    return "The full list of available options are as follows:\n"
                         + "\n"
                         + "-l --length    Set the length of the label in pixels. 1200px is default.\n"
                         + "-w --width     Set the width of the label in pixels. 800px is default.\n"
                         + "-o --output    Set the output path. If omitted, the label will output to the terminal\n"
                         + "-h --help      Display this message\n";
                default:
                    return $"Argument '{value}' not valid. use --help for list of valid arguments";
            }
            return null;
        }
    }
}