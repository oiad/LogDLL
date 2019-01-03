using RGiesecke.DllExport;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LogDLL
{
    public class DllEntry
    {
        [DllExport("_RVExtension@12", CallingConvention = CallingConvention.Winapi)]
        public static void RVExtension(StringBuilder output, int outputSize, [MarshalAs(UnmanagedType.LPStr)] string function)
        {
            outputSize--;

            string originalInput = function;
            string defaultError = "LogDLL Error: ";
            string origPath;
            string path;

            //Find the Tildas
            int firstTilda = originalInput.IndexOf('~');
            int secondTilda = originalInput.LastIndexOf('~');
            //Check to see if the format is correct
            if (firstTilda == -1 || secondTilda == -1 || firstTilda == secondTilda)
            {
                output.Append(defaultError + "Formatting Error (FOLDER/PATH~FILENAME~LOGENTRY)");
                return;
            }
            //Length of filename used for substring
            int nameLength = secondTilda - firstTilda - 1;
            //Extract filename
            string fileName = originalInput.Substring(firstTilda + 1, nameLength);
            
            //Check if file sdname is empty
            if (fileName.Length == 0)
            {
                output.Append(defaultError + "File name can not be empty.");
                return;
            }

            //Construct the path
            if (originalInput.IndexOf(':') < firstTilda && originalInput.IndexOf(':') != -1)
            {
                //Using a Custom Path
                origPath = originalInput.Substring(0, firstTilda);
                //Check if path is empty
                if (origPath.Length == 0)
                {
                    output.Append(defaultError + "Path can not be empty.");
                    return;
                }
                string endPath = "\\" + fileName + "\\";
                path = origPath + endPath;
            }
            else
            {
                //Use Path of server as base
                string folderName = originalInput.Substring(0, firstTilda);
                //Check if foldername is empty
                if (folderName.Length == 0)
                {
                    output.Append(defaultError + "Folder name can not be empty.");
                    return;
                }
                origPath = AppDomain.CurrentDomain.BaseDirectory;
                string endPath = folderName + "\\" + fileName + "\\";
                path = origPath + endPath;

            }

            //Create Directory if not exists
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                if (e is DirectoryNotFoundException)
                {
                    output.Append(defaultError + string.Format("Invalid Path entered ({0})", origPath));
                }
                else
                {
                    output.Append(defaultError + string.Format("An unknown error has occured."));
                }
                return;
            }
            

            //Construct filename
            string curDateString = DateTime.Now.ToString("dd-MM-yyyy");
            string filenameBuilt = fileName + "_" + curDateString + ".txt";

            //Get log
            string message = originalInput.Substring(secondTilda + 1);
            //Check if message is empty
            if (message.Length == 0)
            {
                output.Append(defaultError + "Log entry can not be empty.");
                return;
            }

            using (StreamWriter sw = File.AppendText(path + filenameBuilt))
            {
                string curTimeString = DateTime.Now.ToString("HH:mm:ss");
                sw.WriteLine(curDateString + " " + curTimeString + " | " + message);
            }

            //Done here
            output.Append(string.Format("LogDLL Success: New Log Entry succesfully written to {0}", path + filenameBuilt));
        }
    }
}