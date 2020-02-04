namespace EsterService.Configuration
{
	public interface ISettings
	{
		string ConfigFilePath { get; }
		string HlaCorePath { get; }
		void Save();
	}
}
