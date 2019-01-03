using System;
using ViewT.Crediario.Domain.Main.Entities;

namespace ViewT.Crediario.Domain.Main.Interfaces
{
    public interface IPersonRepository
    {
        Person Add(Person person);
        Person GetByEmail(string email);
        Person GetByEmailAndPassword(string email, string userPassword);
        Person Update(Person person);
        Person GetBySerialKey(string serialKey);
        Person GetById(Guid personId);
    }
}