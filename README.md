# Ester Service

Ester is a message transport application for the European Marrow Donor Information System (EMDIS). It was developed by Steiner Ltd., a Czechoslovakian software company that has played a pivotal role in the creation and development of the EMDIS network.  The Ester application is provided free of charge to Bone Marrow Donor Registries (BMDR's) and enables them to send and receive EMDIS messages from their internal systems or from Prometheus (Steiner's own BMDR solution).

Users of Ester will know that it exists as several components called by a timer tray application (unDemon).  These components (EsterECS, EsterPGP, EsterFML, etc.) are called on a scheduled basis defined in the application configuration.  Due to the nature of a desktop tray application a user is required to be logged on to the desktop at all times for it to operate.  Also if the server is restarted for any reason (updates, etc.) then the user is obliged to log back in and restart the application.  This can be complicated further when using the additional Prometheus components as they also require the separate HLACore (DNA types) application to be loaded into memory for the execution of donor searches.

Ester Service aims to provide the same functionality as the unDemon tray timer but wrapped in a Windows service.  This affords the opportunity to call all Ester components as a specified user without the need for a user to be logged on to the desktop and also benefit from an automated restart of the process without intervention.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

You will need Visual Studio (2017+ recommended, any flavour including Community).

### Installing

Clone (or fork) the repository to your local machine, e.g.

```
$ git clone https://github.com/Jammox/EsterService
```

Open the solution in Visual Studio and use NuGet to install dependency packages using either the Manage NuGet Packages utility or the command prompt:

```
nuget restore EsterService.sln
```

## Deployment

Build the project (Release).  The application will be in the EsterService\bin\Release folder. 
Copy the contents of the release folder to your location of choice.

If your Ester unDemon INI file or HLACore executable are not in the standard location you can edit the EsterService.exe.config to match: 

```
<applicationSettings>
  <EsterService.Properties.Settings>
    <setting name="ConfigFilePath" serializeAs="String">
      <value>C:\Steiner\Ester\estParam\estTimer.ini</value>
    </setting>
    <setting name="HLACorePath" serializeAs="String">
      <value>C:\Steiner\HLAcore\shmhla\SysTray\shmhla.exe</value>
    </setting>
  </EsterService.Properties.Settings>
</applicationSettings>
```

The service will pick up the rest of it's configuration from the standard unDemon INI file, setting what runs and when.

Open a command prompt (as admin) and navigate to the application folder.

Install the service:

```
EsterService.exe install
```

For full details about the command line options available, see [here](http://docs.topshelf-project.com/en/latest/overview/commandline.html).

Don't forget to set the user and password in the Services control panel then start the service.

## Built With

* [Microsoft Visual Studio (Community)](https://visualstudio.microsoft.com/vs/community/) - It's free!
* [TopShelf](http://docs.topshelf-project.com/en/latest/) - Windows Service Framework.
* [Ninject](http://www.ninject.org/) - Dependency management.
* [Common.Logging](https://github.com/net-commons/common-logging) - For logging.
* [INIFileParser](https://github.com/rickyah/ini-parser) - The way your Dad used to do it :)

## Versioning

Standard .NET Major.Minor.Build.Revision format.

## Authors

* **James A.Fitches** - *Initial work* - [Jammox](https://github.com/Jammox)

## License

This project is licensed under the GPL3.0 License - see the [LICENSE](https://github.com/Jammox/EsterService/blob/master/LICENSE) file for details.

## Acknowledgments

* [Steiner Ltd.](http://www.hlasoft.com/), for their great work in supporting Bone Marrow Donation worldwide.
* [Ricardo Amores Hernández](https://github.com/rickyah), for INIFileParser. Retro cool!
* Topshelf, for taking the hassle out of Windows Service Applications.
* Ninject, just awesome!

