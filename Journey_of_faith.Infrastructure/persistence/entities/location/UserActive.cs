// using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Journey_of_faith.Infrastructure.identity;

namespace Journey_of_faith.Infrastructure.persistence.entities.location;


public class UserActive
{
    public int Id {get; set;}
    public bool Status {get; set;}
    public Guid ApplicationUserId {get; set;}
    public ApplicationUser ApplicationUser{get; set;}
    public string? ActiveLocation {get; set;}
    public DateTime? Timespan {get; set;}
}