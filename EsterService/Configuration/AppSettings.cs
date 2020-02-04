namespace EsterService.Configuration
{
	/// <summary>
	/// Application settings wrapper.
	/// </summary>
	public class AppSettings : ISettings
	{
		public string ConfigFilePath
		{
			get { return Properties.Settings.Default.ConfigFilePath; }
		}

		public string HlaCorePath
		{
			get { return Properties.Settings.Default.HLACorePath; }
		}

		public void Save()
		{
			Properties.Settings.Default.Save();
		}
	}
}
