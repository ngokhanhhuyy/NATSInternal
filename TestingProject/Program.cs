using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

internal class Program
{
	public static void Main()
	{
		Car car = new Car("red");
		car.Run();
		
		Car car2 = new Car("blue");
		car2.Run();
	}
}

internal class Car
{
	public Car(string color)
	{
		Color = color;
		PrintCreatedDateTime();
	}
	
	public string Color { get; set; }

	public void Run()
	{
		Console.WriteLine($"The car which color is ${Color} is running!");
	}

	private void PrintCreatedDateTime()
	{
		Console.WriteLine(DateTime.Now);
	}
}