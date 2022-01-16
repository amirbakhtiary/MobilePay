namespace MobilePay.Core.Domain.Commons;

public abstract class Entity<TId> : IInternalEventHandler
{
    readonly Action<object> _applier;

    protected Entity(Action<object> applier) => _applier = applier;
    protected Entity() { }

    public TId Id { get; protected set; }
}
