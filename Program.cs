using System;

// EventArgs carries data about what happened
class TemperatureChangedEventArgs : EventArgs
{
    public double OldTemp { get; }
    public double NewTemp { get; }

    public TemperatureChangedEventArgs(double old, double new_)
    {
        OldTemp = old;
        NewTemp = new_;
    }
}

class Thermometer
{
    private double _temperature;

    // Standard .NET event pattern
    public event EventHandler<TemperatureChangedEventArgs>? TemperatureChanged;

    public double Temperature
    {
        get => _temperature;
        set
        {
            if (value == _temperature) return;
            var args = new TemperatureChangedEventArgs(_temperature, value);
            _temperature = value;
            // Raise the event — null check with ?.
            TemperatureChanged?.Invoke(this, args);
        }
    }
}

class Program
{
    static void Main()
    {
        var thermo = new Thermometer();

        // Subscribe with a lambda
        thermo.TemperatureChanged += (sender, e) =>
        {
            Console.WriteLine($"Temp changed: {e.OldTemp}°C → {e.NewTemp}°C");

            if (e.NewTemp > 37.5)
                Console.WriteLine("⚠️  Fever detected!");
        };

        thermo.Temperature = 36.6;  // Temp changed: 0°C → 36.6°C
        thermo.Temperature = 37.1;  // Temp changed: 36.6°C → 37.1°C
        thermo.Temperature = 38.2;  // Temp changed: 37.1°C → 38.2°C ⚠️
    }
}