# hot-dataservice

[![Build and publish](https://github.com/xivk/hot-dataservice/actions/workflows/main.yml/badge.svg)](https://github.com/xivk/hot-dataservice/actions/workflows/main.yml)  
[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/xivk/hot-dataservice/blob/main/LICENSE.md)  

## About

This is a simple webservice that serves extra data for usage with the [HOT tasking manager](https://github.com/hotosm/osm-tasking-manager2). Basically it just serves data from a configured folder and only returns filtered data:

    http://www.domain.com/data/{x}/{y}/{z}/{file}

An example:

    http://hotosm.osmsharp.com/data/563/524/10/WRI_CAR_routes_2009.osm

This returns an OSM-XML file filtered to a bounding box represented by the tile with x = 536, y = 524 and zoom = 10. You can include this extra data to load into JOSM by adding some HTML to the _Per Task Instructions_:

> Put here anything that can be useful to users while taking a task. {x}, {y} and {z} will be replaced by the corresponding parameters for each task.
> For example: « This task involves loading extra data. Click [here](http://localhost:8111/import?new_layer=true&url=http://www.domain.com/data/{x}/{y}/{z}/routes_2009.osm) to load the data into JOSM ». 

## Deployment

There is a [Docker container](https://hub.docker.com/r/xivk/hot-dataservice) available. An example docker compose:

````
version: "3.1"
services:  
  hot-dataservice:
    image: xivk/hot-dataservice:latest
    volumes:
      - /var/services/hot-dataservice/data:/var/app/data
      - /var/services/hot-dataservice/logs:/var/app/logs
    ports:
      - "5000:5000"
````
