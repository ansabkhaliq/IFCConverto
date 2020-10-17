﻿using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IFCConverto.Enums.TextfileProcessingEnum;

namespace IFCConverto.Services
{
    public class TextfileProcessingService
    {
        public delegate void TextfileProcessingUpdateForUIEventHandler(string message);
        public event TextfileProcessingUpdateForUIEventHandler TotalFiles;
        public event TextfileProcessingUpdateForUIEventHandler RemainingFiles;
        public event TextfileProcessingUpdateForUIEventHandler ProcessingException;

        /// <summary>
        /// Method to process the text file and generate JSON files
        /// </summary>
        /// <param name="sourceLocation">Source location path</param>
        /// <param name="destinationLocation">Destination location path</param>
        /// <returns>Textfile processing Status to update the UI</returns>
        public async Task<TextfileProcessingStatus> ProcessFiles(string sourceLocation, string destinationLocation)
        {
            try
            {
                return await Task.Run(() =>
                {
                    // get all file names from the source location
                    var allFilenames = Directory.EnumerateFiles(sourceLocation).Select(p => Path.GetFileName(p));

                    // Get all filenames that have a .txt extension
                    var files = allFilenames.Where(fn => Path.GetExtension(fn) == ".txt");

                    // If there are no files, then return to notify the user
                    if (files == null || files.Count() == 0)
                    {
                        return TextfileProcessingStatus.NoFiles;
                    }

                    // Get total number of files, (need it for the progress bar)
                    var totalFiles = files.Count();

                    // Send the total count of the files to the viewmodel for update on UI
                    TotalFiles?.Invoke(totalFiles.ToString());

                    // Process each file and convert it 
                    foreach (var file in files)
                    {
                        // Get the path for the source file
                        var sourceFile = Path.Combine(sourceLocation, file);                        

                        // Read the first 2 lines of the text file.
                        // We are only read the 2 lines for now due to current requirements, but for future just increase the number in Take(2) to how many lines you want to read
                        var lines = File.ReadLines(sourceFile).Take(2).ToArray();

                        // Final Heading Tokens after being cleaned and formatted would be stored in this list
                        var finalHeadings = ProcessHeaderString(lines[0]);

                        // Will contain the values from the CSV file 
                        var content = ProcessData(lines[1]);                                                

                        // Time to process the strings and convert to Json
                        // However, if this condition is not satisfied, that means there is something wrong with the file. We need to alert the user.
                        if (content != null && finalHeadings != null && content.Count() > 0 && finalHeadings.Count > 0 && finalHeadings.Count == content.Count)
                        {
                            
                        }                        
                        else
                        {

                        }
                        
                        totalFiles--;

                        // Send message to UI to update progress bar
                        RemainingFiles?.Invoke((totalFiles).ToString());
                    }

                    // Return success message
                    return TextfileProcessingStatus.Done;
                });
            }
            catch (Exception ex)
            {
                ProcessingException?.Invoke(ex.Message);
                return TextfileProcessingStatus.Error;
            }
        }

        private List<string> ProcessData(string contentString)
        {
            List<string> content;

            // Using the built in CSV parser to parse the content as we don't need to clean this like the headers
            using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(contentString)))
            {
                using (Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(ms))
                {
                    tfp.Delimiters = new string[] { "," };
                    tfp.HasFieldsEnclosedInQuotes = true;
                    content = tfp.ReadFields().ToList<string>();
                }
            }

            return content;
        }

        /// <summary>
        /// Private method, that will take in the header line and then format and clean it.
        /// This is a custom parser written for the header
        /// </summary>
        /// <param name="headerString">Headings line from the .csv file</param>
        /// <returns></returns>
        private List<string> ProcessHeaderString(string headerString)
        {
            // Lets clean the heading line as it has some noise in it that we don't need
            var cleanHeading = headerString.Replace("##OTHER##", string.Empty).Replace("##", " (").Split(',');
            var finalHeadingList = new List<string>();

            // For each string in this array we need to format them and clean them (look at the text file to understand)
            foreach (var item in cleanHeading)
            {
                var headingSubstrings = item.Split('(');
                var formattedHeading = string.Empty;

                // If the split length is just 1, then it means that there was 
                if (headingSubstrings.Length > 1)
                {
                    // Get the length of the substring array
                    var lastIndex = headingSubstrings.Length;

                    // if the last element is null or empty that means the header is something like Test##Millimeter## (only 1 element for measurement)
                    // else, there are more than 1 element for measuring unit, so go to else part
                    // This part basically removes the '_' and the uncessary '(' introduced before for processing.
                    if (string.IsNullOrEmpty(headingSubstrings[lastIndex - 1]))
                    {
                        headingSubstrings[1] = '(' + headingSubstrings[1].Trim().Replace('_', ' ') + ')';
                    }
                    else
                    {
                        headingSubstrings[1] = '(' + headingSubstrings[1].Replace('_', ' ');
                        headingSubstrings[lastIndex - 1] = headingSubstrings[lastIndex - 1].Replace('(', ' ').Replace('_', ' ') + ')';
                    }
                    
                    // Change the Upper Case words to Title Case
                    for (var i = 1; i < headingSubstrings.Length; i++)
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                        headingSubstrings[i] = textInfo.ToTitleCase(headingSubstrings[i].ToLower());
                    }

                    // Combine the now cleaned and formatted substring in to 1 complete string
                    foreach (var subString in headingSubstrings)
                    {
                        formattedHeading += subString;
                    }
                }

                // Add the final substrings to the list. It will have formatted and cleaned headings that we can use
                if (string.IsNullOrEmpty(formattedHeading))
                {
                    finalHeadingList.Add(item);
                }
                else
                {
                    finalHeadingList.Add(formattedHeading);
                }                
            }

            return finalHeadingList;
        }
    }
}
