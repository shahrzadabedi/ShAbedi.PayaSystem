using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

[AutoMap(typeof(ShebaCommand))]
public class ShebaCommandRequest
{
    public long Price { get; set; }

    [RegularExpression(@"^IR\d{24}$", ErrorMessage = "FromShebaNumber must start with 'IR' followed by 24 digits.")]
    public string FromShebaNumber { get; set; }

    [RegularExpression(@"^IR\d{24}$", ErrorMessage = "FromShebaNumber must start with 'IR' followed by 24 digits.")]
    public string ToShebaNumber { get; set; }

    public string? Note { get; set; }
}
