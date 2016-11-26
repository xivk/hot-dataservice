# hot-dataservice

## About

This is a simple webservice that serves extra data for usage with the [HOT tasking manager](https://github.com/hotosm/osm-tasking-manager2). Basically it just serves data from a configured folder and only returns filtered data:

    http://www.domain.com/data/{x}/{y}/{z}/{file}

An example:

    http://hotosm.osmsharp.com/data/563/524/10/WRI_CAR_routes_2009.osm

This returns an OSM-XML file filtered to a bounding box represented by the tile with x = 536, y = 524 and zoom = 10. You can include this extra data to load into JOSM by adding some HTML to the _Per Task Instructions_:

> Put here anything that can be useful to users while taking a task. {x}, {y} and {z} will be replaced by the corresponding parameters for each task.
> For example: « This task involves loading extra data. Click [here](http://localhost:8111/import?new_layer=true&url=http://www.domain.com/data/{x}/{y}/{z}/routes_2009.osm) to load the data into JOSM ». 

## Installation

First install [.NET Core](https://www.microsoft.com/net/core), the open-sourced version of the old .NET Framework. You need the latest 1.1 version and then run the build/run scripts. The service will listen to port 5000 by default.

#### Ubuntu

Specific instructions on Ubuntu (tested on 16.04), for other platforms check [.NET Core installation guide](https://www.microsoft.com/net/core).

Add the dotnet apt-get feed

    sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
    sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
    sudo apt-get update

Install .NET Core:
	
    sudo apt-get install dotnet-dev-1.0.0-preview2.1-003177

Clone this repo and run build or run scripts:

    ./run.sh


