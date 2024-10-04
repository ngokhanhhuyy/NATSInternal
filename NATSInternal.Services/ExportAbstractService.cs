namespace NATSInternal.Services;

internal abstract class ExportAbstractService
{
    protected abstract DbSet<> _context;
}