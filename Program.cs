using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using System.IO;
using System.Threading;

namespace HEICtoJPG
{

	class Program
	{
		public static string imageType;

		static void Main(string[] args)
		{
			int fileLeft = 0;
			string sourcePath = "", targetPath = "";
			string[] filesInFolder;
			bool flag = false;
			foreach (var arg in args)
			{
				Console.WriteLine("Checking directory: " + arg);
				
				if(arg.Contains("/source") )sourcePath = GetPath(arg);
				if (arg.Contains("/target")) targetPath = GetPath(arg);
				if (arg.Contains("/delete")) flag = true;
				if (arg.Contains("/jpg")) imageType = "jpg";
				if (arg.Contains("/png")) imageType = "png";
			}
			if(sourcePath=="") sourcePath= Directory.GetCurrentDirectory();
			if (targetPath == "") targetPath = Directory.GetCurrentDirectory();
			
			filesInFolder = Directory.GetFiles(sourcePath);
			foreach (var file in filesInFolder)
			{
				string ext = Path.GetExtension(file).ToLower();
				if (ext == ".heic") fileLeft++;
			}
			foreach (var file in filesInFolder)
			{
				string ext = Path.GetExtension(file).ToLower();
				if (ext == ".heic")
				{
					Console.WriteLine(file);
					ConvertImage(file, targetPath);
				}
			}
	
			foreach (var file in filesInFolder)
			{
				string ext = Path.GetExtension(file).ToLower();
				if (ext == ".heic" && flag && File.Exists(file))
				{
					File.Delete(file);
					System.IO.File.Delete(file);

				}
			}
		
		}
		static void ConvertImage(string fileToConvert, string exportPath)
		{
			string exportFilePath = Path.Combine(exportPath, Path.GetFileNameWithoutExtension(Path.GetFileName(fileToConvert))) + "."+imageType;
			string outFilename = Path.GetFileName(exportFilePath);
			string ext = Path.GetExtension(outFilename);
			string imageExtension = Path.GetExtension(fileToConvert).ToLower();
			string inFilename = Path.GetFileName(fileToConvert);

			if (File.Exists(exportFilePath))
			{
				Console.WriteLine("Skipped file  " + inFilename + " becuase it has already been converted.");
				return;
			}
			if (imageExtension.Contains("heic") || imageExtension.Contains("png"))
			{
				Console.Write("Processing Item " + inFilename + "...");
				
				using (MagickImage image = new MagickImage(File.ReadAllBytes(fileToConvert)))
				{
					image.Write(exportFilePath);
					Console.WriteLine("Ok");
					
				}
			}
			
		}
		static string GetPath(string str)
		{
			
			string[] ptr = str.Split(new char[] { '=' });
			return ptr[1];

		}
	}
}
