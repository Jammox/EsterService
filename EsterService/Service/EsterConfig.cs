using Common.Logging;
using EsterService.Configuration;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EsterService.Service
{
	public class EsterConfig : IServiceConfig
	{
		private readonly ISettings _settings;

		public ILog Log { get; private set; }

		public List<ConfigurationItem> Items { get; protected set; }

		public EsterConfig(ISettings settings, ILog logger)
		{
			_settings = settings;
			Log = logger;
			Items = new List<ConfigurationItem>();
		}

		public void Load()
		{
			try
			{
				Log.Info($"ConfigFilePath: {_settings.ConfigFilePath}");
				Log.Info($"HlaCorePath: {_settings.HlaCorePath}");

				if (!File.Exists(_settings.ConfigFilePath))
					throw new FileNotFoundException();

				// initiate parser and handle non standard /E in file
				var parser = new IniDataParser(
				new IniParserConfiguration() { SkipInvalidLines = true });

				IniData data = parser.Parse(File.ReadAllText(_settings.ConfigFilePath));

				Items = new List<ConfigurationItem>();

				// load schedules and items from config file
				foreach (var section in data.Sections)
				{
					Items.Add(new ConfigurationItem
					{
						Name = section.SectionName,
						File = data[section.SectionName]["ProgName"],
						Path = data[section.SectionName]["ProgDir"],
						Options = data[section.SectionName]["ProgPar"],
						Delay = ParseTime(data[section.SectionName]["Offset"]),
						Interval = ParseTime(data[section.SectionName]["Interval"]),
						Active = data[section.SectionName]["Started"] == "1"
					});
				}
			}
			catch (Exception ex)
			{
				Log.Error("Unable to load config file!", ex);
			}
		}

		private static TimeSpan ParseTime(string time)
		{
			if (time == null)
				throw new ArgumentException(nameof(time));

			// cleanup time and pad if necessary
			time = Regex.Replace(time, "[^0-9:]", "");
			time = time.Length < 6 ? (':' + time).PadLeft(8, '0') : time;

			if (Regex.IsMatch(time, @"^([0-9]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])$"))
			{
				int[] st = Array.ConvertAll(Regex.Split(time, @"\D+"), int.Parse);
				return new TimeSpan(
					hours: (st.Length > 0 ? st[0] : 0),
					minutes: (st.Length > 1 ? st[1] : 0),
					seconds: (st.Length > 2 ? st[2] : 0));
			}
			else
			{
				return new TimeSpan(0);
			}
		}
	}
}
