﻿namespace DatingLoveApp.Business.Dtos.AuthenticationDtos;

public class RegisterLocalUserDto
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }
}