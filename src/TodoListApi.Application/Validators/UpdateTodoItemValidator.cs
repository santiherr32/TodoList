using FluentValidation;
using TodoListApi.Application.DTOs;

namespace TodoListApi.Application.Validators;

public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemDto>
{
    public UpdateTodoItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(40).WithMessage("El título no puede superar 40 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("La descripción no puede superar 200 caracteres.")
            .When(x => x.Description is not null);

        RuleFor(x => x.MaxCompletionDate)
            .NotEmpty().WithMessage("La fecha máxima de cumplimiento es obligatoria.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("La fecha debe ser mayor o igual a hoy.");
    }
}