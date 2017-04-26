using System;

namespace KNEditor
{
    class ELog
    {

        public static void LogError(string pError)
        {
            
            Console.WriteLine("Error: " + pError);
        }

        public static void LogLine(string pError)
        {

            Console.WriteLine(pError);
        }
    }
}
