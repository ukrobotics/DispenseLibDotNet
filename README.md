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

## How do we create a release?
~~~
git tag -a v0.1.0 HEAD -m "My tag description..."
git push origin v0.1.0
~~~


## Codebase quick start
There are two main C# solutions in this codebase as follows:
- dll:-  UKRobotics.D2.DispenseLib.dll :- consume this dll in your own codebase
- exe:-  DispenseCommandLine.exe :- use this exe from scripts or to integrate into another application that supports running other scripts/programs. For example, one or our clients used this exe to run a D2 from Tecan Evoware...


### UKRobotics.D2.DispenseLib.dll
Integrating the D2 into your software is very simple and can be done with 3 lines of code as shown below.

~~~
D2Controller controller = new D2Controller();

controller.OpenComms("COM42");// open the given com port

string protocolId = ""; // protocol UUID taken from https://dispense.ukrobotics.app . Create a protocol and then copy the UUID from the protocol metadata.
string plateTypeId = "3c0cdfed-19f9-430f-89e2-29ff7c5f1f20"; // plate GUID from https://labware.ukrobotics.app , the example given here is for a standard ANSI/SLAS 96 well plate
controller.RunDispense(protocolId, plateTypeId); // this will block until the protocol has run...
~~~

### DispenseCommandLine.exe
The exe allows you to run the D2 from a script or from a 3rd party application, such as a 3rd party scheduler or for example a liquid handler application, without writing any code yourself.

For example, the following command will run a dispense on a D2 connected to COM43 using the given protocol ID and given plate type ID.
~~~
DispenseCommandLine.exe -ComPort COM42 -ProtocolId d338f60cb0d79fb0d16c00966f373a58 -PlateTypeId 3c0cdfed-19f9-430f-89e2-29ff7c5f1f20
~~~

The protocol ID is created automatically when you edit and save a protocol from the D2's software on [https://dispense.ukrobotics.app](https://dispense.ukrobotics.app)

The plate type ID is taken from our public labware library here: [https://labware.ukrobotics.app/](https://labware.ukrobotics.app/) . If you have an item of labware that is not currently in our library please contact us at info at ukrobotics.net.









