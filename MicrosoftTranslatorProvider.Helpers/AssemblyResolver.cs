using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MicrosoftTranslatorProvider.Helpers;

public static class AssemblyResolver
{
	public static List<string> AssemblyFolders = new List<string>();

	public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		AssemblyFolders = AssemblyFolders.Distinct().ToList();
		string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)?.Substring(6);
		DirectoryInfo directoryInfo = new DirectoryInfo(text ?? "");
		if (!AssemblyFolders.Contains(directoryInfo.FullName))
		{
			AssemblyFolders.Add(directoryInfo.FullName);
		}
		AssemblyName requestedAssembly = new AssemblyName(args.Name);
		List<FileInfo> list = new List<FileInfo>();
		foreach (string assemblyFolder in AssemblyFolders)
		{
			list.AddRange(from V in new DirectoryInfo(assemblyFolder).GetFiles("*.*", SearchOption.AllDirectories)
				where V.Name.Contains(requestedAssembly.Name)
				select V);
		}
		List<Assembly> list2 = new List<Assembly>();
		foreach (FileInfo item in list)
		{
			if (!item.FullName.ToLower().EndsWith(".dll") && !item.FullName.ToLower().EndsWith(".exe"))
			{
				continue;
			}
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(item.FullName);
			if (assemblyName.Version > requestedAssembly.Version)
			{
				requestedAssembly.Version = assemblyName.Version;
			}
			else if (assemblyName.Version < requestedAssembly.Version)
			{
				continue;
			}
			list2.Add(Assembly.LoadFrom(item.FullName));
			break;
		}
		foreach (Assembly item2 in list2)
		{
			if (item2.FullName.Split(new char[1] { ',' }).ElementAt(0) == args.Name.Split(new char[1] { ',' }).ElementAt(0))
			{
				return item2;
			}
		}
		return null;
	}
}
