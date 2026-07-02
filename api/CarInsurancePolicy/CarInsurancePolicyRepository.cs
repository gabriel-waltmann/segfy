using System;
using System.Collections.Generic;
using System.Linq;

namespace api.CarInsurancePolicy;

public class CarInsurancePolicyRepository : ICarInsurancePolicyRepository
{
    private readonly DatabaseContext _context;

    public CarInsurancePolicyRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<CarInsurancePolicyEntity> List()
    {
        return _context.Policies.ToList();
    }

    public IEnumerable<CarInsurancePolicyEntity> List(int days)
    {
        var targetDate = DateTime.Now.AddDays(days);

        return _context.Policies
            .Where(p => p.EndDate >= DateTime.Now && p.EndDate <= targetDate)
            .ToList();
    }

    public CarInsurancePolicyEntity? GetByNumber(string number)
    {
        return _context.Policies.Find(number);
    }

    public CarInsurancePolicyEntity Create(CarInsurancePolicyEntity entity)
    {
        _context.Policies.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public CarInsurancePolicyEntity? Update(CarInsurancePolicyEntity entity)
    {
        var existing = _context.Policies.Find(entity.Number);
        if (existing is null)
        {
            return null;
        }

        existing.CpfCnpj = entity.CpfCnpj;
        existing.VehiclePlate = entity.VehiclePlate;
        existing.Price = entity.Price;
        existing.StartDate = entity.StartDate;
        existing.EndDate = entity.EndDate;
        existing.Status = entity.Status;

        _context.SaveChanges();
        return existing;
    }

    public bool Delete(string number)
    {
        var existing = _context.Policies.Find(number);
        if (existing is null)
        {
            return false;
        }

        _context.Policies.Remove(existing);
        _context.SaveChanges();
        return true;
    }
}
