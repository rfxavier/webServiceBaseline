using System;
using ViewT.Crediario.Domain.Main.Entities;
using ViewT.Crediario.Domain.Main.Enums;

namespace ViewT.Crediario.Domain.Tests.Unit.Main.Entities.Builders
{
    public class PersonBuilder
    {
        private Guid _personId = Guid.Empty;
        private string _name = null;
        private string _documentNumber = null;
        private string _phoneNumber = null;
        private string _email = null;
        private string _password = null;
        private string _serialKey = null;
        private string _pushToken = null;
        private Token _token = null;
        private PersonUserStatus _personUserStatus = PersonUserStatus.Active;

        public Person Build()
        {
            var person = new Person(personId: _personId, name: _name, documentNumber: _documentNumber, phoneNumber: _phoneNumber, email: _email,
                password: _password, serialKey: _serialKey);

            if (_personUserStatus == PersonUserStatus.Active)
            {
                person.EnableUser();
            }
            else if (_personUserStatus == PersonUserStatus.Inactive)
            {
                person.DisableUser();
            }

            if (_token != null)
            {
                person.SetToken(_token);
            }

            return person;
        }

        public PersonBuilder WithPersonId(Guid userId)
        {
            this._personId = userId;
            return this;
        }

        public PersonBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }

        public PersonBuilder WithEmail(string email)
        {
            this._email = email;
            return this;
        }

        public PersonBuilder WithPassword(string password)
        {
            this._password = password;
            return this;
        }

        public PersonBuilder WithToken(Token token)
        {
            this._token = token;
            return this;
        }

        public PersonBuilder WithPersonUserStatus(PersonUserStatus personUserStatus)
        {
            this._personUserStatus = personUserStatus;
            return this;
        }

        public PersonBuilder WithSerialKey(string serialKey)
        {
            this._serialKey = serialKey;
            return this;
        }

        public PersonBuilder WithPhoneNumber(string phoneNumber)
        {
            this._phoneNumber = phoneNumber;
            return this;
        }

        public PersonBuilder WithDocumentNumber(string documentNumber)
        {
            this._documentNumber = documentNumber;
            return this;
        }

        public PersonBuilder WithPushToken(string pushToken)
        {
            this._pushToken = pushToken;
            return this;
        }

        public static implicit operator Person(PersonBuilder instance)
        {
            return instance.Build();
        }
    }
}