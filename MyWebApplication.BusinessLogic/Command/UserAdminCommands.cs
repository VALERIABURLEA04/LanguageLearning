using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Command;

/// <summary>
/// Promotes / demotes a user's IsAdmin flag.
/// Memento — previous state captured so Undo can restore it.
/// </summary>
public class SetUserAdminCommand : ICommand
{
    private readonly IUserStore _store;
    private readonly int _userId;
    private readonly bool _targetAdmin;
    private bool? _previousAdmin;
    public bool Success { get; private set; }

    public SetUserAdminCommand(IUserStore store, int userId, bool targetAdmin)
    {
        _store = store;
        _userId = userId;
        _targetAdmin = targetAdmin;
    }

    public void Execute()
    {
        var user = _store.FindById(_userId);
        if (user is null) return;
        _previousAdmin = user.IsAdmin;
        Success = _store.SetAdminStatus(_userId, _targetAdmin);
    }

    public void Undo()
    {
        if (_previousAdmin.HasValue) _store.SetAdminStatus(_userId, _previousAdmin.Value);
    }
}

/// <summary>
/// Hard-delete a user. Memento keeps a copy so Undo can recreate.
/// </summary>
public class DeleteUserCommand : ICommand
{
    private readonly IUserStore _store;
    private readonly int _userId;
    private UserDto? _snapshot;
    public bool Success { get; private set; }

    public DeleteUserCommand(IUserStore store, int userId)
    {
        _store = store;
        _userId = userId;
    }

    public void Execute()
    {
        _snapshot = _store.FindById(_userId);
        Success = _snapshot is not null && _store.Delete(_userId);
    }

    public void Undo()
    {
        if (_snapshot is not null)
            _store.Create(_snapshot.Name, _snapshot.Email, _snapshot.PasswordHash, _snapshot.IsAdmin);
    }
}
