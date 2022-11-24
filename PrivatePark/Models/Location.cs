

using Newtonsoft.Json;

namespace mnacr22.Models;


public class Location
{
    public Location () {}

    public Location(double lat, double lng)
    {
        Lat = lat;
        Lng = lng; 
    }
    
    
    public int Id { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    
    public Address Address { get; set; }
}

public class Geometry
{
    public Location location { get; set;} 
}

public class Results
{
    public Geometry Geometry { get; set;}
}


public class Rootobject
{
    public Results[] results {get; set; }
}


