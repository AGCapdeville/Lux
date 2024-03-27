
public class Location
{
    public int event_id { get; set; }
    public string event_name { get; set; }
    public Location previous_location { get; set; }
    public Location next_location { get; set; }

    // Constructor to initialize event_id and event_name
    public Location(int eventId, string eventName)
    {
        event_id = eventId;
        event_name = eventName;
    }
}
