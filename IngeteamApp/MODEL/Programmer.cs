using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngeteamApp.MODEL
{
    public class Programmer
    {
        public class ProgrammerCollection : ObservableCollection<Programmer>
        {


        }

        //name, surname, email address, place of residence and phone number.

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _surname;

        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _residence;

        public string Residence
        {
            get { return _residence; }
            set { _residence = value; }
        }

        private string _number;

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /*private Task _currTask;

        public Task CurrTask
        {
            get { return _currTask; } 
            set { _currTask = value; }
        }*/
        public Programmer()
        {

        }
        public Programmer (int id, string name, string surname, string email, string residence, string number)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Residence = residence;
            Number = number;

        }
        public override string ToString()
        {
            return "Programmer Number: " + this.Id.ToString() + " -- " + this.Name + " - " + this.Surname + " - " + this.Email + " - Addr: " + this.Residence + " - Tlf: " + this.Number;
        }
    }
}
