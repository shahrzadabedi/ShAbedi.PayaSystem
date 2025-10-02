using AutoMapper;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.FinalizeShebaCommand;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;
using System.ComponentModel.DataAnnotations;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

[AutoMap(typeof(ConfirmShebaCommand))]
public class FinalizeShebaCommandRequest
{
    [Required]
    [RegularExpression("^(confirmed|canceled)$", ErrorMessage = "Status must be either 'Confirmed' or 'Canceled'.")]
    public string Status { get; set; }
    public string? Note { get; set; }
}
