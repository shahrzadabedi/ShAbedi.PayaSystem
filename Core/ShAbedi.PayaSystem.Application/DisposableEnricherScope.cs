namespace ShAbedi.PayaSystem.Application;

public class DisposableEnricherScope : IDisposable
{
    private readonly List<IDisposable> _disposables;

    public DisposableEnricherScope(List<IDisposable> disposables)
    {
        _disposables = disposables;
    }

    public void Dispose()
    {
        foreach (var d in Enumerable.Reverse(_disposables))
        {
            d.Dispose();
        }
    }
}