# hot-dataservice

## About

This is a simple webservice that service extra data for usage with the [HOT tasking manager](https://github.com/hotosm/osm-tasking-manager2). Basically it just serves data from a configured folder and only returns filtered data:

    http://www.domain.com/data/{x}/{y}/{z}/{file}

An example:

    http://hotosm.osmsharp.com/data/563/524/10/WRI_CAR_routes_2009.osm

This returns an OSM-XML file filtered to a bounding box represented by the tile with x = 536, y = 524 and zoom = 10. You can include this extra data to load into JOSM by adding some HTML to the _Per Task Instructions_:

> Put here anything that can be useful to users while taking a task. {x}, {y} and {z} will be replaced by the corresponding parameters for each task.
> For example: « This task involves loading extra data. Click [here](http://localhost:8111/import?new_layer=true&url=http://www.domain.com/data/{x}/{y}/{z}/routes_2009.osm) to load the data into JOSM ». 

## Installation

First install [donet core](https://www.microsoft.com/net/core), the open-sourced version of the old .NET Framework. You need the latest 1.1 version and then run the build/run scripts. The service will listen to port 5000 by default.

#### Ubuntu

Specific instructions on ubuntu (tested on )

