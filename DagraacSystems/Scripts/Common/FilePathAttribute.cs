using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems
{
	class FilePathAttribute : Attribute
	{
		public List<string> m_FilePaths { private set; get; }

		public FilePathAttribute(string filePath)
		{
			m_FilePaths = new List<string>();
			m_FilePaths.Add(filePath);
		}

		public FilePathAttribute(string[] filePaths)
		{
			m_FilePaths = new List<string>();
			m_FilePaths.AddRange(filePaths);
		}
	}
}