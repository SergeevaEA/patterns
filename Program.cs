using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternsSharp
{
    interface ICommand
    {
        void Positive();
        void Negative();
    }

    class Conveyor
    {
        public void On() => Console.WriteLine("Конвейер запущен");
        public void Off() => Console.WriteLine("Конвейер остановлен");
        public void SpeedIncrease() => Console.WriteLine("Увеличена скорость конвейера");
        public void SpeedDecrease() => Console.WriteLine("Снижена скорость конвейера");
    }

    class ConveyorWorkCommand : ICommand
    {
        private Conveyor conveyor;
        public ConveyorWorkCommand(Conveyor conveyor) => this.conveyor = conveyor;
        public void Positive() => conveyor.On();
        public void Negative() => conveyor.Off();
    }


    class ConveyorAdjustCommand : ICommand
    {
        private Conveyor conveyor;
        public ConveyorAdjustCommand(Conveyor conveyor) => this.conveyor = conveyor;
        public void Positive() => conveyor.SpeedIncrease();
        public void Negative() => conveyor.SpeedDecrease();
    }

    class Multipult
    {
        private List<ICommand> commands;
        private Stack<ICommand> history;

        public Multipult()
        {
            commands = new List<ICommand>() { null, null };
            history = new Stack<ICommand>();
        }

        public void SetCommand(int button, ICommand command) => commands[button] = command;
        public void PressOn(int button)
        {
            commands[button].Positive();
            history.Push(commands[button]);
        }

        public void PressCancel()
        {
            if (history.Count != 0)
            {
                history.Pop().Negative();
            }
        }
    }
    interface IProduction
    {
        void Release();
    }

    class IceCreamStick : IProduction
    {
        public void Release() => Console.WriteLine("Выпущено мороженое на палочке");
    }

    class IceCreamCone : IProduction
    {
        public void Release() => Console.WriteLine("Выпущено мороженое в вафельном рожке");
    }

    interface IWorkShop
    {
        IProduction Create();
    }

    class IceCreamStickWorkShop : IWorkShop
    {
        public IProduction Create() => new IceCreamStick();
    }

    class IceCreamConeWorkShop : IWorkShop
    {
        public IProduction Create() => new IceCreamCone();
    }

    interface IPrice
    {
        float GetPrice();
    }

    class IceCreamStickPrice : IPrice
    {
        private float price;
        public IceCreamStickPrice(float p) => this.price = p;
        public float GetPrice() => price;
    }

    class IceCreamConePrice : IPrice
    {
        private float price;
        public IceCreamConePrice(float p) => this.price = p;
        public float GetPrice() => price;
    }

    class AdapterPrice : IPrice
    {
        IceCreamConePrice conePrice;
        public AdapterPrice(IceCreamConePrice conePrice) => this.conePrice = conePrice;
        public float GetPrice() => conePrice.GetPrice() / 0.012f;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Conveyor conveyor = new Conveyor();
            Multipult multipult = new Multipult();
            multipult.SetCommand(0, new ConveyorWorkCommand(conveyor));
            multipult.SetCommand(1, new ConveyorAdjustCommand(conveyor));

            multipult.PressOn(0);

            IWorkShop creator = new IceCreamStickWorkShop();
            IProduction iceCreamStick = creator.Create();

            iceCreamStick.Release();

            multipult.PressOn(1);

            creator = new IceCreamConeWorkShop();
            IProduction iceCreamCone = creator.Create();

            iceCreamCone.Release();

            multipult.PressCancel();
            multipult.PressCancel();

            

            float rub = 90f;
            float usd = 1.08f;

            IPrice rubPrice = new IceCreamStickPrice(rub);
            IPrice usdPrice = new AdapterPrice(new IceCreamConePrice(usd));

            Console.WriteLine("Цена мороженого на палочке в рублях: " + rubPrice.GetPrice()); // rub
            Console.WriteLine("Цена мороженого в вафельном рожке в рублях: " + usdPrice.GetPrice()); // rub
        }
    }
}
