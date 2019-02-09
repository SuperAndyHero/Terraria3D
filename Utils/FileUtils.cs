using System.IO;

namespace Terraria3D
{
	public static class FileUtils
	{
		public static string GetTextFile(string fileName)
		{
			var bytes = Terraria3D.Instance.File.GetFile(fileName);
			var result = string.Empty;
			using (var memoryStream = new MemoryStream(bytes))
			{
				using (var reader = new StreamReader(memoryStream))
				{
					result = reader.ReadToEnd();
				}
			}
			return result;
		}
	}
}
