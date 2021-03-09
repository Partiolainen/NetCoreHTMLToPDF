using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using NetCoreHTMLToPDF.Enums;

namespace NetCoreHTMLToPDF
{
    /// <summary>
    /// Html Converter. Converts HTML string and URLs to pdf bytes
    /// </summary>
    public class HtmlConverter
    {
	    private readonly string directory;
        private readonly string toolFilepath;

        public HtmlConverter(string LibPath = null)
        {
	        var toolFilename = string.IsNullOrEmpty(LibPath) ? (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "/usr/local/bin/wkhtmltopdf") : LibPath;

			directory = AppContext.BaseDirectory;
			toolFilepath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
				? Path.Combine(directory, toolFilename)
				: toolFilename;

            if (File.Exists(toolFilepath)) return;
            var assembly = typeof(HtmlConverter).GetTypeInfo().Assembly;
            var type = typeof(HtmlConverter);
            var ns = type.Namespace;

            using var resourceStream = assembly.GetManifestResourceStream($"{ns}.{toolFilename}");
            using var fileStream = File.OpenWrite(toolFilepath);
            resourceStream?.CopyTo(fileStream);
        }

        /// <summary>
        /// Converts HTML string to image
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <param name="pageSize">Set paper size to: A4, Letter, etc. (default A4)</param>
        /// <param name="pageOrientation">Set orientation to Landscape or Portrait (default Portrait)</param>
        /// <param name="customFlags">Custom flags for wkhtmltoimage lib</param>
        /// <param name="timeOut">Execution timeout</param>
        /// <returns></returns>
        public byte[] FromHtmlString(string html, PageSize pageSize = PageSize.A4, PageOrientation pageOrientation = PageOrientation.Portrait, string customFlags = null, int? timeOut = null)
        {
            var filename = Path.Combine(directory, $"{Guid.NewGuid()}.html");
            File.WriteAllText(filename, html);
            var bytes = FromUrl(filename, pageSize, pageOrientation, customFlags, timeOut);
            File.Delete(filename);
            return bytes;
        }

        /// <summary>
        /// Converts HTML page to image
        /// </summary>
        /// <param name="url">Valid http(s):// URL</param>
        /// <param name="pageSize">Set paper size to: A4, Letter, etc. (default A4)</param>
        /// <param name="pageOrientation">Set orientation to Landscape or Portrait (default Portrait)</param>
        /// <param name="customFlags">Custom flags for wkhtmltopdf lib</param>
        /// <param name="timeOut">Execution timeout</param>
        /// <returns></returns>
        public byte[] FromUrl(string url, PageSize pageSize = PageSize.A4, PageOrientation pageOrientation = PageOrientation.Portrait, string customFlags = null, int? timeOut = null)
        {
            var filename = Path.Combine(directory, $"{Guid.NewGuid()}.pdf");
            var arguments = $"--page-size {pageSize} --orientation {pageOrientation} {customFlags} \"{url}\" \"{filename}\"";

            try
            {
                var processStartInfo = new ProcessStartInfo(toolFilepath, arguments)
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = directory,
                    //RedirectStandardError = true
                };
			
                var process = Process.Start(processStartInfo);

                if (process != null)
                {
                    //process.ErrorDataReceived += Process_ErrorDataReceived;
                    if (timeOut == null) process.WaitForExit();
                    else process.WaitForExit(timeOut.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Process path line: {toolFilepath} {arguments}");
                Console.WriteLine(e);
                throw;
            }

            if (!File.Exists(filename)) throw new Exception("Something went wrong. Please check input parameters");
            var bytes = File.ReadAllBytes(filename);
            File.Delete(filename);
            return bytes;

        }

        //private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    throw new Exception(e.Data);
        //}
    }
}
