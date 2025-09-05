using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using FluentValidation;
using FluentValidation.Results;

#nullable enable
public class Program
{
	public static void Main()
	{
		ChangePasswordRequestDto requestDto = new() { NewPassword = string.Empty };
		ChangePasswordValidator validator = new();
		ValidationResult result = validator.Validate(requestDto);
		if (!result.IsValid)
		{
			JsonSerializerOptions options = new()
			{
				WriteIndented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};
			
			Console.WriteLine(JsonSerializer.Serialize(result.Errors, options));
		}
	}

	internal interface IHasNewPasswordRequestDto
	{
		string NewPassword { get; set; }
	}
	
	public class ChangePasswordRequestDto : IHasNewPasswordRequestDto
	{
		public required string NewPassword { get; set; }
	}
	
	public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestDto>
	{
		public ChangePasswordValidator()
		{
			Include(new NewPasswordValidator());
		}
	}
	
	internal class NewPasswordValidator : AbstractValidator<IHasNewPasswordRequestDto>
	{
		public NewPasswordValidator()
		{
			RuleFor(dto => dto.NewPassword).Length(8, 20);
		}
	}
}