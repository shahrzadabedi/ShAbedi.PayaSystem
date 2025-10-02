using ShAbedi.PayaSystem.Domain.Entities;

namespace ShAbedi.PayaSystem.Application.Common.Contracts;

public interface IShebaCommandRepository
{
    void Add(ShebaRequest request);
    void Update(ShebaRequest request);
}