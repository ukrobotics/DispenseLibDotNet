# DispenseLibDotNet

![example workflow](https://github.com/ukrobotics/DispenseLibDotNet/actions/workflows/ci.yml/badge.svg)


## Intro
This library is intended to aid users and integrators to create their own software for UK Robotics' D2 dispenser.

For general user documentation for the D2, see here [D2 quick start docs](https://ukrobotics.tech/docs/d2dispenser/d2-quick-start/)

## Status
This codebase is currently in BETA - we welcome comments, feedback and collaboration!

## Releases
[Downloads/Releases](https://github.com/ukrobotics/DispenseLibDotNet/releases)

## Version numbers
Versions numbers will follow the rules as defined here:
[Semantic Versioning 2.0.0](https://semver.org/)

## Units of measure
This codebase uses strong value types for units where possible, such as Distance, Volume etc.  These take SI metric units such as mm, m for distance and l,ml,ul,nl and pl for volumes. We always like to use strong value types in our codebase as this reduces the chance of unit confusions, especially on API interefaces! 


## Codebase quick start
There are two main C# solutions in this codebase as follows:
- dll:-  UKRobotics.D2.DispenseLib.dll :- consume this dll in your own codebase
- exe:-  DispenseCommandLine.exe :- use this exe from scripts or to integrate into another application that supports running other scripts/programs. For example, one or our clients used this exe to run a D2 from Tecan Evoware...


### UKRobotics.D2.DispenseLib.dll
The class D2Controller is the primary class you will use to control the D2.

Integrating the D2 into your software is very simple and can be done with 3 lines of code as shown below.

~~~
D2Controller controller = new D2Controller();

controller.OpenComms("COM42");// open the given com port

string protocolId = ""; // protocol UUID taken from https://dispense.ukrobotics.app . Create a protocol and then copy the UUID from the protocol metadata.
string plateTypeId = "3c0cdfed-19f9-430f-89e2-29ff7c5f1f20"; // plate GUID from https://labware.ukrobotics.app , the example given here is for a standard ANSI/SLAS 96 well plate
controller.RunDispense(protocolId, plateTypeId); // this will block until the protocol has run...
~~~

However you can also run many other commands if you wish, such as:
- Park and Unpark:-  These commands will park/unpark the arms into the parking hook. This is generally never required as the D2 will automatically park and unpark the arms when you run a dispense protocol.
- Flush:-  This will flush the given valve with the given flush volume.  The D2 will park the arms over the waste before the flush begins.
- GetDispenseState:-  Will return the state of a running dispense. See the Enum called DispenseState
- AwaitIdleValveState:-  Will return when the valve is idle to allow you to chain following commands easily

**Want low level commands???**  If you want to define your own dispense protocol on the fly you can do this. You don't need to run a dispense protocol as defined by the GUI. For example, you can pull a dispense table from your own DB, then have this passed down to the D2 to be run. See the method RunDispense(string protocolId, string plateTypeGuid) for how our API does this and get in touch if you need support.

### DispenseCommandLine.exe
The exe allows you to run the D2 from a script or from a 3rd party application, such as a 3rd party scheduler or for example a liquid handler application, without writing any code yourself.

For example, the following command will run a dispense on a D2 connected to COM43 using the given protocol ID and given plate type ID.
~~~
DispenseCommandLine.exe -ComPort COM42 -ProtocolId d338f60cb0d79fb0d16c00966f373a58 -PlateTypeId 3c0cdfed-19f9-430f-89e2-29ff7c5f1f20
~~~

The protocol ID is created automatically when you edit and save a protocol from the D2's software on [https://dispense.ukrobotics.app](https://dispense.ukrobotics.app)

The plate type ID is taken from our public labware library here: [https://labware.ukrobotics.app/](https://labware.ukrobotics.app/) . If you have an item of labware that is not currently in our library please contact us at info at ukrobotics.net.



## How do we create a release?
~~~
git tag -a v0.1.0 HEAD -m "My tag description..."
git push origin v0.1.0
~~~







