namespace Rhizine.Core.Services.Interfaces;

public interface IPersistAndRestoreService
{
    void RestoreData();

    void PersistData();
}