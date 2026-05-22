namespace Library;

public class Event
{
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsRecurring { get; set; }
}

/// <summary>
/// Event scheduler used to demonstrate DateTime-based assertions.
/// </summary>
public class EventScheduler
{
    private readonly List<Event> _events = [];
    private readonly Func<DateTime> _clock;

    public EventScheduler(Func<DateTime>? clock = null) =>
        _clock = clock ?? (() => DateTime.Now);

    public void Schedule(Event e)
    {
        if (e.EndDate <= e.StartDate)
            throw new ArgumentException("End date must be after start date.");
        if (e.StartDate.Date < _clock().Date)
            throw new ArgumentException("Cannot schedule events in the past.");
        _events.Add(e);
    }

    public IEnumerable<Event> GetEventsOn(DateTime date) =>
        _events.Where(e => e.StartDate.Date <= date.Date && e.EndDate.Date >= date.Date);

    public bool HasConflict(Event newEvent) =>
        _events.Any(e => e.StartDate < newEvent.EndDate && e.EndDate > newEvent.StartDate);

    public Event? GetNextEvent() =>
        _events.Where(e => e.StartDate > _clock())
               .OrderBy(e => e.StartDate)
               .FirstOrDefault();

    public TimeSpan? TimeUntilNextEvent()
    {
        var next = GetNextEvent();
        return next is null ? null : next.StartDate - _clock();
    }
}
