using Library;
using Xunit;

namespace Tests;

/// <summary>
/// Example 08 — DateTime Tests
///
/// Strategies for testing time-sensitive logic:
///   - Inject a clock (Func&lt;DateTime&gt;) to control "now" in tests
///   - Assert.True with date comparisons
///   - Assert.InRange for date windows
///   - Testing TimeSpan results
/// </summary>
public class DateTimeTests
{
    /// <summary>
    /// Freeze time to a known instant so every assertion is deterministic.
    /// </summary>
    private static DateTime FakeNow => new(2025, 6, 15, 12, 0, 0);

    private EventScheduler BuildScheduler() => new(clock: () => FakeNow);

    [Fact]
    public void Schedule_FutureEvent_IsStoredSuccessfully()
    {
        var scheduler = BuildScheduler();
        var evt = new Event
        {
            Title     = "Team Meeting",
            StartDate = FakeNow.AddDays(1),
            EndDate   = FakeNow.AddDays(1).AddHours(2)
        };

        scheduler.Schedule(evt); // should not throw

        var next = scheduler.GetNextEvent();
        Assert.NotNull(next);
        Assert.Equal("Team Meeting", next.Title);
    }

    [Fact]
    public void Schedule_PastEvent_ThrowsArgumentException()
    {
        var scheduler = BuildScheduler();
        var evt = new Event
        {
            Title     = "Old Event",
            StartDate = FakeNow.AddDays(-1),
            EndDate   = FakeNow.AddDays(-1).AddHours(1)
        };

        Assert.Throws<ArgumentException>(() => scheduler.Schedule(evt));
    }

    [Fact]
    public void Schedule_EndBeforeStart_ThrowsArgumentException()
    {
        var scheduler = BuildScheduler();
        var evt = new Event
        {
            Title     = "Bad Event",
            StartDate = FakeNow.AddDays(2),
            EndDate   = FakeNow.AddDays(1)   // end is before start
        };

        Assert.Throws<ArgumentException>(() => scheduler.Schedule(evt));
    }

    [Fact]
    public void GetEventsOn_MatchingDate_ReturnsEvent()
    {
        var scheduler = BuildScheduler();
        var day = FakeNow.AddDays(3).Date;
        scheduler.Schedule(new Event
        {
            Title     = "Conference",
            StartDate = day,
            EndDate   = day.AddHours(8)
        });

        var results = scheduler.GetEventsOn(day).ToList();
        Assert.Single(results);
        Assert.Equal("Conference", results[0].Title);
    }

    [Fact]
    public void HasConflict_OverlappingEvent_ReturnsTrue()
    {
        var scheduler = BuildScheduler();
        var start = FakeNow.AddDays(1);
        scheduler.Schedule(new Event { Title = "A", StartDate = start, EndDate = start.AddHours(3) });

        var overlap = new Event { Title = "B", StartDate = start.AddHours(1), EndDate = start.AddHours(4) };
        Assert.True(scheduler.HasConflict(overlap));
    }

    [Fact]
    public void TimeUntilNextEvent_FutureEvent_IsPositive()
    {
        var scheduler = BuildScheduler();
        scheduler.Schedule(new Event
        {
            Title     = "Future",
            StartDate = FakeNow.AddHours(5),
            EndDate   = FakeNow.AddHours(6)
        });

        var timeUntil = scheduler.TimeUntilNextEvent();
        Assert.NotNull(timeUntil);
        Assert.True(timeUntil.Value > TimeSpan.Zero);
    }

    [Fact]
    public void GetNextEvent_NoEvents_ReturnsNull()
    {
        var scheduler = BuildScheduler();
        Assert.Null(scheduler.GetNextEvent());
    }
}
