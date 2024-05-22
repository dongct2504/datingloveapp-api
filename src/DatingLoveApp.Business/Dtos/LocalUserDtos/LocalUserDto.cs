﻿namespace DatingLoveApp.Business.Dtos.LocalUserDtos;

public class LocalUserDto
{
    public Guid LocalUserId { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public string? Address { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }
}
