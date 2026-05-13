using System.Collections.Generic;

namespace MyWebApplication.BusinessLogic.Observer;

/// <summary>
/// Subject in the Observer pattern. Notifies all attached observers when
/// a lesson is added to a course. Observers are injected via constructor.
/// </summary>
public class CourseNotifier : ISubject
{
    private readonly List<IObserver> _observers = new();

    public CourseNotifier(NotificationObserver notificationObserver)
    {
        Attach(notificationObserver);
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify(string message)
    {
        foreach (var obs in _observers) obs.Update(message);
    }

    public void CourseAdded(string courseTitle) =>
        Notify($"Curs nou disponibil: \"{courseTitle}\"");

    public void LessonAdded(string courseTitle, string lessonTitle) =>
        Notify($"Lecție nouă în cursul \"{courseTitle}\": {lessonTitle}");
}
