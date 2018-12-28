using RGiesecke.DllExport;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LogDLL
{
    public class DllEntry
    {
        [DllExport("_RVExtension@12", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        public static void RVExtension(StringBuilder output, int outputSize, [MarshalAs(UnmanagedType.LPStr)] string function)
        {
            outputSize--;

            string originalInput = function;
            int firstTilda = originalInput.IndexOf('~');
            int secondTilda = originalInput.LastIndexOf('~');
            int nameLength = secondTilda - firstTilda - 1;
            string fileName = originalInput.Substring(firstTilda + 1, nameLength);

            string path;

            //Construct the path
            if (originalInput.IndexOf(':') < firstTilda && firstTilda != -1 && originalInput.IndexOf(':') != -1)
            {
                //Using a Custom Path
                path = originalInput.Substring(0, originalInput.IndexOf('~'));
                string endPath = "\\" + fileName + "\\";
                path += endPath;
            }
            else
            {
                //Use Path of DLL as base
                string folderName = originalInput.Substring(0, originalInput.IndexOf('~'));
                path = AppDomain.CurrentDomain.BaseDirectory;
                string endPath = "\\" + folderName + "\\" + fileName + "\\";
                path += endPath;

            }

            //Create Directory if not exists
            Directory.CreateDirectory(path);

            //Construct filename
            string curDateString = DateTime.Now.ToString("dd-MM-yyyy");
            string curTimeString = DateTime.Now.ToString("HH:mm:ss");
            string filenameBuilt = fileName + "_" + curDateString + ".log";

            //Get log
            string message = originalInput.Substring(secondTilda + 1);

            using (StreamWriter sw = File.AppendText(path + filenameBuilt))
            {
                sw.WriteLine(curTimeString + " | " + message);
            }
        }
    }
}