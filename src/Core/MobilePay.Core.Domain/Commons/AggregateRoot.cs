namespace MobilePay.Core.Domain.Commons;

public abstract class AggregateRoot : IInternalEventHandler
{
    public Guid Id { get; protected set; }

    private readonly List<object> _changes;

    protected AggregateRoot() => _changes = new List<object>();

    public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

    public void ClearChanges() => _changes.Clear();
}
