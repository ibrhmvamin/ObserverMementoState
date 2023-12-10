namespace Observer
{
    //public interface IObserver
    //{
    //    string UserName { get; set; }
    //    void Update(bool isAvailability);
    //}

    //public class CustomerObserver : IObserver
    //{
    //    public string UserName { get; set; }

    //    public CustomerObserver(string userName, ISubject subject)
    //    {
    //        UserName = userName;
    //        subject.RegisterObserver(this);
    //    }


    //    public void Update(bool isAvailability)
    //    {
    //        if (isAvailability)
    //            Console.WriteLine("Hello " + UserName + ", Product is now available on Amazon");
    //    }
    //}

    //public interface ISubject
    //{
    //    void RegisterObserver(IObserver observer);
    //    void RemoveObserver(IObserver observer);
    //    void NotifyObservers();
    //}

    //public class Subject : ISubject
    //{
    //    public List<IObserver> observers = new List<IObserver>();

    //    public string ProductName { get; set; }
    //    public double ProductPrice { get; set; }

    //    private bool _isAvailability;

    //    public bool IsAvailability
    //    {
    //        get { return _isAvailability; }
    //        set
    //        {
    //            _isAvailability = value;
    //            if (value)
    //            {
    //                Console.WriteLine("Availability changed from Out of Stock to Available.");
    //                NotifyObservers();
    //            }
    //            else
    //                Console.WriteLine("The product is not available in stock.");
    //        }
    //    }


    //    public Subject(string productName, double productPrice, bool isAvailability)
    //    {
    //        ProductName = productName;
    //        ProductPrice = productPrice;
    //        IsAvailability = isAvailability;
    //    }

    //    public void RegisterObserver(IObserver observer)
    //    {
    //        Console.WriteLine("Observer Added : " + observer.UserName);
    //        observers.Add(observer);
    //    }

    //    public void RemoveObserver(IObserver observer)
    //    {
    //        observers.Remove(observer);
    //    }

    //    public void NotifyObservers()
    //    {
    //        Console.WriteLine("Product Name :" + ProductName +
    //                          ", product Price : " + ProductPrice +
    //                          " is Now available. So notifying all Registered users ");
    //        Console.WriteLine();
    //        foreach (IObserver observer in observers)
    //        {
    //            observer.Update(IsAvailability);
    //        }
    //    }

    //}


    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Subject Iphone = new Subject("Iphone 15", 2999, false);

    //        var customer1 = new CustomerObserver("Amin", Iphone);
    //        var customer2 = new CustomerObserver("Seid", Iphone);
    //        var customer3 = new CustomerObserver("Resad", Iphone);
    //        var customer4 = new CustomerObserver("Ayxan", Iphone);
    //        var customer5 = new CustomerObserver("Maqa", Iphone);

    //        Console.WriteLine();
    //        Console.WriteLine();
    //        Console.WriteLine();
    //        Console.WriteLine();


    //        Iphone.RemoveObserver(customer3);

    //        Iphone.IsAvailability = true;



    //    }
    //}


    //    interface IState   
    //    {
    //        void Do(IDevice device);
    //    }

    //    interface IDevice
    //    {
    //        public IState State { get; set; }
    //        void PowerButton();
    //    }

    //    class Computer : IDevice
    //    {

    //        public IState State { get; set; }

    //        public Computer()
    //        {
    //            State = new OffState();
    //        }
    //        public void PowerButton()
    //            => State.Do(this);
    //    }

    //    class OffState : IState
    //    {
    //        public void Do(IDevice device)
    //        {
    //            Console.WriteLine("Computer is on");
    //            device.State = new OnState();
    //        }
    //    }

    //    class OnState : IState
    //    {
    //        public void Do(IDevice device)
    //        {
    //            Console.WriteLine("Computer is off");
    //            device.State = new OffState();
    //        }
    //    }

    //    internal class Program
    //    {
    //        static void Main(string[] args)
    //        {
    //            IDevice device = new Computer();
    //            device.PowerButton();
    //            device.PowerButton();
    //            device.PowerButton();
    //            device.PowerButton();

    //        }
    //    }


    class Originator
    {
        private string _state;

        public Originator(string state)
        {
            this._state = state;
            Console.WriteLine("Originator: My initial state is: " + state);
        }

       public void DoSomething()
        {
            Console.WriteLine("Originator: I'm doing something important.");
            this._state = this.GenerateRandomString(30);
            Console.WriteLine($"Originator: and my state has changed to: {_state}");
        }

        private string GenerateRandomString(int length = 10)
        {
            string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;

            while (length > 0)
            {
                result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];

                Thread.Sleep(12);

                length--;
            }

            return result;
        }

       public IMemento Save()
        {
            return new ConcreteMemento(this._state);
        }

        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            this._state = memento.GetState();
            Console.Write($"Originator: My state has changed to: {_state}");
        }
    }

    public interface IMemento
    {
        string GetName();

        string GetState();

        DateTime GetDate();
    }

    class ConcreteMemento : IMemento
    {
        private string _state;

        private DateTime _date;

        public ConcreteMemento(string state)
        {
            this._state = state;
            this._date = DateTime.Now;
        }

        public string GetState()
        {
            return this._state;
        }

       public string GetName()
        {
            return $"{this._date} / ({this._state.Substring(0, 9)})...";
        }

        public DateTime GetDate()
        {
            return this._date;
        }
    }

    class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private Originator _originator = null;

        public Caretaker(Originator originator)
        {
            this._originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("\nCaretaker: Saving Originator's state...");
            this._mementos.Add(this._originator.Save());
        }

        public void Undo()
        {
            if (this._mementos.Count == 0)
            {
                return;
            }

            var memento = this._mementos.Last();
            this._mementos.Remove(memento);

            Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

            try
            {
                this._originator.Restore(memento);
            }
            catch (Exception)
            {
                this.Undo();
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("Caretaker: Here's the list of mementos:");

            foreach (var memento in this._mementos)
            {
                Console.WriteLine(memento.GetName());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Originator originator = new Originator("Super-duper-super-puper-super.");
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            Console.WriteLine();
            caretaker.ShowHistory();

            Console.WriteLine("\nClient: Now, let's rollback!\n");
            caretaker.Undo();

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine();
        }
    }

}