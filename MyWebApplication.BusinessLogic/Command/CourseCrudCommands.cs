using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Command;

// ---------- COURSE ----------

public class CreateCourseCommand : ICommand
{
    private readonly ICourseStore _store;
    private readonly CourseDto _input;
    public CourseDto? Created { get; private set; }

    public CreateCourseCommand(ICourseStore store, CourseDto input)
    {
        _store = store; _input = input;
    }
    public void Execute() => Created = _store.Create(_input);
    public void Undo()
    {
        if (Created is not null) _store.Delete(Created.Id);
    }
}

public class UpdateCourseCommand : ICommand
{
    private readonly ICourseStore _store;
    private readonly CourseDto _input;
    private CourseDto? _before;
    public bool Success { get; private set; }

    public UpdateCourseCommand(ICourseStore store, CourseDto input)
    {
        _store = store; _input = input;
    }
    public void Execute()
    {
        _before = _store.FindById(_input.Id);
        Success = _store.Update(_input);
    }
    public void Undo()
    {
        if (_before is not null) _store.Update(_before);
    }
}

public class DeleteCourseCommand : ICommand
{
    private readonly ICourseStore _store;
    private readonly int _id;
    private CourseDto? _snapshot;
    public bool Success { get; private set; }

    public DeleteCourseCommand(ICourseStore store, int id)
    {
        _store = store; _id = id;
    }
    public void Execute()
    {
        _snapshot = _store.FindById(_id);
        Success = _snapshot is not null && _store.Delete(_id);
    }
    public void Undo()
    {
        if (_snapshot is not null) _store.Create(_snapshot);
    }
}

// ---------- LESSON ----------

public class CreateLessonCmd : ICommand
{
    private readonly ILessonStore _store;
    private readonly LessonDto _input;
    public LessonDto? Created { get; private set; }

    public CreateLessonCmd(ILessonStore store, LessonDto input)
    {
        _store = store; _input = input;
    }
    public void Execute() => Created = _store.Create(_input);
    public void Undo()
    {
        if (Created is not null) _store.Delete(Created.Id);
    }
}

public class UpdateLessonCmd : ICommand
{
    private readonly ILessonStore _store;
    private readonly LessonDto _input;
    private LessonDto? _before;
    public bool Success { get; private set; }

    public UpdateLessonCmd(ILessonStore store, LessonDto input)
    {
        _store = store; _input = input;
    }
    public void Execute()
    {
        _before = _store.FindById(_input.Id);
        Success = _store.Update(_input);
    }
    public void Undo()
    {
        if (_before is not null) _store.Update(_before);
    }
}

public class DeleteLessonCmd : ICommand
{
    private readonly ILessonStore _store;
    private readonly int _id;
    private LessonDto? _snapshot;
    public bool Success { get; private set; }

    public DeleteLessonCmd(ILessonStore store, int id)
    {
        _store = store; _id = id;
    }
    public void Execute()
    {
        _snapshot = _store.FindById(_id);
        Success = _snapshot is not null && _store.Delete(_id);
    }
    public void Undo()
    {
        if (_snapshot is not null) _store.Create(_snapshot);
    }
}

// ---------- PURCHASE ----------

public class CreatePurchaseCmd : ICommand
{
    private readonly IPurchaseStore _store;
    private readonly PurchaseDto _input;
    public PurchaseDto? Created { get; private set; }

    public CreatePurchaseCmd(IPurchaseStore store, PurchaseDto input)
    {
        _store = store; _input = input;
    }
    public void Execute() => Created = _store.Create(_input);
    public void Undo()
    {
        if (Created is not null) _store.Delete(Created.Id);
    }
}

public class UpdatePurchaseCmd : ICommand
{
    private readonly IPurchaseStore _store;
    private readonly PurchaseDto _input;
    private PurchaseDto? _before;
    public bool Success { get; private set; }

    public UpdatePurchaseCmd(IPurchaseStore store, PurchaseDto input)
    {
        _store = store; _input = input;
    }
    public void Execute()
    {
        _before = _store.FindById(_input.Id);
        Success = _store.Update(_input);
    }
    public void Undo()
    {
        if (_before is not null) _store.Update(_before);
    }
}

public class DeletePurchaseCmd : ICommand
{
    private readonly IPurchaseStore _store;
    private readonly int _id;
    private PurchaseDto? _snapshot;
    public bool Success { get; private set; }

    public DeletePurchaseCmd(IPurchaseStore store, int id)
    {
        _store = store; _id = id;
    }
    public void Execute()
    {
        _snapshot = _store.FindById(_id);
        Success = _snapshot is not null && _store.Delete(_id);
    }
    public void Undo()
    {
        if (_snapshot is not null) _store.Create(_snapshot);
    }
}
