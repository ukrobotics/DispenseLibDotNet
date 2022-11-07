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

Integrating the D2 is very simple and can be done with 3 lines of code as shown below.

~~~
D2Controller controller = new D2Controller();

controller.OpenComms("COM42");// open the given com port

string protocolId = ""; // protocol UUID taken from https://dispense.ukrobotics.app . Create a protocol and then copy the UUID from the protocol metadata.
string plateTypeId = "3c0cdfed-19f9-430f-89e2-29ff7c5f1f20"; // plate GUID from https://labware.ukrobotics.app , the example given here is for a standard ANSI/SLAS 96 well plate
controller.RunDispense(protocolId, plateTypeId); // this will block until the protocol has run...
~~~





